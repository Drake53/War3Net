// ------------------------------------------------------------------------------
// <copyright file="MpqStreamFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;

using War3Net.IO.Mpq.Extensions;

namespace War3Net.IO.Mpq
{
    public static class MpqStreamFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MpqStream"/> class.
        /// </summary>
        /// <param name="archive">The archive from which to load a file.</param>
        /// <param name="entry">The file's entry in the <see cref="BlockTable"/>.</param>
        internal static MpqStream FromArchive(MpqArchive archive, MpqEntry entry)
        {
            return archive.MemoryMappedFile is not null
                ? FromStream(archive.MemoryMappedFile.CreateViewStream(entry.FilePosition, entry.CompressedSize, MemoryMappedFileAccess.Read), entry, archive.BlockSize, leaveOpen: false)
                : FromStream(archive.BaseStream, entry, archive.BlockSize, leaveOpen: true);
        }

        internal static MpqStream FromStream(Stream baseStream, string? fileName, bool leaveOpen = false)
        {
            var entry = new MpqEntry(fileName, 0, 0, (uint)baseStream.Length, (uint)baseStream.Length, MpqFileFlags.Exists | MpqFileFlags.SingleUnit);

            return FromStream(baseStream, entry, 0, leaveOpen);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqStream"/> class.
        /// </summary>
        /// <param name="baseStream">The <see cref="MpqArchive"/>'s stream.</param>
        /// <param name="entry">The file's entry in the <see cref="BlockTable"/>.</param>
        /// <param name="blockSize">The <see cref="MpqArchive.BlockSize"/>.</param>
        /// <param name="leaveOpen">If <see langword="false"/>, the <paramref name="baseStream"/> will be closed when the returned <see cref="MpqStream"/> is closed.</param>
        internal static MpqStream FromStream(Stream baseStream, MpqEntry entry, int blockSize, bool leaveOpen = false)
        {
            var filePosition = entry.FilePosition;
            var streamOffset = baseStream is MemoryMappedViewStream ? 0 : filePosition;
            var fileSize = entry.FileSize;
            var compressedSize = entry.CompressedSize;
            var flags = entry.Flags;
            var isCompressed = (flags & MpqFileFlags.Compressed) != 0;
            var isEncrypted = flags.HasFlag(MpqFileFlags.Encrypted);
            var isSingleUnit = flags.HasFlag(MpqFileFlags.SingleUnit);

            var blockPositions = Array.Empty<uint>();
            var canRead = true;

            if (isSingleUnit)
            {
                if (!TryPeekCompressionType(
                    baseStream,
                    streamOffset,
                    compressedSize,
                    flags,
                    entry.EncryptionSeed,
                    out var mpqCompressionType))
                {
                    canRead = false;
                }
                else if (mpqCompressionType.HasValue && !mpqCompressionType.Value.IsKnownMpqCompressionType())
                {
                    canRead = false;
                }
            }
            else
            {
                // Compressed files start with an array of offsets to make seeking possible.
                if (isCompressed)
                {
                    var blockPositionsCount = (int)((fileSize + blockSize - 1) / blockSize) + 1;

                    // Files with metadata have an extra block containing block checksums.
                    if (flags.HasFlag(MpqFileFlags.FileHasMetadata))
                    {
                        blockPositionsCount++;
                    }

                    blockPositions = new uint[blockPositionsCount];

                    lock (baseStream)
                    {
                        baseStream.Seek(streamOffset, SeekOrigin.Begin);
                        using (var reader = new BinaryReader(baseStream, Encoding.UTF8, true))
                        {
                            for (var i = 0; i < blockPositions.Length; i++)
                            {
                                blockPositions[i] = reader.ReadUInt32();
                            }
                        }
                    }

                    if (isEncrypted && blockPositions.Length > 1 && !TryDecryptBlockPositions(entry, blockPositions, blockSize))
                    {
                        canRead = false;
                    }
                    else if (!ValidateBlockPositions(baseStream, streamOffset, flags, entry.EncryptionSeed, blockPositions, blockSize, fileSize))
                    {
                        canRead = false;
                    }
                }
                else if (isEncrypted && fileSize >= 4 && entry.EncryptionSeed == 0)
                {
                    canRead = false;
                }
            }

            return new MpqStream(
                baseStream,
                blockSize,
                filePosition,
                fileSize,
                compressedSize,
                flags,
                entry.EncryptionSeed,
                entry.BaseEncryptionSeed,
                blockPositions,
                canRead,
                leaveOpen);
        }

        private static bool TryDecryptBlockPositions(MpqEntry entry, uint[] blockPositions, int blockSize)
        {
            // This should only happen when the file name is not known.
            if (entry.EncryptionSeed == 0)
            {
                var expectedOffsetFirstBlock = (uint)blockPositions.Length * 4;
                var maxOffsetSecondBlock = (uint)blockSize + expectedOffsetFirstBlock;

                if (!entry.TryUpdateEncryptionSeed(blockPositions[0], blockPositions[1], expectedOffsetFirstBlock, maxOffsetSecondBlock))
                {
                    return false;
                }
            }

            StormBuffer.DecryptBlock(blockPositions, entry.EncryptionSeed - 1);
            return true;
        }

        private static bool ValidateBlockPositions(
            Stream stream,
            uint streamOffset,
            MpqFileFlags flags,
            uint encryptionSeed,
            uint[] blockPositions,
            int blockSize,
            uint fileSize)
        {
            var currentPosition = blockPositions[0];
            for (var i = 1; i < blockPositions.Length; i++)
            {
                var currentBlockSize = blockPositions[i] - currentPosition;

                if (currentBlockSize <= 0 || currentBlockSize > blockSize)
                {
                    return false;
                }

                var expectedLength = Math.Min((int)(fileSize - ((i - 1) * blockSize)), blockSize);
                if (!TryPeekCompressionType(
                    stream,
                    streamOffset,
                    flags,
                    encryptionSeed,
                    blockPositions,
                    i - 1,
                    expectedLength,
                    out var mpqCompressionType))
                {
                    return false;
                }

                if (mpqCompressionType.HasValue && !mpqCompressionType.Value.IsKnownMpqCompressionType())
                {
                    return false;
                }

                currentPosition = blockPositions[i];
            }

            return true;
        }

        private static bool TryPeekCompressionType(
            Stream stream,
            uint streamOffset,
            uint compressedSize,
            MpqFileFlags flags,
            uint encryptionSeed,
            out MpqCompressionType? mpqCompressionType)
        {
            var bufferSize = Math.Min((int)compressedSize, 4);

            var buffer = new byte[bufferSize];
            lock (stream)
            {
                stream.Seek(streamOffset, SeekOrigin.Begin);
                var read = stream.Read(buffer, 0, bufferSize);
                if (read != bufferSize)
                {
                    mpqCompressionType = null;
                    return false;
                }
            }

            if (flags.HasFlag(MpqFileFlags.Encrypted) && bufferSize > 3)
            {
                if (encryptionSeed == 0)
                {
                    mpqCompressionType = null;
                    return false;
                }

                StormBuffer.DecryptBlock(buffer, encryptionSeed);
            }

            if (flags.HasFlag(MpqFileFlags.CompressedMulti) && bufferSize > 0)
            {
                mpqCompressionType = (MpqCompressionType)buffer[0];
                return true;
            }

            mpqCompressionType = null;
            return true;
        }

        private static bool TryPeekCompressionType(
            Stream stream,
            uint streamOffset,
            MpqFileFlags flags,
            uint encryptionSeed,
            uint[] blockPositions,
            int blockIndex,
            int expectedLength,
            out MpqCompressionType? mpqCompressionType)
        {
            var offset = blockPositions[blockIndex];
            var bufferSize = (int)(blockPositions[blockIndex + 1] - offset);
            var isEncrypted = flags.HasFlag(MpqFileFlags.Encrypted);

            if (bufferSize == expectedLength)
            {
                mpqCompressionType = null;
                return !isEncrypted || bufferSize < 4 || encryptionSeed != 0;
            }

            offset += streamOffset;
            bufferSize = Math.Min(bufferSize, 4);

            var buffer = new byte[bufferSize];
            lock (stream)
            {
                stream.Seek(offset, SeekOrigin.Begin);
                var read = stream.Read(buffer, 0, bufferSize);
                if (read != bufferSize)
                {
                    mpqCompressionType = null;
                    return false;
                }
            }

            if (isEncrypted && bufferSize > 3)
            {
                if (encryptionSeed == 0)
                {
                    mpqCompressionType = null;
                    return false;
                }

                StormBuffer.DecryptBlock(buffer, (uint)(blockIndex + encryptionSeed));
            }

            if (flags.HasFlag(MpqFileFlags.CompressedPK))
            {
                mpqCompressionType = null;
                return true;
            }

            mpqCompressionType = (MpqCompressionType)buffer[0];
            return true;
        }
    }
}