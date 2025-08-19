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
    /// <remarks>
    /// <para>
    /// An encoding entry is part of the CEKeyPageTable in the <see cref="EncodingFile"/>, mapping one
    /// <see cref="CascKey"/> (Content Key) to one or more <see cref="EKey"/>s (Encoding Keys). This structure
    /// supports multiple encoded representations of the same content, such as encrypted and unencrypted
    /// versions of a file processed by <see cref="Compression.BlteDecoder"/>.
    /// </para>
    /// <para>
    /// Entry format (from TACT specification):
    /// </para>
    /// <list type="bullet">
    /// <item><description>keyCount (1 byte): Number of <see cref="EKey"/>s for this content</description></item>
    /// <item><description>file_size (40 bits, big-endian): Size of the non-encoded version of the file</description></item>
    /// <item><description>ckey: The <see cref="CascKey"/> this entry represents</description></item>
    /// <item><description>ekeys: Array of <see cref="EKey"/>s (count = keyCount)</description></item>
    /// </list>
    /// <para>
    /// Multiple <see cref="EKey"/>s for a single <see cref="CascKey"/> allow the CASC system to support different
    /// encoding methods for the same content, providing flexibility in how data is stored and transmitted.
    /// The <see cref="Storage.OnlineCascStorage"/> uses these entries to resolve file lookups from
    /// <see cref="Root.TvfsRootHandler"/> to <see cref="Index.IndexFile"/> locations.
    /// </para>
    /// </remarks>
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
        /// Gets or sets the content key representing the MD5 hash of the uncompressed file.
        /// </summary>
        /// <value>The <see cref="CascKey"/> that uniquely identifies the content before any encoding is applied.</value>
        public CascKey CKey { get; set; }

        /// <summary>
        /// Gets or sets the content size in bytes of the uncompressed file.
        /// </summary>
        /// <value>The size of the original file before any <see cref="Compression.BlteDecoder"/> encoding is applied.</value>
        public uint ContentSize { get; set; }

        /// <summary>
        /// Gets the list of encoded keys representing different encoded versions of the content.
        /// </summary>
        /// <value>A list of <see cref="EKey"/>s that can be used to retrieve the content from <see cref="Index.IndexFile"/>s or CDN.</value>
        public List<EKey> EKeys { get; }

        /// <summary>
        /// Gets the number of encoded keys available for this content.
        /// </summary>
        /// <value>The count of <see cref="EKey"/>s in the <see cref="EKeys"/> list, representing different encoded versions.</value>
        public int EKeyCount => EKeys.Count;

        /// <summary>
        /// Gets the primary encoded key (first in the list) for this content.
        /// </summary>
        /// <value>The first <see cref="EKey"/> in the list, or <see langword="null"/> if no keys are available.</value>
        /// <remarks>
        /// The primary <see cref="EKey"/> is typically used by <see cref="Storage.OnlineCascStorage"/> for file retrieval
        /// when multiple encoded representations are available.
        /// </remarks>
        public EKey? PrimaryEKey => EKeys.Count > 0 ? EKeys[0] : null;

        /// <summary>
        /// Parses an encoding entry from a binary reader.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> positioned at the entry data.</param>
        /// <param name="header">The <see cref="EncodingHeader"/> containing format information for parsing.</param>
        /// <returns>A new <see cref="EncodingEntry"/> instance with parsed <see cref="CascKey"/> and <see cref="EKey"/> data.</returns>
        /// <exception cref="EndOfStreamException">Thrown when the reader reaches the end of the stream unexpectedly.</exception>
        /// <remarks>
        /// This method reads the binary format specified in the TACT protocol, including the key count,
        /// content size (40-bit big-endian), <see cref="CascKey"/>, and array of <see cref="EKey"/>s.
        /// </remarks>
        public static EncodingEntry Parse(BinaryReader reader, EncodingHeader header)
        {
            var entry = new EncodingEntry();

            // Read EKey count
            var ekeyCount = reader.ReadByte();

            // Read content size (40 bits, big-endian)
            var sizeBytes = reader.ReadBytes(5);
            ulong contentSize = 0;
            for (var i = 0; i < 5; i++)
            {
                contentSize = (contentSize << 8) | sizeBytes[i];
            }

            entry.ContentSize = (uint)contentSize;

            // Read content key
            entry.CKey = reader.ReadCKey();

            // Read encoded keys
            for (var i = 0; i < ekeyCount; i++)
            {
                var ekeyBytes = reader.ReadBytes(header.EKeyLength);
                entry.EKeys.Add(new EKey(ekeyBytes));
            }

            return entry;
        }

        /// <summary>
        /// Writes the encoding entry to a binary writer.
        /// </summary>
        /// <param name="writer">The <see cref="BinaryWriter"/> to write the entry data to.</param>
        /// <param name="header">The <see cref="EncodingHeader"/> containing format information for writing.</param>
        /// <remarks>
        /// This method writes the entry in the binary format specified by the TACT protocol,
        /// including the key count, content size (40-bit big-endian), <see cref="CascKey"/>, and <see cref="EKey"/> array.
        /// </remarks>
        public void WriteTo(BinaryWriter writer, EncodingHeader header)
        {
            // Write EKey count
            writer.Write((byte)EKeys.Count);

            // Write content size (40 bits, big-endian)
            ulong size = ContentSize;
            for (var i = 4; i >= 0; i--)
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