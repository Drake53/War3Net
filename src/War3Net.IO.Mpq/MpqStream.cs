// ------------------------------------------------------------------------------
// <copyright file="MpqStream.cs" company="Foole (fooleau@gmail.com)">
// Copyright (c) 2006 Foole (fooleau@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

#define WITH_ZLIB
#define WITH_BZIP

using System;
using System.IO;
using System.Text;

#if WITH_ZLIB
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
#endif
#if WITH_BZIP
using ICSharpCode.SharpZipLib.BZip2;
#endif

namespace War3Net.IO.Mpq
{
    /// <summary>
    /// A Stream based class for reading a file from an <see cref="MpqArchive"/>.
    /// </summary>
    public class MpqStream : Stream
    {
        private readonly Stream _stream;
        private readonly int _blockSize;

        private readonly MpqEntry _entry;
        private uint[] _blockPositions;

        private long _position;
        private byte[] _currentData;
        private int _currentBlockIndex = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqStream"/> class.
        /// </summary>
        /// <param name="archive"></param>
        /// <param name="entry"></param>
        internal MpqStream(MpqArchive archive, MpqEntry entry)
        {
            _entry = entry;

            _stream = archive.BaseStream;
            _blockSize = archive.BlockSize;

            if (_entry.IsCompressed && !_entry.IsSingleUnit)
            {
                LoadBlockPositions();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqStream"/> class.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="baseStream"></param>
        /// <param name="blockSize"></param>
        internal MpqStream(MpqEntry entry, Stream baseStream, int blockSize)
        {
            _entry = entry;

            _stream = baseStream;
            _blockSize = blockSize;

            if (_entry.IsCompressed && !_entry.IsSingleUnit)
            {
                LoadBlockPositions();
            }
        }

        // Compressed files start with an array of offsets to make seeking possible
        private void LoadBlockPositions()
        {
            var blockposcount = (int)((_entry.FileSize + _blockSize - 1) / _blockSize) + 1;

            // Files with metadata have an extra block containing block checksums
            if ((_entry.Flags & MpqFileFlags.FileHasMetadata) != 0)
            {
                blockposcount++;
            }

            _blockPositions = new uint[blockposcount];

            lock (_stream)
            {
                _stream.Seek((uint)_entry.FilePosition, SeekOrigin.Begin);
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

            if (_entry.IsEncrypted)
            {
                if (_entry.EncryptionSeed == 0)
                {
                    // This should only happen when the file name is not known.
                    if (!_entry.TryUpdateEncryptionSeed(_blockPositions[0], _blockPositions[1], blockpossize))
                    {
                        throw new MpqParserException("Unable to determine encyption seed");
                    }
                }

                StormBuffer.DecryptBlock(_blockPositions, _entry.EncryptionSeed - 1);

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

        private byte[] LoadBlock(int blockIndex, int expectedLength)
        {
            uint offset;
            int toread;
            uint encryptionseed;

            if (_entry.IsCompressed)
            {
                offset = _blockPositions[blockIndex];
                toread = (int)(_blockPositions[blockIndex + 1] - offset);
            }
            else
            {
                offset = (uint)(blockIndex * _blockSize);
                toread = expectedLength;
            }

            offset += (uint)_entry.FilePosition;

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

            if (_entry.IsEncrypted && _entry.FileSize > 3)
            {
                if (_entry.EncryptionSeed == 0)
                {
                    throw new MpqParserException("Unable to determine encryption key");
                }

                encryptionseed = (uint)(blockIndex + _entry.EncryptionSeed);
                StormBuffer.DecryptBlock(data, encryptionseed);
            }

            if (_entry.IsCompressed && (toread != expectedLength))
            {
                data = (_entry.Flags & MpqFileFlags.CompressedMulti) != 0
                    ? DecompressMulti(data, expectedLength)
                    : PKDecompress(new MemoryStream(data), expectedLength);
            }

            return data;
        }

        #region Stream overrides
        /// <inheritdoc/>
        public override bool CanRead => true;

        /// <inheritdoc/>
        public override bool CanSeek => true;

        /// <inheritdoc/>
        public override bool CanWrite => false;

        /// <inheritdoc/>
        public override long Length => _entry.FileSize;

        /// <inheritdoc/>
        public override long Position
        {
            get => _position;
            set => Seek(value, SeekOrigin.Begin);
        }

        /// <inheritdoc/>
        public override void Flush()
        {
            // NOP
        }

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin)
        {
            long target;

            switch (origin)
            {
                case SeekOrigin.Begin:
                    target = offset;
                    break;
                case SeekOrigin.Current:
                    target = Position + offset;
                    break;
                case SeekOrigin.End:
                    target = Length + offset;
                    break;
                default:
                    throw new ArgumentException("Invalid SeekOrigin", nameof(origin));
            }

            if (target < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), "Attmpted to Seek before the beginning of the stream");
            }

            if (target >= Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), "Attmpted to Seek beyond the end of the stream");
            }

            _position = target;

            return _position;
        }

        /// <inheritdoc/>
        public override void SetLength(long value)
        {
            throw new NotSupportedException("SetLength is not supported");
        }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_entry.IsSingleUnit)
            {
                return ReadInternalSingleUnit(buffer, offset, count);
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

        // SingleUnit entries can be compressed but are never encrypted
        private int ReadInternalSingleUnit(byte[] buffer, int offset, int count)
        {
            if (_position >= Length)
            {
                return 0;
            }

            if (_currentData == null)
            {
                LoadSingleUnit();
            }

            var bytestocopy = Math.Min((int)(_currentData.Length - _position), count);

            Array.Copy(_currentData, _position, buffer, offset, bytestocopy);

            _position += bytestocopy;
            return bytestocopy;
        }

        private void LoadSingleUnit()
        {
            // Read the entire file into memory
            var filedata = new byte[_entry.CompressedSize];
            lock (_stream)
            {
                _stream.Seek((uint)_entry.FilePosition, SeekOrigin.Begin);
                var read = _stream.Read(filedata, 0, filedata.Length);
                if (read != filedata.Length)
                {
                    throw new MpqParserException("Insufficient data or invalid data length");
                }
            }

            if (_entry.CompressedSize == _entry.FileSize)
            {
                _currentData = filedata;
            }
            else
            {
                _currentData = DecompressMulti(filedata, (int)_entry.FileSize);
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

            var localposition = (int)(_position % _blockSize);
            var bytestocopy = Math.Min(_currentData.Length - localposition, count);
            if (bytestocopy <= 0)
            {
                return 0;
            }

            Array.Copy(_currentData, localposition, buffer, offset, bytestocopy);

            _position += bytestocopy;
            return bytestocopy;
        }

        public override int ReadByte()
        {
            if (_position >= Length)
            {
                return -1;
            }

            if (_entry.IsSingleUnit)
            {
                return ReadByteSingleUnit();
            }

            BufferData();

            var localposition = (int)(_position % _blockSize);
            _position++;
            return _currentData[localposition];
        }

        private int ReadByteSingleUnit()
        {
            if (_currentData == null)
            {
                LoadSingleUnit();
            }

            return _currentData[_position++];
        }

        private void BufferData()
        {
            var requiredblock = (int)(_position / _blockSize);
            if (requiredblock != _currentBlockIndex)
            {
                var expectedlength = (int)Math.Min(Length - (requiredblock * _blockSize), _blockSize);
                _currentData = LoadBlock(requiredblock, expectedlength);
                _currentBlockIndex = requiredblock;
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException("Writing is not supported");
        }
        #endregion Stream overrides

        private static byte[] DecompressMulti(byte[] input, int outputLength)
        {
            using (Stream sinput = new MemoryStream(input))
            {
                var comptype = (MpqCompressionType)sinput.ReadByte();

                // WC3 onward mosly use Zlib
                // Starcraft 1 mostly uses PKLib, plus types 41 and 81 for audio files
                switch (comptype)
                {
                    case MpqCompressionType.Huffman:
                        using (var huffman = MpqHuffman.Decompress(sinput))
                        {
                            return huffman.ToArray();
                        }

                    case MpqCompressionType.ZLib:
#if WITH_ZLIB
                        return ZlibDecompress(sinput, outputLength);
#endif

                    case MpqCompressionType.PKLib:
                        return PKDecompress(sinput, outputLength);

                    case MpqCompressionType.BZip2:
#if WITH_BZIP
                        return BZip2Decompress(sinput, outputLength);
#endif

                    case MpqCompressionType.ImaAdpcmStereo:
                        return MpqWavCompression.Decompress(sinput, 2);

                    case MpqCompressionType.ImaAdpcmMono:
                        return MpqWavCompression.Decompress(sinput, 1);

                    case MpqCompressionType.Lzma:
                        // TODO: LZMA
                        throw new MpqParserException("LZMA compression is not yet supported");

                    case MpqCompressionType.Sparse | MpqCompressionType.ZLib:
                        // TODO: sparse then zlib
                        throw new MpqParserException("Sparse compression + Deflate compression is not yet supported");

                    case MpqCompressionType.Sparse | MpqCompressionType.BZip2:
                        // TODO: sparse then bzip2
                        throw new MpqParserException("Sparse compression + BZip2 compression is not yet supported");

                    case MpqCompressionType.ImaAdpcmMono | MpqCompressionType.Huffman:
                        return MpqWavCompression.Decompress(MpqHuffman.Decompress(sinput), 1);

                    case MpqCompressionType.ImaAdpcmMono | MpqCompressionType.PKLib:
                        return MpqWavCompression.Decompress(new MemoryStream(PKDecompress(sinput, outputLength)), 1);

                    case MpqCompressionType.ImaAdpcmStereo | MpqCompressionType.Huffman:
                        return MpqWavCompression.Decompress(MpqHuffman.Decompress(sinput), 2);

                    case MpqCompressionType.ImaAdpcmStereo | MpqCompressionType.PKLib:
                        return MpqWavCompression.Decompress(new MemoryStream(PKDecompress(sinput, outputLength)), 2);

                    default:
                        throw new MpqParserException("Compression is not yet supported: 0x" + comptype.ToString("X"));
                }
            }
        }

#if WITH_BZIP
        private static byte[] BZip2Decompress(Stream data, int expectedLength)
        {
            using (var output = new MemoryStream(expectedLength))
            {
                BZip2.Decompress(data, output, false);
                return output.ToArray();
            }
        }
#endif

        private static byte[] PKDecompress(Stream data, int expectedLength)
        {
            var position = data.Position;
            try
            {
                var pk = new PKLibDecompress(data);
                return pk.Explode(expectedLength);
            }
            catch (InvalidDataException previousException)
            {
                data.Position = position + 3;

                using (var reader = new BinaryReader(data))
                {
                    var expectedStreamLength = reader.ReadUInt32();
                    if (expectedStreamLength != data.Length)
                    {
                        throw new InvalidDataException("Unexpected stream length value", previousException);
                    }

                    if (expectedLength + 8 == expectedStreamLength)
                    {
                        // Assume data is not compressed.
                        return reader.ReadBytes(expectedLength);
                    }

                    var comptype = (MpqCompressionType)reader.ReadByte();
                    if (comptype != MpqCompressionType.ZLib)
                    {
                        throw new NotImplementedException();
                    }

                    return ZlibDecompress(data, expectedLength);
                }
            }
        }

#if WITH_ZLIB
        private static byte[] ZlibDecompress(Stream data, int expectedLength)
        {
            // This assumes that Zlib won't be used in combination with another compression type
            var output = new byte[expectedLength];
            Stream s = new InflaterInputStream(data);
            var offset = 0;
            while (expectedLength > 0)
            {
                var size = s.Read(output, offset, expectedLength);
                if (size == 0)
                {
                    break;
                }

                offset += size;
                expectedLength -= size;
            }

            return output;
        }
#endif
    }
}