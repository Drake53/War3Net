// ------------------------------------------------------------------------------
// <copyright file="IndexHeaderV2.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.IO.Casc.Index
{
    /// <summary>
    /// Version 2 index file header.
    /// </summary>
    public class IndexHeaderV2
    {
        /// <summary>
        /// The expected version for v2 index files.
        /// </summary>
        public const ushort ExpectedVersion = 0x07;

        /// <summary>
        /// The size of the header in bytes.
        /// </summary>
        public const int HeaderSize = 0x10;

        /// <summary>
        /// Gets or sets the index version (must be 0x07).
        /// </summary>
        public ushort IndexVersion { get; set; }

        /// <summary>
        /// Gets or sets the bucket index.
        /// </summary>
        public byte BucketIndex { get; set; }

        /// <summary>
        /// Gets or sets the extra bytes (must be 0).
        /// </summary>
        public byte ExtraBytes { get; set; }

        /// <summary>
        /// Gets or sets the length of the EncodedSize field in bytes.
        /// </summary>
        public byte EncodedSizeLength { get; set; }

        /// <summary>
        /// Gets or sets the length of the StorageOffset field in bytes.
        /// </summary>
        public byte StorageOffsetLength { get; set; }

        /// <summary>
        /// Gets or sets the length of the encoded key in bytes.
        /// </summary>
        public byte EKeyLength { get; set; }

        /// <summary>
        /// Gets or sets the number of bits for the archive file offset.
        /// </summary>
        public byte FileOffsetBits { get; set; }

        /// <summary>
        /// Gets or sets the size of one data segment.
        /// </summary>
        public ulong SegmentSize { get; set; }

        /// <summary>
        /// Parses an index header v2 from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The parsed header.</returns>
        public static IndexHeaderV2 Parse(Stream stream)
        {
            using var reader = new BinaryReader(stream, System.Text.Encoding.UTF8, true);
            return Parse(reader);
        }

        /// <summary>
        /// Parses an index header v2 from a binary reader.
        /// </summary>
        /// <param name="reader">The reader to read from.</param>
        /// <returns>The parsed header.</returns>
        public static IndexHeaderV2 Parse(BinaryReader reader)
        {
            var header = new IndexHeaderV2
            {
                IndexVersion = reader.ReadUInt16(),
                BucketIndex = reader.ReadByte(),
                ExtraBytes = reader.ReadByte(),
                EncodedSizeLength = reader.ReadByte(),
                StorageOffsetLength = reader.ReadByte(),
                EKeyLength = reader.ReadByte(),
                FileOffsetBits = reader.ReadByte(),
                SegmentSize = reader.ReadUInt64(),
            };

            if (header.IndexVersion != ExpectedVersion)
            {
                throw new CascParserException($"Invalid index v2 version: 0x{header.IndexVersion:X4}, expected 0x{ExpectedVersion:X4}");
            }

            if (header.ExtraBytes != 0)
            {
                throw new CascParserException($"Invalid ExtraBytes value: {header.ExtraBytes}, expected 0");
            }

            return header;
        }

        /// <summary>
        /// Converts this v2 header to a normalized header.
        /// </summary>
        /// <returns>The normalized header.</returns>
        public IndexHeader ToNormalizedHeader()
        {
            return new IndexHeader
            {
                IndexVersion = IndexVersion,
                BucketIndex = BucketIndex,
                StorageOffsetLength = StorageOffsetLength,
                EncodedSizeLength = EncodedSizeLength,
                EKeyLength = EKeyLength,
                FileOffsetBits = FileOffsetBits,
                SegmentSize = SegmentSize,
                HeaderLength = HeaderSize,
                HeaderPadding = 0x08, // V2 has 8 bytes of padding after header
                EntryLength = EKeyLength + StorageOffsetLength + EncodedSizeLength,
                EKeyCount = 0, // V2 doesn't specify count in header
            };
        }
    }
}