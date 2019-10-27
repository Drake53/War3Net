// ------------------------------------------------------------------------------
// <copyright file="MpqEntry.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

#if NETCOREAPP3_0
using System.Diagnostics.CodeAnalysis;
#endif

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

        private uint _encryptionSeed;

        private string? _filename;
        private uint? _headerOffset;
        private uint? _fileOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqEntry"/> class.
        /// </summary>
        /// <param name="br">The reader from which to read the entry.</param>
        /// <param name="headerOffset">The containing <see cref="MpqArchive"/>'s header offset.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="br"/> is null.</exception>
        public MpqEntry(BinaryReader br, uint headerOffset)
            : this(headerOffset, br?.ReadUInt32() ?? throw new ArgumentNullException(nameof(br)), br.ReadUInt32(), br.ReadUInt32(), (MpqFileFlags)br.ReadUInt32())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqEntry"/> class.
        /// </summary>
        /// <param name="headerOffset">The containing <see cref="MpqArchive"/>'s header offset.</param>
        /// <param name="fileOffset">The file's position in the archive, relative to the header offset.</param>
        /// <param name="compressedSize">The compressed size of the file.</param>
        /// <param name="fileSize">The uncompressed size of the file.</param>
        /// <param name="flags">The file's <see cref="MpqFileFlags"/>.</param>
        internal MpqEntry(uint headerOffset, uint fileOffset, uint compressedSize, uint fileSize, MpqFileFlags flags)
        {
            _headerOffset = headerOffset;
            _fileOffset = fileOffset;
            _compressedSize = compressedSize;
            _fileSize = fileSize;
            _flags = flags;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqEntry"/> class.
        /// </summary>
        /// <param name="filename">The filename of the file.</param>
        /// <param name="compressedSize">The compressed size of the file.</param>
        /// <param name="fileSize">The uncompressed size of the file.</param>
        /// <param name="flags">The file's <see cref="MpqFileFlags"/>.</param>
        internal MpqEntry(string? filename, uint compressedSize, uint fileSize, MpqFileFlags flags)
        {
            _filename = filename;
            _headerOffset = null;
            _fileOffset = null;
            _compressedSize = compressedSize;
            _fileSize = fileSize;
            _flags = flags;
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
        /// Gets the filename of the file in the archive.
        /// </summary>
#if NETCOREAPP3_0
        [DisallowNull]
#endif
        public string? Filename
        {
            get => _filename;
            internal set
            {
                _filename = value;
                UpdateEncryptionSeed();
            }
        }

        /// <summary>
        /// Gets the containing <see cref="MpqArchive"/>'s header offset.
        /// </summary>
        public uint? HeaderOffset => _headerOffset;

        /// <summary>
        /// Gets the relative (to the <see cref="MpqHeader"/>) position of the file in the archive.
        /// </summary>
        public uint? FileOffset => _fileOffset;

        /// <summary>
        /// Gets the absolute position of this <see cref="MpqEntry"/>'s file in the base stream of the containing <see cref="MpqArchive"/>.
        /// </summary>
        public uint? FilePosition => _headerOffset + _fileOffset;

        /// <summary>
        /// Gets a value indicating whether this <see cref="MpqEntry"/> has the flag <see cref="MpqFileFlags.Compressed"/>.
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

        /// <summary>
        /// Sets the file's offsets, which are required to get the absolute position of the file.
        /// </summary>
        /// <param name="headerOffset">The containing <see cref="MpqArchive"/>'s header offset.</param>
        /// <param name="fileOffset">The file's relative offset to the <see cref="MpqHeader"/>.</param>
        /// <exception cref="InvalidOperationException">Thrown when the <see cref="IsAdded"/> property is true.</exception>
        public void SetPos(uint headerOffset, uint fileOffset)
        {
            if (_fileOffset != null)
            {
                throw new InvalidOperationException("Cannot change the FilePos for an MpqEntry after it's been set.");
            }

            _headerOffset = headerOffset;
            _fileOffset = fileOffset;

            if (_encryptionSeed == 0)
            {
                UpdateEncryptionSeed();
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Filename ?? (_flags == 0 ? "(Deleted file)" : $"Unknown file @ {FilePosition}");
        }

        /// <summary>
        /// Writes the entry to a <see cref="BlockTable"/>.
        /// </summary>
        /// <param name="writer">The writer to which the entry is written.</param>
        internal void WriteEntry(BinaryWriter writer)
        {
            if (_fileOffset is null)
            {
                throw new Exception();
            }

            writer.Write((uint)_fileOffset);
            writer.Write(_compressedSize);
            writer.Write(_fileSize);
            writer.Write((uint)_flags);
        }

        /// <summary>
        /// Try to determine the entry's encryption seed when the filename is not known.
        /// </summary>
        /// <param name="blockPos0">The first block's offset in the <see cref="MpqStream"/>.</param>
        /// <param name="blockPos1">The second block's offset in the <see cref="MpqStream"/>.</param>
        /// <param name="blockPosSize">The size (in bytes) for all the block position offsets in the stream.</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        internal bool TryUpdateEncryptionSeed(uint blockPos0, uint blockPos1, uint blockPosSize)
        {
            var result = StormBuffer.DetectFileSeed(blockPos0, blockPos1, blockPosSize);
            if (result == 0)
            {
                return false;
            }

            _encryptionSeed = result + 1;
            return true;
        }

        private static uint CalculateEncryptionSeed(string? filename, uint? fileOffset, uint fileSize, MpqFileFlags flags)
        {
            if (filename is null)
            {
                return 0;
            }

            var blockOffsetAdjusted = flags.HasFlag(MpqFileFlags.BlockOffsetAdjustedKey);
            if (fileOffset is null)
            {
                return blockOffsetAdjusted ? 0 : StormBuffer.HashString(Path.GetFileName(filename), 0x300);
            }

            var seed = StormBuffer.HashString(Path.GetFileName(filename), 0x300);
            if (blockOffsetAdjusted)
            {
                seed = (seed + (uint)fileOffset) ^ fileSize;
            }

            return seed;
        }

        private void UpdateEncryptionSeed()
        {
            _encryptionSeed = CalculateEncryptionSeed(_filename, _fileOffset, _fileSize, _flags);
        }
    }
}