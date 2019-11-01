// ------------------------------------------------------------------------------
// <copyright file="MpqFile.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;

using War3Net.IO.Compression;

namespace War3Net.IO.Mpq
{
    public abstract class MpqFile : IEquatable<MpqFile>, IDisposable
    {
        private readonly Stream _baseStream;
        private MpqFileFlags _flags;
        private MpqLocale _locale;

        // TODO: move compression and encryption logic to a different file (MpqStream?)

        internal MpqFile(Stream? sourceStream, MpqFileFlags flags, MpqLocale locale)
        {
            _baseStream = new MemoryStream();
            sourceStream?.CopyTo(_baseStream);
            _baseStream.Position = 0;

            _flags = flags;
            _locale = locale;
        }

        public MpqFileFlags Flags
        {
            get => _flags;
            set
            {
                if (IsOriginalStream)
                {
                    throw new NotSupportedException("Unable to change file flags when the file's stream is potentially pre-compressed and/or -encrypted.");
                }

                if ((value & MpqFileFlags.Garbage) != 0)
                {
                    throw new ArgumentException("Invalid enum.", nameof(value));
                }

                if (value.HasFlag(MpqFileFlags.Encrypted) && EncryptionSeed is null)
                {
                    throw new ArgumentException("Cannot set encrypted flag when there is no encryption seed.", nameof(value));
                }

                _flags = value;
            }
        }

        public MpqLocale Locale
        {
            get => _locale;
            set
            {
                if (!Enum.IsDefined(typeof(MpqLocale), value))
                {
                    throw new ArgumentException("Invalid enum.", nameof(value));
                }

                _locale = value;
            }
        }

        internal abstract bool IsOriginalStream { get; }

        /// <summary>
        /// Position in the <see cref="HashTable"/>.
        /// </summary>
        internal abstract uint HashIndex { get; }

        /// <summary>
        /// Gets a value that, combined with <see cref="HashIndex"/>, represents the range of indices where the file may be placed.
        /// </summary>
        /// <remarks>
        /// This value is always zero for <see cref="MpqKnownFile"/>.
        /// For <see cref="MpqUnknownFile"/>, it depends on the <see cref="MpqHash"/>es preceding this file's hash in the <see cref="MpqArchive"/> from which the file was retrieved.
        /// </remarks>
        internal abstract uint HashCollisions { get; }

        /// <summary>
        /// The base encryption seed used to encrypt this <see cref="MpqFile"/>'s stream.
        /// </summary>
        /// <remarks>
        /// If the <see cref="MpqFile"/> has the <see cref="MpqFileFlags.BlockOffsetAdjustedKey"/> flag, this seed must be adjusted based on the file's position and size.
        /// </remarks>
        protected abstract uint? EncryptionSeed { get; }

        /// <inheritdoc/>
        public void Dispose()
        {
            _baseStream.Dispose();
        }

        /// <inheritdoc/>
        bool IEquatable<MpqFile>.Equals(MpqFile other)
        {
            // TODO: compare locale?
            return GetType() == other.GetType() ? Equals(other) : false;
        }

        internal abstract bool Equals(MpqFile other);

        internal void AddToArchive(MpqArchive mpqArchive, uint index, out MpqEntry mpqEntry, out MpqHash mpqHash)
        {
            var headerOffset = mpqArchive.HeaderOffset;
            var absoluteFileOffset = (uint)mpqArchive.BaseStream.Position;
            var relativeFileOffset = absoluteFileOffset - headerOffset;

            var fileSize = (uint)_baseStream.Length;
            var blockSize = mpqArchive.BlockSize;

            if (IsOriginalStream)
            {
                if (this is MpqEncryptedFile encryptedFile)
                {
                    if (_flags.HasFlag(MpqFileFlags.BlockOffsetAdjustedKey) && encryptedFile.FilePos != relativeFileOffset)
                    {
                        throw new Exception("Cannot copy pre-encrypted stream at the current position of the MpqArchive's stream, because the relative file position is incorrect.");
                    }

                    _baseStream.CopyTo(mpqArchive.BaseStream);
                    mpqEntry = new MpqEntry(headerOffset, relativeFileOffset, fileSize, encryptedFile.FileSize, _flags);
                    mpqHash = new MpqHash(encryptedFile.Name, _locale, index, encryptedFile.Mask);
                }
                else
                {
                    throw new NotImplementedException();
                }

                return;
            }

            using var compressedStream = GetCompressedStream(CompressionType.ZLib, blockSize);
            var compressedSize = (uint)compressedStream.Length;

            var blockPosCount = (uint)(((int)fileSize + blockSize - 1) / blockSize) + 1;
            if (_flags.HasFlag(MpqFileFlags.Encrypted) && blockPosCount > 1)
            {
                var blockPositions = new int[blockPosCount];
                var singleUnit = _flags.HasFlag(MpqFileFlags.SingleUnit);

                var hasBlockPositions = !singleUnit && ((_flags & MpqFileFlags.Compressed) != 0);
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
                        blockPositions[blockIndex - 1] = blockSize * (blockIndex - 1);
                    }

                    blockPositions[blockPosCount - 1] = (int)compressedSize;
                }

                var encryptionSeed = EncryptionSeed!.Value;
                if (_flags.HasFlag(MpqFileFlags.BlockOffsetAdjustedKey))
                {
                    encryptionSeed = MpqEntry.AdjustEncryptionSeed(encryptionSeed, relativeFileOffset, fileSize);
                }

                var currentOffset = 0;
                using (var writer = new BinaryWriter(mpqArchive.BaseStream, new UTF8Encoding(false, true), true))
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
                compressedStream.CopyTo(mpqArchive.BaseStream);
            }

            if (this is MpqKnownFile mpqKnownFile)
            {
                mpqEntry = new MpqEntry(mpqKnownFile.FileName, compressedSize, fileSize, _flags);
                mpqHash = new MpqHash(mpqKnownFile.FileName, mpqArchive.HashTableMask, _locale, index);
                mpqEntry.SetPos(headerOffset, relativeFileOffset);
            }
            else if (this is MpqUnknownFile mpqUnknownFile)
            {
                mpqEntry = new MpqEntry(headerOffset, relativeFileOffset, compressedSize, fileSize, _flags);
                mpqHash = new MpqHash(mpqUnknownFile.Name, _locale, index, mpqUnknownFile.Mask);
            }
            else
            {
                throw new NotImplementedException($"Unknown subclass of {nameof(MpqFile)}.");
            }
        }

        /*internal void AddToArchive(uint headerOffset, uint index, uint filePos, uint mask)
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
        }*/

        /*public void SerializeTo( Stream stream )
        {
            WriteTo( new BinaryWriter( stream ) );
        }*/

        /*internal void WriteTo(BinaryWriter writer, bool dispose = true)
        {
        }*/

        private Stream GetCompressedStream(CompressionType compressionType, int blockSize)
        {
            var resultStream = new MemoryStream();
            var singleUnit = _flags.HasFlag(MpqFileFlags.SingleUnit);

            void TryCompress(uint bytes)
            {
                var offset = _baseStream.Position;
                var compressedStream = new MemoryStream();

                _ = compressionType switch
                {
                    CompressionType.ZLib => ZLibCompression.CompressTo(_baseStream, compressedStream, (int)bytes, true),

                    _ => throw new NotSupportedException(),
                };

                // Add one because CompressionType byte not written yet.
                var length = compressedStream.Length + 1;
                if (length == bytes || (!singleUnit && length > bytes))
                {
                    _baseStream.CopyTo(resultStream, offset, (int)bytes, StreamExtensions.DefaultBufferSize);
                }
                else
                {
                    resultStream.WriteByte((byte)CompressionType.ZLib);
                    compressedStream.Position = 0;
                    compressedStream.CopyTo(resultStream);
                }

                compressedStream.Dispose();

                if (singleUnit)
                {
                    _baseStream.Dispose();
                }
            }

            var length = (uint)_baseStream.Length;

            if ((_flags & MpqFileFlags.Compressed) == 0)
            {
                _baseStream.CopyTo(resultStream);
            }
            else if (singleUnit)
            {
                TryCompress(length);
            }
            else
            {
                var blockCount = (uint)((length + blockSize - 1) / blockSize) + 1;
                var blockOffsets = new uint[blockCount];

                blockOffsets[0] = 4 * blockCount;
                resultStream.Position = blockOffsets[0];

                for (var blockIndex = 1; blockIndex < blockCount; blockIndex++)
                {
                    var bytesToCompress = blockIndex + 1 == blockCount ? (uint)(_baseStream.Length - _baseStream.Position) : (uint)blockSize;

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
    }
}