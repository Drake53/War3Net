// ------------------------------------------------------------------------------
// <copyright file="EncodingEntry.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using War3Net.IO.Casc.Structures;
using War3Net.IO.Casc.Utilities;

namespace War3Net.IO.Casc.Encoding
{
    /// <summary>
    /// Represents a single entry in the ENCODING manifest.
    /// </summary>
    public class EncodingEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncodingEntry"/> class.
        /// </summary>
        public EncodingEntry()
        {
            EKeys = new List<EKey>();
        }

        /// <summary>
        /// Gets or sets the content key.
        /// </summary>
        public CascKey CKey { get; set; }

        /// <summary>
        /// Gets or sets the content size.
        /// </summary>
        public uint ContentSize { get; set; }

        /// <summary>
        /// Gets the list of encoded keys for this content.
        /// </summary>
        public List<EKey> EKeys { get; }

        /// <summary>
        /// Gets the number of encoded keys.
        /// </summary>
        public int EKeyCount => EKeys.Count;

        /// <summary>
        /// Gets the primary encoded key (first in the list).
        /// </summary>
        public EKey? PrimaryEKey => EKeys.Count > 0 ? EKeys[0] : null;

        /// <summary>
        /// Parses an encoding entry from a binary reader.
        /// </summary>
        /// <param name="reader">The reader to read from.</param>
        /// <param name="header">The encoding header.</param>
        /// <returns>The parsed entry.</returns>
        public static EncodingEntry Parse(BinaryReader reader, EncodingHeader header)
        {
            var entry = new EncodingEntry();

            // Read EKey count
            var ekeyCount = reader.ReadUInt16BE();

            // Read content size (40 bits, big-endian)
            var sizeBytes = reader.ReadBytes(5);
            ulong contentSize = 0;
            for (int i = 0; i < 5; i++)
            {
                contentSize = (contentSize << 8) | sizeBytes[i];
            }

            entry.ContentSize = (uint)contentSize;

            // Read content key
            entry.CKey = reader.ReadCKey();

            // Read encoded keys
            for (int i = 0; i < ekeyCount; i++)
            {
                var ekeyBytes = reader.ReadBytes(header.EKeyLength);
                entry.EKeys.Add(new EKey(ekeyBytes));
            }

            return entry;
        }

        /// <summary>
        /// Writes the entry to a binary writer.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="header">The encoding header.</param>
        public void WriteTo(BinaryWriter writer, EncodingHeader header)
        {
            // Write EKey count
            writer.WriteUInt16BE((ushort)EKeys.Count);

            // Write content size (40 bits, big-endian)
            ulong size = ContentSize;
            for (int i = 4; i >= 0; i--)
            {
                writer.Write((byte)((size >> (i * 8)) & 0xFF));
            }

            // Write content key
            writer.WriteCKey(CKey);

            // Write encoded keys
            foreach (var eKey in EKeys)
            {
                writer.WriteEKey(eKey, header.EKeyLength);
            }
        }
    }
}