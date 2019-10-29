// ------------------------------------------------------------------------------
// <copyright file="MpqFile.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

using War3Net.IO.Compression;

namespace War3Net.IO.Mpq
{
    public class MpqFile : IEquatable<MpqFile>, IDisposable
    {
        private readonly Stream _baseStream;
        private readonly MpqEntry _entry;
        private readonly MpqLocale _locale;
        private readonly int _blockSize; // used for compression

        private readonly uint _hashCollisions; // possible amount of collisions this unknown file had in old archive
        private MpqHash? _hash;
        private uint _hashIndex; // position in hashtable

        // TODO: make private ctor to ensure that filename is only null when using the MpqHash ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqFile"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="sourceStream"/> argument is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="blockSize"/> is invalid.</exception>
        public MpqFile(Stream? sourceStream, string fileName, MpqLocale locale, MpqFileFlags flags, ushort blockSize)
        {
            _blockSize = 0x200 << ((blockSize < 0 || blockSize > 22) ? throw new ArgumentOutOfRangeException(nameof(blockSize)) : blockSize);

            _baseStream = new MemoryStream();
            sourceStream?.CopyTo(_baseStream);
            _baseStream.Position = 0;

            _entry = new MpqEntry(fileName, null, (uint)_baseStream.Length, flags);
            _locale = locale;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqFile"/> class, for which the filename is unknown.
        /// </summary>
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

        internal MpqEntry MpqEntry => _entry;

        internal MpqHash MpqHash => _hash.Value;

        internal uint HashIndex => _hashIndex;

        internal uint HashCollisions => _hashCollisions;

        /// <summary>
        /// Gets the filename of this <see cref="MpqFile"/>.
        /// </summary>
        private string Name => _entry.Filename;

        /// <inheritdoc/>
        public void Dispose()
        {
            _baseStream.Dispose();
        }

        /// <inheritdoc/>
        bool IEquatable<MpqFile>.Equals(MpqFile other)
        {
            // TODO: if Name is null, use other property (MpqHash?) to compare
            return StringComparer.OrdinalIgnoreCase.Compare(Name, other.Name) == 0;
        }

        internal void AddToArchive(uint headerOffset, uint index, uint filePos, uint mask)
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
                _hash = new MpqHash(Name, mask, _locale, index);
                _hashIndex = MpqHash.GetIndex(Name, mask);
            }
        }

        /*public void SerializeTo( Stream stream )
        {
            WriteTo( new BinaryWriter( stream ) );
        }*/

        // TODO: move compression and encryption logic to a different file (MpqStream?)
        internal void WriteTo(BinaryWriter writer, bool dispose = true)
        {
            if (_entry.CompressedSize.HasValue)
            {
                throw new InvalidOperationException("Cannot serialize MpqFile twice.");
            }

            using var compressedStream = GetCompressedStream(_baseStream, _entry.Flags, CompressionType.ZLib);
            _entry.CompressedSize = (uint)compressedStream.Length;

            var blockPosCount = (uint)(((int)_entry.FileSize + _blockSize - 1) / _blockSize) + 1;
            if (_entry.IsEncrypted && blockPosCount > 1)
            {
                var blockPositions = new int[blockPosCount];
                var singleUnit = _entry.Flags.HasFlag(MpqFileFlags.SingleUnit);

                var hasBlockPositions = !singleUnit && _entry.IsCompressed;
                if (hasBlockPositions)
                {
                    for (var blockIndex = 0; blockIndex < blockPosCount; blockIndex++)
                    {
                        using (var br = new BinaryReader(compressedStream, new System.Text.UTF8Encoding(), true))
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
                        blockPositions[blockIndex - 1] = _blockSize * (blockIndex - 1);
                    }

                    blockPositions[blockPosCount - 1] = (int)_entry.CompressedSize;
                }

                var currentOffset = 0;
                for (var blockIndex = hasBlockPositions ? 0 : 1; blockIndex < blockPosCount; blockIndex++)
                {
                    var toWrite = blockPositions[blockIndex] - currentOffset;

                    var data = StormBuffer.EncryptStream(compressedStream, (uint)(_entry.EncryptionSeed + blockIndex - 1), currentOffset, toWrite);
                    writer.Write(data);

                    currentOffset += toWrite;
                }
            }
            else
            {
                compressedStream.CopyTo(writer.BaseStream);
            }

            if (dispose)
            {
                Dispose();
            }
        }

        private Stream GetCompressedStream(Stream baseStream, MpqFileFlags flags, CompressionType compressionType)
        {
            var compressedStream = new MemoryStream();
            var length = (uint)baseStream.Length;

            if ((flags & MpqFileFlags.Compressed) == 0)
            {
                baseStream.CopyTo(compressedStream);
            }
            else if (flags.HasFlag(MpqFileFlags.SingleUnit))
            {
                _ = compressionType switch
                {
                    CompressionType.ZLib => ZLibCompression.TryCompress(baseStream, compressedStream, length, true),

                    _ => throw new NotSupportedException(),
                };
            }
            else
            {
                var blockCount = (uint)((length + _blockSize - 1) / _blockSize) + 1;
                var blockOffsets = new uint[blockCount];

                blockOffsets[0] = 4 * blockCount;
                compressedStream.Position = blockOffsets[0];

                for (var blockIndex = 1; blockIndex < blockCount; blockIndex++)
                {
                    var bytesToCompress = blockIndex + 1 == blockCount ? (uint)(baseStream.Length - baseStream.Position) : (uint)_blockSize;
                    blockOffsets[blockIndex] = compressionType switch
                    {
                        CompressionType.ZLib => ZLibCompression.TryCompress(baseStream, compressedStream, bytesToCompress, false),

                        _ => throw new NotSupportedException(),
                    };
                }

                compressedStream.Position = 0;
                using (var writer = new BinaryWriter(compressedStream, new System.Text.UTF8Encoding(false, true), true))
                {
                    for (var blockIndex = 0; blockIndex < blockCount; blockIndex++)
                    {
                        writer.Write(blockOffsets[blockIndex]);
                    }
                }
            }

            compressedStream.Position = 0;
            return compressedStream;
        }
    }
}