// ------------------------------------------------------------------------------
// <copyright file="IndexHeader.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.IO.Casc.Index
{
    /// <summary>
    /// Normalized header for CASC index files.
    /// </summary>
    public class IndexHeader
    {
        /// <summary>
        /// Gets or sets the index version (5 for v1.0, 7 for v2.0).
        /// </summary>
        public ushort IndexVersion { get; set; }

        /// <summary>
        /// Gets or sets the bucket index.
        /// </summary>
        public byte BucketIndex { get; set; }

        /// <summary>
        /// Gets or sets the length of the StorageOffset field in bytes.
        /// </summary>
        public byte StorageOffsetLength { get; set; }

        /// <summary>
        /// Gets or sets the length of the EncodedSize field in bytes.
        /// </summary>
        public byte EncodedSizeLength { get; set; }

        /// <summary>
        /// Gets or sets the length of the EKey field in bytes.
        /// </summary>
        public byte EKeyLength { get; set; }

        /// <summary>
        /// Gets or sets the number of bits for the archive file offset in StorageOffset field.
        /// </summary>
        public byte FileOffsetBits { get; set; }

        /// <summary>
        /// Gets or sets the size of one data segment (data.### file).
        /// </summary>
        public ulong SegmentSize { get; set; }

        /// <summary>
        /// Gets or sets the length of the header structure in bytes.
        /// </summary>
        public int HeaderLength { get; set; }

        /// <summary>
        /// Gets or sets the length of padding after the header.
        /// </summary>
        public int HeaderPadding { get; set; }

        /// <summary>
        /// Gets or sets the length of each EKey entry in bytes.
        /// </summary>
        public int EntryLength { get; set; }

        /// <summary>
        /// Gets or sets the number of EKey entries (v1 only).
        /// </summary>
        public int EKeyCount { get; set; }

        /// <summary>
        /// Gets a value indicating whether this is a version 1 index.
        /// </summary>
        public bool IsVersion1 => IndexVersion == 5;

        /// <summary>
        /// Gets a value indicating whether this is a version 2 index.
        /// </summary>
        public bool IsVersion2 => IndexVersion == 7;

        /// <summary>
        /// Calculates the entry length based on the header fields.
        /// </summary>
        public void CalculateEntryLength()
        {
            EntryLength = EKeyLength + StorageOffsetLength + EncodedSizeLength;
        }
    }
}