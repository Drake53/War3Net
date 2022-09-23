// ------------------------------------------------------------------------------
// <copyright file="MpqHash.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;

using War3Net.Common.Providers;

namespace War3Net.IO.Mpq
{
    /// <summary>
    /// An entry in a <see cref="HashTable"/>.
    /// </summary>
    public struct MpqHash
    {
        /// <summary>
        /// The length (in bytes) of an <see cref="MpqHash"/>.
        /// </summary>
        public const uint Size = 16;

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqHash"/> struct.
        /// </summary>
        public MpqHash(ulong name, MpqLocale locale, uint blockIndex, uint mask)
            : this(name, locale, blockIndex)
        {
            Mask = mask;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqHash"/> struct.
        /// </summary>
        [Obsolete]
        public MpqHash(uint name1, uint name2, MpqLocale locale, uint blockIndex, uint mask)
            : this(CombineNames(name1, name2), locale, blockIndex)
        {
            Mask = mask;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqHash"/> struct.
        /// </summary>
        public MpqHash(BinaryReader reader, uint mask)
            : this(reader.ReadUInt64(), (MpqLocale)reader.ReadUInt32(), reader.ReadUInt32())
        {
            Mask = mask;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqHash"/> struct.
        /// </summary>
        public MpqHash(string fileName, uint mask, MpqLocale locale, uint blockIndex)
            : this(GetHashedFileName(fileName), locale, blockIndex, mask)
        {
        }

        private MpqHash(ulong name, MpqLocale locale, uint blockIndex)
            : this()
        {
            Name = name;
            Locale = locale;
            BlockIndex = blockIndex;
        }

        public static MpqHash DELETED => new MpqHash(ulong.MaxValue, (MpqLocale)0xFFFFFFFF, 0xFFFFFFFE);

        public static MpqHash NULL => new MpqHash(ulong.MaxValue, (MpqLocale)0xFFFFFFFF, 0xFFFFFFFF); // todo: rename EMPTY?

        public ulong Name { get; private set; }

        public MpqLocale Locale { get; private set; }

        public uint BlockIndex { get; private set; }

        public uint Mask { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="MpqHash"/> corresponds to an <see cref="MpqEntry"/>.
        /// </summary>
        public bool IsEmpty => BlockIndex == 0xFFFFFFFF;

        /// <summary>
        /// Gets a value indicating whether the <see cref="MpqHash"/> has had its corresponding <see cref="MpqEntry"/> deleted.
        /// </summary>
        public bool IsDeleted => BlockIndex == 0xFFFFFFFE;

        /// <summary>
        /// Gets a value indicating whether this <see cref="MpqHash"/> can be overwritten by another hash in the <see cref="HashTable"/>.
        /// </summary>
        public bool IsAvailable => BlockIndex >= 0xFFFFFFFE;

        // The hash table's size cannot be larger than 2^16 (mpq v0) or 2^20 (mpq v1).
        // Assuming the block table's size is also at most that large, a block index of (2^24 - 1) or higher should be invalid.
        internal bool IsValidBlockIndex => BlockIndex < 0x00FFFFFF;

        public static uint GetIndex(string path)
        {
            if (path.Any(c => c >= 0x200))
            {
                throw new ArgumentException($"One or more of the characters in the input string have a numerical value of 0x200 or larger.", nameof(path));
            }

            return StormBuffer.HashString(path, 0);
        }

        public static uint GetIndex(string path, uint mask)
        {
            return GetIndex(path) & mask;
        }

        public static ulong GetHashedFileName(string fileName)
        {
            if (fileName.Any(c => c >= 0x200))
            {
                throw new ArgumentException($"One or more of the characters in the input string have a numerical value of 0x200 or larger.", nameof(fileName));
            }

            return CombineNames(StormBuffer.HashString(fileName, 0x100), StormBuffer.HashString(fileName, 0x200));
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return IsEmpty ? "EMPTY" : IsDeleted ? "DELETED" : $"Entry #{BlockIndex}";
        }

        public void SerializeTo(Stream stream)
        {
            using (var writer = new BinaryWriter(stream, UTF8EncodingProvider.StrictUTF8, true))
            {
                WriteTo(writer);
            }
        }

        public void WriteTo(BinaryWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.Write(Name);
            writer.Write((uint)Locale);
            writer.Write(BlockIndex);
        }

        [Obsolete]
        private static ulong CombineNames(uint name1, uint name2)
        {
            return name1 | ((ulong)name2 << 32);
        }
    }
}