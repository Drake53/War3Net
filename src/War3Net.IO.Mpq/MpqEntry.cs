// ------------------------------------------------------------------------------
// <copyright file="MpqEntry.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;

using System.Diagnostics.CodeAnalysis;

namespace War3Net.IO.Mpq
{
    /// <summary>
    /// An entry in a <see cref="BlockTable"/>, which corresponds to a single file in the <see cref="MpqArchive"/>.
    /// </summary>
    public class MpqEntry
    {
        /// <summary>
        /// The length (in bytes) of an <see cref="MpqEntry"/>.
        /// </summary>
        public const uint Size = 16;

        private readonly uint _compressedSize;
        private readonly uint _fileSize;
        private readonly MpqFileFlags _flags;
        private readonly uint _headerOffset;
        private readonly uint _fileOffset;

        private string? _fileName;

        private uint _encryptionSeed;
        private uint _baseEncryptionSeed;

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqEntry"/> class.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="headerOffset">The containing <see cref="MpqArchive"/>'s header offset.</param>
        /// <param name="fileOffset">The file's position in the archive, relative to the header offset.</param>
        /// <param name="compressedSize">The compressed size of the file.</param>
        /// <param name="fileSize">The uncompressed size of the file.</param>
        /// <param name="flags">The file's <see cref="MpqFileFlags"/>.</param>
        internal MpqEntry(string? fileName, uint headerOffset, uint fileOffset, uint compressedSize, uint fileSize, MpqFileFlags flags)
        {
            _headerOffset = headerOffset;
            _fileOffset = fileOffset;
            _fileName = fileName;
            _compressedSize = compressedSize;
            _fileSize = fileSize;
            _flags = flags;

            if (fileName != null)
            {
                UpdateEncryptionSeed();
            }
        }

        /// <summary>
        /// Gets the compressed file size of this <see cref="MpqEntry"/>.
        /// </summary>
        public uint CompressedSize => _compressedSize;

        /// <summary>
        /// Gets the uncompressed file size of this <see cref="MpqEntry"/>.
        /// </summary>
        public uint FileSize => _fileSize;

        /// <summary>
        /// Gets the file's flags.
        /// </summary>
        public MpqFileFlags Flags => _flags;

        /// <summary>
        /// Gets the encryption seed that is used if the file is encrypted.
        /// </summary>
        public uint EncryptionSeed => _encryptionSeed;

        /// <summary>
        /// Gets the encryption seed for this entry's filename.
        /// </summary>
        /// <remarks>
        /// The base encryption seed is not adjusted for <see cref="MpqFileFlags.BlockOffsetAdjustedKey"/>.
        /// </remarks>
        public uint BaseEncryptionSeed => _baseEncryptionSeed;

        /// <summary>
        /// Gets the filename of the file in the archive.
        /// </summary>
        [DisallowNull]
        public string? FileName
        {
            get => _fileName;
            internal set
            {
                _fileName = value;
                UpdateEncryptionSeed();
            }
        }

        /// <summary>
        /// Gets the containing <see cref="MpqArchive"/>'s header offset.
        /// </summary>
        public uint HeaderOffset => _headerOffset;

        /// <summary>
        /// Gets the relative (to the <see cref="MpqHeader"/>) position of the file in the archive.
        /// </summary>
        public uint FileOffset => _fileOffset;

        /// <summary>
        /// Gets the absolute position of this <see cref="MpqEntry"/>'s file in the base stream of the containing <see cref="MpqArchive"/>.
        /// </summary>
        public uint FilePosition => _headerOffset + _fileOffset;

        /// <summary>
        /// Gets a value indicating whether this <see cref="MpqEntry"/> has any of the <see cref="MpqFileFlags.Compressed"/> flags.
        /// </summary>
        public bool IsCompressed => (_flags & MpqFileFlags.Compressed) != 0;

        /// <summary>
        /// Gets a value indicating whether this <see cref="MpqEntry"/> has the flag <see cref="MpqFileFlags.Encrypted"/>.
        /// </summary>
        public bool IsEncrypted => _flags.HasFlag(MpqFileFlags.Encrypted);

        /// <summary>
        /// Gets a value indicating whether this <see cref="MpqEntry"/> has the flag <see cref="MpqFileFlags.SingleUnit"/>.
        /// </summary>
        public bool IsSingleUnit => _flags.HasFlag(MpqFileFlags.SingleUnit);

        public static MpqEntry Parse(Stream stream, uint headerOffset)
        {
            using var reader = new BinaryReader(stream);
            return FromReader(reader, headerOffset);
        }

        public static MpqEntry FromReader(BinaryReader reader, uint headerOffset)
        {
            _ = reader ?? throw new ArgumentNullException(nameof(reader));
            return new MpqEntry(null, headerOffset, reader.ReadUInt32(), reader.ReadUInt32(), reader.ReadUInt32(), (MpqFileFlags)reader.ReadUInt32());
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return FileName ?? (_flags == 0 ? "(Deleted file)" : $"Unknown file @ {FilePosition}");
        }

        public void SerializeTo(Stream stream)
        {
            using (var writer = new BinaryWriter(stream, new System.Text.UTF8Encoding(false, true), true))
            {
                WriteTo(writer);
            }
        }

        /// <summary>
        /// Writes the entry to a <see cref="BlockTable"/>.
        /// </summary>
        /// <param name="writer">The writer to which the entry is written.</param>
        public void WriteTo(BinaryWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.Write(_fileOffset);
            writer.Write(_compressedSize);
            writer.Write(_fileSize);
            writer.Write((uint)_flags);
        }

        internal static uint AdjustEncryptionSeed(uint baseSeed, uint fileOffset, uint fileSize)
        {
            return (baseSeed + fileOffset) ^ fileSize;
        }

        internal static uint UnadjustEncryptionSeed(uint adjustedSeed, uint fileOffset, uint fileSize)
        {
            return (adjustedSeed ^ fileSize) - fileOffset;
        }

        internal static uint CalculateEncryptionSeed(string? fileName)
        {
            return CalculateEncryptionSeed(fileName, out var encryptionSeed) ? encryptionSeed : 0;
        }

        internal static bool CalculateEncryptionSeed(string? fileName, out uint encryptionSeed)
        {
            if (fileName != null && StormBuffer.TryGetHashString(Path.GetFileName(fileName), 0x300, out encryptionSeed))
            {
                return true;
            }

            encryptionSeed = 0;
            return false;
        }

        internal static uint CalculateEncryptionSeed(string? fileName, uint fileOffset, uint fileSize, MpqFileFlags flags)
        {
            if (fileName is null)
            {
                return 0;
            }

            var blockOffsetAdjusted = flags.HasFlag(MpqFileFlags.BlockOffsetAdjustedKey);
            var seed = CalculateEncryptionSeed(fileName);
            if (blockOffsetAdjusted)
            {
                seed = AdjustEncryptionSeed(seed, (uint)fileOffset, fileSize);
            }

            return seed;
        }

        /// <summary>
        /// Try to determine the entry's encryption seed when the filename is not known.
        /// </summary>
        /// <param name="blockPos0">The encrypted value for the first block's offset in the <see cref="MpqStream"/>.</param>
        /// <param name="blockPos1">The encrypted value for the second block's offset in the <see cref="MpqStream"/>.</param>
        /// <param name="blockPosSize">The size (in bytes) for all the block position offsets in the stream.</param>
        /// <param name="max">The highest possible value that <paramref name="blockPos1"/> can have when decrypted.</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        internal bool TryUpdateEncryptionSeed(uint blockPos0, uint blockPos1, uint blockPosSize, uint max)
        {
            if (!StormBuffer.DetectFileSeed(blockPos0, blockPos1, blockPosSize, out var result))
            {
                var possibleSeeds = StormBuffer.DetectFileSeeds(blockPos0, blockPosSize)
                    .Where(seed =>
                    {
                        var data = new[] { blockPos0, blockPos1 };
                        StormBuffer.DecryptBlock(data, seed);
                        return data[1] <= max;
                    })
                    .ToList();

                if (possibleSeeds.Count != 1)
                {
                    return false;
                }

                result = possibleSeeds.Single();
            }

            _encryptionSeed = result + 1;
            _baseEncryptionSeed = _flags.HasFlag(MpqFileFlags.BlockOffsetAdjustedKey)
                ? UnadjustEncryptionSeed(_encryptionSeed, _fileOffset, _fileSize)
                : _encryptionSeed;

            return true;
        }

        private void UpdateEncryptionSeed()
        {
            _encryptionSeed = CalculateEncryptionSeed(_fileName, _fileOffset, _fileSize, _flags);
            _baseEncryptionSeed = CalculateEncryptionSeed(_fileName);
        }
    }
}