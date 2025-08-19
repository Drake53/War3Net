// ------------------------------------------------------------------------------
// <copyright file="IndexHeaderV1.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.IO.Casc.Index
{
    /// <summary>
    /// Version 1 index file header.
    /// </summary>
    public class IndexHeaderV1
    {
        /// <summary>
        /// The expected version for v1 index files.
        /// </summary>
        public const ushort ExpectedVersion = 0x05;

        /// <summary>
        /// The size of the header in bytes.
        /// </summary>
        public const int HeaderSize = 0x20;

        /// <summary>
        /// Gets or sets the index version (must be 0x05).
        /// </summary>
        public ushort IndexVersion { get; set; }

        /// <summary>
        /// Gets or sets the bucket index.
        /// </summary>
        public byte BucketIndex { get; set; }

        /// <summary>
        /// Gets or sets the alignment byte.
        /// </summary>
        public byte Align3 { get; set; }

        /// <summary>
        /// Gets or sets field 4.
        /// </summary>
        public uint Field4 { get; set; }

        /// <summary>
        /// Gets or sets field 8.
        /// </summary>
        public ulong Field8 { get; set; }

        /// <summary>
        /// Gets or sets the size of one data segment.
        /// </summary>
        public ulong SegmentSize { get; set; }

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
        /// Gets or sets the first EKey count.
        /// </summary>
        public uint EKeyCount1 { get; set; }

        /// <summary>
        /// Gets or sets the second EKey count.
        /// </summary>
        public uint EKeyCount2 { get; set; }

        /// <summary>
        /// Gets or sets the first keys hash.
        /// </summary>
        public uint KeysHash1 { get; set; }

        /// <summary>
        /// Gets or sets the second keys hash.
        /// </summary>
        public uint KeysHash2 { get; set; }

        /// <summary>
        /// Gets or sets the header hash.
        /// </summary>
        public uint HeaderHash { get; set; }

        /// <summary>
        /// Parses an index header v1 from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The parsed header.</returns>
        public static IndexHeaderV1 Parse(Stream stream)
        {
            using var reader = new BinaryReader(stream, System.Text.Encoding.UTF8, true);
            return Parse(reader);
        }

        /// <summary>
        /// Parses an index header v1 from a binary reader.
        /// </summary>
        /// <param name="reader">The reader to read from.</param>
        /// <returns>The parsed header.</returns>
        public static IndexHeaderV1 Parse(BinaryReader reader)
        {
            var startPos = reader.BaseStream.Position;
            
            var header = new IndexHeaderV1
            {
                IndexVersion = reader.ReadUInt16(),
                BucketIndex = reader.ReadByte(),
                Align3 = reader.ReadByte(),
                Field4 = reader.ReadUInt32(),
                Field8 = reader.ReadUInt64(),
                SegmentSize = reader.ReadUInt64(),
                EncodedSizeLength = reader.ReadByte(),
                StorageOffsetLength = reader.ReadByte(),
                EKeyLength = reader.ReadByte(),
                FileOffsetBits = reader.ReadByte(),
                EKeyCount1 = reader.ReadUInt32(),
                EKeyCount2 = reader.ReadUInt32(),
                KeysHash1 = reader.ReadUInt32(),
                KeysHash2 = reader.ReadUInt32(),
                HeaderHash = reader.ReadUInt32(),
            };

            if (header.IndexVersion != ExpectedVersion)
            {
                throw new CascParserException($"Invalid index v1 version: 0x{header.IndexVersion:X4}, expected 0x{ExpectedVersion:X4}");
            }

            // Validate header hash if needed
            if (header.HeaderHash != 0)
            {
                // Calculate hash of header bytes (excluding the hash field itself)
                var currentPos = reader.BaseStream.Position;
                reader.BaseStream.Position = startPos;
                var headerBytes = reader.ReadBytes(HeaderSize - 4); // Read all but the hash field
                reader.BaseStream.Position = currentPos;
                
                // Use Jenkins hash or appropriate algorithm
                var calculatedHash = Utilities.HashHelper.ComputeJenkinsHash(headerBytes);
                if (calculatedHash != header.HeaderHash)
                {
                    System.Diagnostics.Trace.TraceWarning($"Index header hash mismatch: calculated 0x{calculatedHash:X8}, expected 0x{header.HeaderHash:X8}");
                }
            }

            return header;
        }

        /// <summary>
        /// Converts this v1 header to a normalized header.
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
                HeaderPadding = 0,
                EntryLength = EKeyLength + StorageOffsetLength + EncodedSizeLength,
                EKeyCount = (int)EKeyCount1,
            };
        }
    }
}