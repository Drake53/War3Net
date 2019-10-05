// ------------------------------------------------------------------------------
// <copyright file="MpqFile.cs" company="Foole (fooleau@gmail.com)">
// Copyright (c) 2006 Foole (fooleau@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

#define PK_COMPRESS

using System;
using System.IO;

namespace War3Net.IO.Mpq
{
    public class MpqFile : IEquatable<MpqFile>, IDisposable
    {
        private readonly Stream _baseStream;
        private readonly MpqEntry _entry;
        private readonly uint _hashCollisions; // possible amount of collisions this unknown file had in old archive
        private readonly int _blockSize; // used for compression
        private readonly string _fileName;

        private MemoryStream _compressedStream;
        private MpqHash? _hash;
        private uint _hashIndex; // position in hashtable

        private MpqLocale _locale;

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqFile"/> class.
        /// </summary>
        /// <param name="sourceStream"></param>
        /// <param name="fileName"></param>
        /// <param name="flags"></param>
        /// <param name="blockSize"></param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="sourceStream"/> argument is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="blockSize"/> is invalid.</exception>
        public MpqFile(Stream sourceStream, string fileName, MpqLocale locale, MpqFileFlags flags, ushort blockSize)
        {
            // TODO: copy stream?
            _baseStream = sourceStream ?? throw new ArgumentNullException(nameof(sourceStream));
            _baseStream.Position = 0;
            _fileName = fileName;

            _blockSize = 0x200 << ((blockSize < 0 || blockSize > 22) ? throw new ArgumentOutOfRangeException(nameof(blockSize)) : blockSize);

            var fileSize = (uint)_baseStream.Length;
            var compressedSize = ((flags & MpqFileFlags.Compressed) != 0)
                ? Compress(flags.HasFlag(MpqFileFlags.SingleUnit))
                : fileSize;

            _entry = new MpqEntry(fileName, compressedSize, fileSize, flags);
            _locale = locale;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqFile"/> class, for which the filename is unknown.
        /// </summary>
        /// <param name="sourceStream"></param>
        /// <param name="mpqHash"></param>
        /// <param name="hashIndex"></param>
        /// <param name="hashCollisions"></param>
        /// <param name="flags"></param>
        /// <param name="blockSize"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="sourceStream"/> argument is null.</exception>
        public MpqFile(Stream sourceStream, MpqHash mpqHash, uint hashIndex, uint hashCollisions, MpqFileFlags flags, ushort blockSize)
            : this(sourceStream, null, mpqHash.Locale, flags, blockSize)
        {
            if (mpqHash.Mask == 0)
            {
                throw new ArgumentException("Expected the Mask value of mpqHash argument to be set to a non-zero value.", nameof(mpqHash));
            }

            _hash = mpqHash;
            _hashIndex = hashIndex;
            _hashCollisions = hashCollisions;
        }

        /// <summary>
        /// 
        /// </summary>
        public MpqEntry MpqEntry => _entry;

        /// <summary>
        /// 
        /// </summary>
        public MpqHash MpqHash => _hash.Value;

        /// <summary>
        /// 
        /// </summary>
        public uint HashIndex => _hashIndex;

        /// <summary>
        /// 
        /// </summary>
        public uint HashCollisions => _hashCollisions;

        /// <summary>
        /// 
        /// </summary>
        public Stream BaseStream => _baseStream;

        /// <summary>
        /// 
        /// </summary>
        public MemoryStream MemoryStream => _compressedStream;

        /// <summary>
        /// Gets the filename of this <see cref="MpqFile"/>.
        /// </summary>
        public string Name => _fileName;

        public void AddToArchive(uint headerOffset, uint index, uint filePos, uint mask)
        {
            // TODO: verify that blocksize of mpqfile and mpqarchive to which it gets added are the same, otherwise throw an exception

            _entry.SetPos(headerOffset, filePos);

            // This file came from another archive, and has an unknown filename.
            if (_hash.HasValue)
            {
                // Overwrite blockIndex from old archive.
                var hash = _hash.Value;
                _hash = new MpqHash(hash.Name1, hash.Name2, hash.Locale, index, hash.Mask);
            }
            else
            {
                _hash = new MpqHash(_fileName, mask, _locale, index);
                _hashIndex = MpqHash.GetIndex(_fileName, mask);
            }
        }

        /*public void WriteToStream( Stream stream )
        {
            WriteToStream( new BinaryWriter( stream ) );
            //WriteToStream( new StreamWriter( stream ) );
        }*/

        public void SerializeTo(BinaryWriter writer, bool dispose = true)
        {
            var stream = _entry.IsCompressed ? _compressedStream : _baseStream;

            if (_entry.IsEncrypted)
            {
                var blockPosCount = (uint)( ( (int)_baseStream.Length + _blockSize - 1 ) / _blockSize ) + 1;
                var blockPositions = new int[blockPosCount];
                if (_entry.IsCompressed)
                {
                    for (var blockIndex = 0; blockIndex < blockPosCount; blockIndex++)
                    {
                        using (var br = new BinaryReader(stream, new System.Text.UTF8Encoding(), true))
                        {
                            for (var i = 0; i < blockPosCount; i++)
                            {
                                blockPositions[i] = (int)br.ReadUInt32();
                            }
                        }

                        stream.Seek(0, SeekOrigin.Begin);
                    }
                }
                else
                {
                    for (var blockIndex = 1; blockIndex < blockPosCount; blockIndex++)
                    {
                        blockPositions[blockIndex - 1] = _blockSize * (blockIndex - 1);
                    }

                    blockPositions[blockPosCount - 1] = (int)_baseStream.Length;
                }

                var currentOffset = 0;
                for (var blockIndex = _entry.IsCompressed ? 0 : 1; blockIndex < blockPosCount; blockIndex++)
                {
                    var toWrite = (int)blockPositions[blockIndex] - currentOffset;

                    var data = StormBuffer.EncryptStream(stream, (uint)(_entry.EncryptionSeed + blockIndex - 1), currentOffset, toWrite);
                    for (var b = 0; b < data.Length; b++)
                    {
                        writer.Write(data[b]);
                    }

                    currentOffset += toWrite;
                }
            }
            else
            {
                WriteStreamToWriter(stream, writer);
            }

            if (dispose)
            {
                Dispose();
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (_entry.IsCompressed)
            {
                _compressedStream.Dispose();
            }

            _baseStream.Dispose();
        }

        bool IEquatable<MpqFile>.Equals(MpqFile other)
        {
            return StringComparer.OrdinalIgnoreCase.Compare(_fileName, other._fileName) == 0;
        }

        private uint Compress(bool singleUnit)
        {
            _compressedStream = new MemoryStream();

            if (singleUnit)
            {
                // TODO: support other compression algorithms
#if PK_COMPRESS
                Compression.Deflate.TryCompress(_baseStream, _compressedStream, (uint)_baseStream.Length, false);
#else
                Compression.Deflate.CompressTo(_baseStream, _compressedStream, (int)_baseStream.Length, false);
#endif
            }
            else
            {
                //var blockSize = _archive?.BlockSize ?? _blockSize;
                var blockCount = (uint)( ( (int)_baseStream.Length + _blockSize - 1 ) / _blockSize ) + 1;
                var blockOffsets = new uint[blockCount];

                blockOffsets[0] = 4 * blockCount;
                _compressedStream.Position = blockOffsets[0];

                for (var blockIndex = 1; blockIndex < blockCount; blockIndex++)
                {
                    // TODO: support other compression algorithms
#if PK_COMPRESS
                    blockOffsets[blockIndex] = Compression.Deflate.TryCompress(_baseStream, _compressedStream, (uint)_blockSize, true);
#else
                    blockOffsets[blockIndex] = Compression.Deflate.CompressTo(_baseStream, _compressedStream, _blockSize, true);
#endif
                }

                _baseStream.Dispose();

                _compressedStream.Position = 0;
                using (var writer = new BinaryWriter(_compressedStream, new System.Text.UTF8Encoding(false, true), true))
                {
                    for (var blockIndex = 0; blockIndex < blockCount; blockIndex++)
                    {
                        writer.Write(blockOffsets[blockIndex]);
                    }
                }
            }

            _compressedStream.Position = 0;
            return (uint)_compressedStream.Length;
        }

        private void CopyToCompressedStream(Stream source, byte compressionType)
        {
            source.Position = 0;

            // First byte in the block indicates the compression algorithm used.
            _compressedStream.WriteByte(compressionType);

            while (true)
            {
                var read = source.ReadByte();
                if (read == -1)
                {
                    break;
                }

                _compressedStream.WriteByte((byte)read);
            }
        }

        private void WriteStreamToWriter(Stream source, BinaryWriter writer)
        {
            while (true)
            {
                var read = source.ReadByte();
                if (read == -1)
                {
                    break;
                }

                writer.Write((byte)read);
            }
        }
    }
}