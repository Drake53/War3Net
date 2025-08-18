// ------------------------------------------------------------------------------
// <copyright file="EKeyEntry.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

using War3Net.IO.Casc.Structures;

namespace War3Net.IO.Casc.Index
{
    /// <summary>
    /// Represents an encoded key entry in an index file.
    /// </summary>
    public class EKeyEntry
    {
        /// <summary>
        /// Gets or sets the encoded key.
        /// </summary>
        public EKey EKey { get; set; }

        /// <summary>
        /// Gets or sets the data file index.
        /// </summary>
        public uint DataFileIndex { get; set; }

        /// <summary>
        /// Gets or sets the offset within the data file.
        /// </summary>
        public ulong DataFileOffset { get; set; }

        /// <summary>
        /// Gets or sets the encoded size of the file.
        /// </summary>
        public uint EncodedSize { get; set; }

        /// <summary>
        /// Parses an EKey entry from a binary reader.
        /// </summary>
        /// <param name="reader">The reader to read from.</param>
        /// <param name="header">The index header.</param>
        /// <returns>The parsed entry.</returns>
        public static EKeyEntry Parse(BinaryReader reader, IndexHeader header)
        {
            var entry = new EKeyEntry();

            // Read EKey
            var ekeyBytes = reader.ReadBytes(header.EKeyLength);
            entry.EKey = new EKey(ekeyBytes);

            // Read storage offset (big-endian)
            var storageOffsetBytes = reader.ReadBytes(header.StorageOffsetLength);
            var storageOffset = ReadBigEndianValue(storageOffsetBytes);

            // Extract data file index and offset
            var fileOffsetMask = (1UL << header.FileOffsetBits) - 1;
            entry.DataFileOffset = storageOffset & fileOffsetMask;
            entry.DataFileIndex = (uint)(storageOffset >> header.FileOffsetBits);

            // Read encoded size (big-endian)
            if (header.EncodedSizeLength > 0)
            {
                var encodedSizeBytes = reader.ReadBytes(header.EncodedSizeLength);
                entry.EncodedSize = (uint)ReadBigEndianValue(encodedSizeBytes);
            }

            return entry;
        }

        /// <summary>
        /// Writes the entry to a binary writer.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="header">The index header.</param>
        public void WriteTo(BinaryWriter writer, IndexHeader header)
        {
            // Write EKey
            var ekeyBytes = EKey.ToArray();
            if (ekeyBytes.Length < header.EKeyLength)
            {
                Array.Resize(ref ekeyBytes, header.EKeyLength);
            }

            writer.Write(ekeyBytes, 0, header.EKeyLength);

            // Combine data file index and offset
            var storageOffset = ((ulong)DataFileIndex << header.FileOffsetBits) | DataFileOffset;

            // Write storage offset (big-endian)
            WriteBigEndianValue(writer, storageOffset, header.StorageOffsetLength);

            // Write encoded size (big-endian)
            if (header.EncodedSizeLength > 0)
            {
                WriteBigEndianValue(writer, EncodedSize, header.EncodedSizeLength);
            }
        }

        private static ulong ReadBigEndianValue(byte[] bytes)
        {
            ulong value = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                value = (value << 8) | bytes[i];
            }

            return value;
        }

        private static void WriteBigEndianValue(BinaryWriter writer, ulong value, int byteCount)
        {
            var bytes = new byte[byteCount];
            for (int i = byteCount - 1; i >= 0; i--)
            {
                bytes[i] = (byte)(value & 0xFF);
                value >>= 8;
            }

            writer.Write(bytes);
        }
    }
}