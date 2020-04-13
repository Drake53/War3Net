// ------------------------------------------------------------------------------
// <copyright file="MpqStream.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;

using War3Net.Common.Extensions;
using War3Net.IO.Compression;

namespace War3Net.IO.Mpq
{
    /// <summary>
    /// A Stream based class for reading a file from an <see cref="MpqArchive"/>.
    /// </summary>
    public class MpqStream : Stream
    {
        private readonly Stream _stream;
        private readonly int _blockSize;
        private readonly MpqStreamMode _mode;
        private readonly uint[] _blockPositions;
        private readonly bool _isStreamOwner;

        // MpqEntry data
        private readonly uint _filePosition;
        private readonly uint _fileSize;
        private readonly uint _compressedSize;
        private readonly MpqFileFlags _flags;
        private readonly bool _isCompressed;
        private readonly bool _isEncrypted;
        private readonly bool _isSingleUnit;
        private readonly uint _encryptionSeed;
        private readonly uint _baseEncryptionSeed;

        private byte[]? _currentData;
        private long _position;
        private int _currentBlockIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqStream"/> class.
        /// </summary>
        /// <param name="archive">The archive from which to load a file.</param>
        /// <param name="entry">The file's entry in the <see cref="BlockTable"/>.</param>
        internal MpqStream(MpqArchive archive, MpqEntry entry)
            : this(entry, archive.BaseStream, archive.BlockSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqStream"/> class.
        /// </summary>
        /// <param name="entry">The file's entry in the <see cref="BlockTable"/>.</param>
        /// <param name="baseStream">The <see cref="MpqArchive"/>'s stream.</param>
        /// <param name="blockSize">The <see cref="MpqArchive.BlockSize"/>.</param>
        internal MpqStream(MpqEntry entry, Stream baseStream, int blockSize)
        {
            _mode = MpqStreamMode.Read;
            _isStreamOwner = false;

            _filePosition = entry.FilePosition;
            _fileSize = entry.FileSize;
            _compressedSize = entry.CompressedSize;
            _flags = entry.Flags;
            _isCompressed = (_flags & MpqFileFlags.Compressed) != 0;
            _isEncrypted = _flags.HasFlag(MpqFileFlags.Encrypted);
            _isSingleUnit = _flags.HasFlag(MpqFileFlags.SingleUnit);

            _encryptionSeed = entry.EncryptionSeed;
            _baseEncryptionSeed = entry.BaseEncryptionSeed;

            _stream = baseStream;
            _blockSize = blockSize;

            if (_isSingleUnit)
            {
                // Read the entire file into memory
                var filedata = new byte[_compressedSize];
                lock (_stream)
                {
                    _stream.Seek(_filePosition, SeekOrigin.Begin);
                    var read = _stream.Read(filedata, 0, filedata.Length);
                    if (read != filedata.Length)
                    {
                        throw new MpqParserException("Insufficient data or invalid data length");
                    }
                }

                if (_isEncrypted && _fileSize > 3)
                {
                    if (_encryptionSeed == 0)
                    {
                        throw new MpqParserException("Unable to determine encryption key");
                    }

                    StormBuffer.DecryptBlock(filedata, _encryptionSeed);
                }

                _currentData = _flags.HasFlag(MpqFileFlags.CompressedMulti) && _compressedSize > 0
                    ? DecompressMulti(filedata, _fileSize)
                    : filedata;
            }
            else
            {
                _currentBlockIndex = -1;

                // Compressed files start with an array of offsets to make seeking possible
                if (_isCompressed)
                {
                    var blockposcount = (int)((_fileSize + _blockSize - 1) / _blockSize) + 1;

                    // Files with metadata have an extra block containing block checksums
                    if ((_flags & MpqFileFlags.FileHasMetadata) != 0)
                    {
                        blockposcount++;
                    }

                    _blockPositions = new uint[blockposcount];

                    lock (_stream)
                    {
                        _stream.Seek(_filePosition, SeekOrigin.Begin);
                        using (var br = new BinaryReader(_stream, new UTF8Encoding(), true))
                        {
                            for (var i = 0; i < blockposcount; i++)
                            {
                                _blockPositions[i] = br.ReadUInt32();
                            }
                        }
                    }

                    var blockpossize = (uint)blockposcount * 4;

                    /*
                    if (_blockPositions[0] != blockpossize)
                    {
                        // _entry.Flags |= MpqFileFlags.Encrypted;
                        throw new MpqParserException();
                    }
                     */

                    if (_isEncrypted && blockposcount > 1)
                    {
                        if (_encryptionSeed == 0)
                        {
                            // This should only happen when the file name is not known.
                            if (!entry.TryUpdateEncryptionSeed(_blockPositions[0], _blockPositions[1], blockpossize))
                            {
                                throw new MpqParserException("Unable to determine encyption seed");
                            }
                        }

                        _encryptionSeed = entry.EncryptionSeed;
                        _baseEncryptionSeed = entry.BaseEncryptionSeed;
                        StormBuffer.DecryptBlock(_blockPositions, _encryptionSeed - 1);

                        if (_blockPositions[0] != blockpossize)
                        {
                            throw new MpqParserException("Decryption failed");
                        }

                        if (_blockPositions[1] > _blockSize + blockpossize)
                        {
                            throw new MpqParserException("Decryption failed");
                        }
                    }
                }
            }
        }

        internal MpqStream(Stream baseStream, string? fileName, bool leaveOpen = false)
            : this(new MpqEntry(fileName, 0, 0, (uint)baseStream.Length, (uint)baseStream.Length, MpqFileFlags.Exists | MpqFileFlags.SingleUnit), baseStream, 0)
        {
            _isStreamOwner = !leaveOpen;
        }

        internal Stream Transform(MpqFileFlags targetFlags, MpqCompressionType compressionType, uint targetFilePosition, int targetBlockSize)
        {
            using var memoryStream = new MemoryStream();
            CopyTo(memoryStream);
            memoryStream.Position = 0;
            var fileSize = memoryStream.Length;

            using var compressedStream = GetCompressedStream(memoryStream, targetFlags, compressionType, targetBlockSize);
            var compressedSize = (uint)compressedStream.Length;

            var resultStream = new MemoryStream();

            var blockPosCount = (uint)(((int)fileSize + targetBlockSize - 1) / targetBlockSize) + 1;
            if (targetFlags.HasFlag(MpqFileFlags.Encrypted) && blockPosCount > 1)
            {
                var blockPositions = new int[blockPosCount];
                var singleUnit = targetFlags.HasFlag(MpqFileFlags.SingleUnit);

                var hasBlockPositions = !singleUnit && ((targetFlags & MpqFileFlags.Compressed) != 0);
                if (hasBlockPositions)
                {
                    for (var blockIndex = 0; blockIndex < blockPosCount; blockIndex++)
                    {
                        using (var br = new BinaryReader(compressedStream, new UTF8Encoding(), true))
                        {
                            for (var i = 0; i < blockPosCount; i++)
                            {
                                blockPositions[i] = (int)br.ReadUInt32();
                            }
                        }

                        compressedStream.Seek(0, SeekOrigin.Begin);
                    }
                }
                else
                {
                    if (singleUnit)
                    {
                        blockPosCount = 2;
                    }

                    blockPositions[0] = 0;
                    for (var blockIndex = 2; blockIndex < blockPosCount; blockIndex++)
                    {
                        blockPositions[blockIndex - 1] = targetBlockSize * (blockIndex - 1);
                    }

                    blockPositions[blockPosCount - 1] = (int)compressedSize;
                }

                var encryptionSeed = _baseEncryptionSeed;
                if (targetFlags.HasFlag(MpqFileFlags.BlockOffsetAdjustedKey))
                {
                    encryptionSeed = MpqEntry.AdjustEncryptionSeed(encryptionSeed, targetFilePosition, (uint)fileSize);
                }

                var currentOffset = 0;
                using (var writer = new BinaryWriter(resultStream, new UTF8Encoding(false, true), true))
                {
                    for (var blockIndex = hasBlockPositions ? 0 : 1; blockIndex < blockPosCount; blockIndex++)
                    {
                        var toWrite = blockPositions[blockIndex] - currentOffset;

                        var data = StormBuffer.EncryptStream(compressedStream, (uint)(encryptionSeed + blockIndex - 1), currentOffset, toWrite);
                        writer.Write(data);

                        currentOffset += toWrite;
                    }
                }
            }
            else
            {
                compressedStream.CopyTo(resultStream);
            }

            resultStream.Position = 0;
            return resultStream;
        }

        private Stream GetCompressedStream(Stream baseStream, MpqFileFlags targetFlags, MpqCompressionType compressionType, int targetBlockSize)
        {
            var resultStream = new MemoryStream();
            var singleUnit = targetFlags.HasFlag(MpqFileFlags.SingleUnit);

            void TryCompress(uint bytes)
            {
                var offset = baseStream.Position;
                var compressedStream = compressionType switch
                {
                    MpqCompressionType.ZLib => ZLibCompression.Compress(baseStream, (int)bytes, true),

                    _ => throw new NotSupportedException(),
                };

                // Add one because CompressionType byte not written yet.
                var length = compressedStream.Length + 1;
                if (!singleUnit && length >= bytes)
                {
                    baseStream.CopyTo(resultStream, offset, (int)bytes, StreamExtensions.DefaultBufferSize);
                }
                else
                {
                    resultStream.WriteByte((byte)compressionType);
                    compressedStream.Position = 0;
                    compressedStream.CopyTo(resultStream);
                }

                compressedStream.Dispose();

                if (singleUnit)
                {
                    baseStream.Dispose();
                }
            }

            var length = (uint)baseStream.Length;

            if ((targetFlags & MpqFileFlags.Compressed) == 0)
            {
                baseStream.CopyTo(resultStream);
            }
            else if (singleUnit)
            {
                TryCompress(length);
            }
            else
            {
                var blockCount = (uint)((length + targetBlockSize - 1) / targetBlockSize) + 1;
                var blockOffsets = new uint[blockCount];

                blockOffsets[0] = 4 * blockCount;
                resultStream.Position = blockOffsets[0];

                for (var blockIndex = 1; blockIndex < blockCount; blockIndex++)
                {
                    var bytesToCompress = blockIndex + 1 == blockCount ? (uint)(baseStream.Length - baseStream.Position) : (uint)targetBlockSize;

                    TryCompress(bytesToCompress);
                    blockOffsets[blockIndex] = (uint)resultStream.Position;
                }

                resultStream.Position = 0;
                using (var writer = new BinaryWriter(resultStream, new System.Text.UTF8Encoding(false, true), true))
                {
                    for (var blockIndex = 0; blockIndex < blockCount; blockIndex++)
                    {
                        writer.Write(blockOffsets[blockIndex]);
                    }
                }
            }

            resultStream.Position = 0;
            return resultStream;
        }

        private enum MpqStreamMode
        {
            Read = 0,
            Write = 1,
        }

        public MpqFileFlags Flags => _flags;

        public bool IsCompressed => _isCompressed;

        public bool IsEncrypted => _isEncrypted;

        public bool CanBeDecrypted => !_isEncrypted || _fileSize < 4 || _encryptionSeed != 0;

        public uint CompressedSize => _compressedSize;
        public uint FileSize => _fileSize;
        public uint FilePosition => _filePosition;
        public int BlockSize => _blockSize;

        /// <inheritdoc/>
        public override bool CanRead => _mode == MpqStreamMode.Read;

        /// <inheritdoc/>
        public override bool CanSeek => _mode == MpqStreamMode.Read;

        /// <inheritdoc/>
        public override bool CanWrite => _mode == MpqStreamMode.Write;

        /// <inheritdoc/>
        public override long Length => _fileSize;

        /// <inheritdoc/>
        public override long Position
        {
            get => _position;
            set => Seek(value, SeekOrigin.Begin);
        }

        /// <inheritdoc/>
        public override void Flush()
        {
        }

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin)
        {
            if (!CanSeek)
            {
                throw new NotSupportedException();
            }

            var target = origin switch
            {
                SeekOrigin.Begin => offset,
                SeekOrigin.Current => Position + offset,
                SeekOrigin.End => Length + offset,
                _ => throw new ArgumentException("Invalid SeekOrigin", nameof(origin)),
            };

            if (target < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), "Attempted to Seek before the beginning of the stream");
            }

            if (target > Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), "Attempted to Seek beyond the end of the stream");
            }

            return _position = target;
        }

        /// <inheritdoc/>
        public override void SetLength(long value)
        {
            throw new NotSupportedException("SetLength is not supported");
        }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!CanRead)
            {
                throw new NotSupportedException();
            }

            if (_isSingleUnit)
            {
                return ReadInternal(buffer, offset, count);
            }

            var toread = count;
            var readtotal = 0;

            while (toread > 0)
            {
                var read = ReadInternal(buffer, offset, toread);
                if (read == 0)
                {
                    break;
                }

                readtotal += read;
                offset += read;
                toread -= read;
            }

            return readtotal;
        }

        /// <inheritdoc/>
        public override int ReadByte()
        {
            if (!CanRead)
            {
                throw new NotSupportedException();
            }

            if (_position >= Length)
            {
                return -1;
            }

            BufferData();
            return _currentData![_isSingleUnit ? _position++ : (int)(_position++ & (_blockSize - 1))];
        }

        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!CanWrite)
            {
                throw new NotSupportedException();
            }

            throw new NotImplementedException();
        }

        public override void Close()
        {
            base.Close();
            if (_isStreamOwner)
            {
                _stream.Close();
            }
        }

        /// <summary>
        /// Copy the base stream, so that the contents do not get decompressed not decrypted.
        /// </summary>
        internal void CopyBaseStreamTo(Stream target)
        {
            lock (_stream)
            {
                _stream.CopyTo(target, _filePosition, (int)_compressedSize, StreamExtensions.DefaultBufferSize);
            }
        }

        private static byte[] DecompressMulti(byte[] input, uint outputLength)
        {
            using var memoryStream = new MemoryStream(input);
            return GetDecompressionFunction((MpqCompressionType)memoryStream.ReadByte(), outputLength).Invoke(memoryStream);
        }

        private static Func<Stream, byte[]> GetDecompressionFunction(MpqCompressionType compressionType, uint outputLength)
        {
            return compressionType switch
            {
                MpqCompressionType.Huffman => HuffmanCoding.Decompress,
                MpqCompressionType.ZLib => (stream) => ZLibCompression.Decompress(stream, outputLength),
                MpqCompressionType.PKLib => (stream) => PKDecompress(stream, outputLength),
                MpqCompressionType.BZip2 => (stream) => BZip2Compression.Decompress(stream, outputLength),
                MpqCompressionType.Lzma => throw new MpqParserException("LZMA compression is not yet supported"),
                MpqCompressionType.Sparse => throw new MpqParserException("Sparse compression is not yet supported"),
                MpqCompressionType.ImaAdpcmMono => (stream) => AdpcmCompression.Decompress(stream, 1),
                MpqCompressionType.ImaAdpcmStereo => (stream) => AdpcmCompression.Decompress(stream, 2),

                MpqCompressionType.Sparse | MpqCompressionType.ZLib => throw new MpqParserException("Sparse compression + Deflate compression is not yet supported"),
                MpqCompressionType.Sparse | MpqCompressionType.BZip2 => throw new MpqParserException("Sparse compression + BZip2 compression is not yet supported"),

                MpqCompressionType.ImaAdpcmMono | MpqCompressionType.Huffman => (stream) => AdpcmCompression.Decompress(HuffmanCoding.Decompress(stream), 1),
                MpqCompressionType.ImaAdpcmMono | MpqCompressionType.PKLib => (stream) => AdpcmCompression.Decompress(PKDecompress(stream, outputLength), 1),

                MpqCompressionType.ImaAdpcmStereo | MpqCompressionType.Huffman => (stream) => AdpcmCompression.Decompress(HuffmanCoding.Decompress(stream), 2),
                MpqCompressionType.ImaAdpcmStereo | MpqCompressionType.PKLib => (stream) => AdpcmCompression.Decompress(PKDecompress(stream, outputLength), 2),

                _ => throw new MpqParserException($"Compression of type 0x{compressionType.ToString("X")} is not yet supported"),
            };
        }

        private static byte[] PKDecompress(Stream data, uint expectedLength)
        {
            var b1 = data.ReadByte();
            var b2 = data.ReadByte();
            var b3 = data.ReadByte();
            if (b1 == 0 && b2 == 0 && b3 == 0)
            {
                using (var reader = new BinaryReader(data))
                {
                    var expectedStreamLength = reader.ReadUInt32();
                    if (expectedStreamLength != data.Length)
                    {
                        throw new InvalidDataException("Unexpected stream length value");
                    }

                    if (expectedLength + 8 == expectedStreamLength)
                    {
                        // Assume data is not compressed.
                        return reader.ReadBytes((int)expectedLength);
                    }

                    var comptype = (MpqCompressionType)reader.ReadByte();
                    if (comptype != MpqCompressionType.ZLib)
                    {
                        throw new NotImplementedException();
                    }

                    return ZLibCompression.Decompress(data, expectedLength);
                }
            }
            else
            {
                data.Seek(-3, SeekOrigin.Current);
                return PKLibCompression.Decompress(data, expectedLength);
            }
        }

        private int ReadInternal(byte[] buffer, int offset, int count)
        {
            // OW: avoid reading past the contents of the file
            if (_position >= Length)
            {
                return 0;
            }

            BufferData();

            var localposition = _isSingleUnit ? _position : (_position & (_blockSize - 1));
            var canRead = (int)(_currentData!.Length - localposition);
            var bytestocopy = canRead > count ? count : canRead;
            if (bytestocopy <= 0)
            {
                return 0;
            }

            Array.Copy(_currentData, localposition, buffer, offset, bytestocopy);

            _position += bytestocopy;
            return bytestocopy;
        }

        private void BufferData()
        {
            if (!_isSingleUnit)
            {
                var requiredblock = (int)(_position / _blockSize);
                if (requiredblock != _currentBlockIndex)
                {
                    var expectedlength = (uint)Math.Min(Length - (requiredblock * _blockSize), _blockSize);
                    _currentData = LoadBlock(requiredblock, expectedlength);
                    _currentBlockIndex = requiredblock;
                }
            }
        }

        private byte[] LoadBlock(int blockIndex, uint expectedLength)
        {
            uint offset;
            int toread;

            if (_isCompressed)
            {
                offset = _blockPositions[blockIndex];
                toread = (int)(_blockPositions[blockIndex + 1] - offset);
            }
            else
            {
                offset = (uint)(blockIndex * _blockSize);
                toread = (int)expectedLength;
            }

            offset += _filePosition;

            var data = new byte[toread];
            lock (_stream)
            {
                _stream.Seek(offset, SeekOrigin.Begin);
                var read = _stream.Read(data, 0, toread);
                if (read != toread)
                {
                    throw new MpqParserException("Insufficient data or invalid data length");
                }
            }

            if (_isEncrypted && _fileSize > 3)
            {
                if (_encryptionSeed == 0)
                {
                    throw new MpqParserException("Unable to determine encryption key");
                }

                StormBuffer.DecryptBlock(data, (uint)(blockIndex + _encryptionSeed));
            }

            if (_isCompressed && (toread != expectedLength))
            {
                if (toread > expectedLength)
                {
                    throw new MpqParserException("Block's compressed data is larger than decompressed, so it should have been stored in uncompressed format.");
                }

                data = _flags.HasFlag(MpqFileFlags.CompressedPK)
                    ? PKLibCompression.Decompress(data, expectedLength)
                    : DecompressMulti(data, expectedLength);
            }

            return data;
        }
    }
}