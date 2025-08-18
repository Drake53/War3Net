// ------------------------------------------------------------------------------
// <copyright file="EncodingHeader.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.IO.Casc.Utilities;

namespace War3Net.IO.Casc.Encoding
{
    /// <summary>
    /// Represents the header of an ENCODING manifest file.
    /// </summary>
    public class EncodingHeader
    {
        /// <summary>
        /// Gets or sets the magic signature (should be 'EN').
        /// </summary>
        public ushort Magic { get; set; }

        /// <summary>
        /// Gets or sets the version (expected to be 1).
        /// </summary>
        public byte Version { get; set; }

        /// <summary>
        /// Gets or sets the content key length in bytes.
        /// </summary>
        public byte CKeyLength { get; set; }

        /// <summary>
        /// Gets or sets the encoded key length in bytes.
        /// </summary>
        public byte EKeyLength { get; set; }

        /// <summary>
        /// Gets or sets the size of the CKey page in KB.
        /// </summary>
        public ushort CKeyPageSize { get; set; }

        /// <summary>
        /// Gets or sets the size of the EKey page in KB.
        /// </summary>
        public ushort EKeyPageSize { get; set; }

        /// <summary>
        /// Gets or sets the number of CKey pages.
        /// </summary>
        public uint CKeyPageCount { get; set; }

        /// <summary>
        /// Gets or sets the number of EKey pages.
        /// </summary>
        public uint EKeyPageCount { get; set; }

        /// <summary>
        /// Gets or sets field 11 (asserted to be zero).
        /// </summary>
        public byte Field11 { get; set; }

        /// <summary>
        /// Gets or sets the size of the ESpec string block.
        /// </summary>
        public uint ESpecBlockSize { get; set; }

        /// <summary>
        /// Gets the size of the CKey page in bytes.
        /// </summary>
        public int CKeyPageSizeBytes => CKeyPageSize * 1024;

        /// <summary>
        /// Gets the size of the EKey page in bytes.
        /// </summary>
        public int EKeyPageSizeBytes => EKeyPageSize * 1024;

        /// <summary>
        /// Parses an encoding header from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The parsed header.</returns>
        public static EncodingHeader Parse(Stream stream)
        {
            using var reader = new BinaryReader(stream, System.Text.Encoding.UTF8, true);
            return Parse(reader);
        }

        /// <summary>
        /// Parses an encoding header from a binary reader.
        /// </summary>
        /// <param name="reader">The reader to read from.</param>
        /// <returns>The parsed header.</returns>
        public static EncodingHeader Parse(BinaryReader reader)
        {
            var header = new EncodingHeader
            {
                Magic = reader.ReadUInt16(),
                Version = reader.ReadByte(),
                CKeyLength = reader.ReadByte(),
                EKeyLength = reader.ReadByte(),
                CKeyPageSize = reader.ReadUInt16BE(),
                EKeyPageSize = reader.ReadUInt16BE(),
                CKeyPageCount = reader.ReadUInt32BE(),
                EKeyPageCount = reader.ReadUInt32BE(),
                Field11 = reader.ReadByte(),
                ESpecBlockSize = reader.ReadUInt32BE(),
            };

            // Validate magic
            if (header.Magic != CascConstants.FileMagicEncoding)
            {
                throw new CascParserException($"Invalid ENCODING magic: 0x{header.Magic:X4}, expected 0x{CascConstants.FileMagicEncoding:X4}");
            }

            // Validate version
            if (header.Version != 1)
            {
                throw new CascParserException($"Unsupported ENCODING version: {header.Version}");
            }

            return header;
        }

        /// <summary>
        /// Writes the header to a binary writer.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        public void WriteTo(BinaryWriter writer)
        {
            writer.Write(Magic);
            writer.Write(Version);
            writer.Write(CKeyLength);
            writer.Write(EKeyLength);
            writer.WriteUInt16BE(CKeyPageSize);
            writer.WriteUInt16BE(EKeyPageSize);
            writer.WriteUInt32BE(CKeyPageCount);
            writer.WriteUInt32BE(EKeyPageCount);
            writer.Write(Field11);
            writer.WriteUInt32BE(ESpecBlockSize);
        }
    }
}