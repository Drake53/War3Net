// ------------------------------------------------------------------------------
// <copyright file="MpqHeader.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

namespace War3Net.IO.Mpq
{
    /// <summary>
    /// The header of an <see cref="MpqArchive"/>.
    /// </summary>
    public class MpqHeader
    {
        /// <summary>
        /// The expected signature of an MPQ file.
        /// </summary>
        public const uint MpqId = 0x1a51504d;

        /// <summary>
        /// The length (in bytes) of an <see cref="MpqHeader"/>.
        /// </summary>
        public const uint Size = 32;

        // A protected archive. Seen in some custom wc3 maps.
        private const uint ProtectedOffset = 0x6d9e4b86;

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqHeader"/> class.
        /// </summary>
        /// <param name="fileArchiveSize">The length (in bytes) of the file archive.</param>
        /// <param name="hashTableEntries">The amount of <see cref="MpqHash"/> objects in the <see cref="HashTable"/>.</param>
        /// <param name="blockTableEntries">The amount of <see cref="MpqEntry"/> objects in the <see cref="BlockTable"/>.</param>
        /// <param name="blockSize">The blocksize that the corresponding <see cref="MpqArchive"/> has.</param>
        /// <param name="archiveBeforeTables">If true, the archive and table offsets are set so that the archive directly follows the header.</param>
        public MpqHeader(uint fileArchiveSize, uint hashTableEntries, uint blockTableEntries, ushort blockSize, bool archiveBeforeTables = true)
            : this()
        {
            var hashTableSize = hashTableEntries * MpqHash.Size;
            var blockTableSize = blockTableEntries * MpqEntry.Size;

            if (archiveBeforeTables)
            {
                // MPQ contents are in order: header, archive, HT, BT
                DataOffset = Size;
                ArchiveSize = Size + fileArchiveSize + hashTableSize + blockTableSize;
                MpqVersion = 0;
                BlockSize = blockSize;
                HashTableOffset = Size + fileArchiveSize;
                BlockTableOffset = Size + fileArchiveSize + hashTableSize;
                HashTableSize = hashTableEntries;
                BlockTableSize = blockTableEntries;
            }
            else
            {
                // MPQ contents are in order: header, HT, BT, archive
                DataOffset = Size + hashTableSize + blockTableSize;
                ArchiveSize = Size + fileArchiveSize + hashTableSize + blockTableSize;
                MpqVersion = 0;
                BlockSize = blockSize;
                HashTableOffset = Size;
                BlockTableOffset = Size + hashTableSize;
                HashTableSize = hashTableEntries;
                BlockTableSize = blockTableEntries;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqHeader"/> class.
        /// </summary>
        private MpqHeader()
        {
            ID = MpqId;
        }

        /// <summary>
        /// Gets the signature of the MPQ file. Should be <see cref="MpqId"/>.
        /// </summary>
        public uint ID { get; private set; }

        /// <summary>
        /// Gets the offset of the files in the archive, relative to the <see cref="MpqHeader"/>.
        /// </summary>
        public uint DataOffset { get; private set; }

        /// <summary>
        /// Gets the size of the entire <see cref="MpqArchive"/>. This includes the header, archive files, hashtable, and blocktable sizes.
        /// </summary>
        public uint ArchiveSize { get; internal set; }

        /// <summary>
        /// Gets the format version of the .mpq file. Currently, only version 0 is supported.
        /// </summary>
        public ushort MpqVersion { get; private set; } // Most are 0.  Burning Crusade = 1

        /// <summary>
        /// Gets the <see cref="MpqArchive"/>'s block size.
        /// </summary>
        public ushort BlockSize { get; private set; } // Size of file block is 0x200 << BlockSize

        /// <summary>
        /// Gets the offset of the <see cref="HashTable"/>, relative to the <see cref="MpqHeader"/>.
        /// </summary>
        public uint HashTableOffset { get; internal set; }

        /// <summary>
        /// Gets the offset of the <see cref="BlockTable"/>, relative to the <see cref="MpqHeader"/>.
        /// </summary>
        public uint BlockTableOffset { get; internal set; }

        /// <summary>
        /// Gets the amount of <see cref="MpqHash"/> entries in the <see cref="HashTable"/>.
        /// </summary>
        public uint HashTableSize { get; private set; }

        /// <summary>
        /// Gets the amount of <see cref="MpqEntry"/> entries in the <see cref="BlockTable"/>.
        /// </summary>
        public uint BlockTableSize { get; private set; }

        // Version 1 fields
        // The extended block table is an array of Int16 - higher bits of the offests in the block table.
        public long ExtendedBlockTableOffset { get; private set; }

        public short HashTableOffsetHigh { get; private set; }

        public short BlockTableOffsetHigh { get; private set; }

        /// <summary>
        /// Gets the offset of this <see cref="MpqHeader"/>, relative to the start of the base stream.
        /// </summary>
        public uint HeaderOffset { get; internal set; }

        /// <summary>
        /// Gets the absolute offset of the files in the <see cref="MpqArchive"/>'s base stream.
        /// </summary>
        public uint DataPosition => DataOffset == ProtectedOffset ? Size : DataOffset + HeaderOffset;

        /// <summary>
        /// Gets the absolute offset of the <see cref="HashTable"/> in the <see cref="MpqArchive"/>'s base stream.
        /// </summary>
        public uint HashTablePosition => HashTableOffset + HeaderOffset;

        /// <summary>
        /// Gets the absolute offset of the <see cref="BlockTable"/> in the <see cref="MpqArchive"/>'s base stream.
        /// </summary>
        public uint BlockTablePosition => BlockTableOffset + HeaderOffset;

        /// <summary>
        /// Reads from the given reader to create a new MPQ header.
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        public static MpqHeader FromReader(BinaryReader br)
        {
            var id = br?.ReadUInt32() ?? throw new ArgumentNullException(nameof(br));
            if (id != MpqId)
            {
                throw new MpqParserException("");
            }

            var header = new MpqHeader
            {
                ID = id,
                DataOffset = br.ReadUInt32(),
                ArchiveSize = br.ReadUInt32(),
                MpqVersion = br.ReadUInt16(),
                BlockSize = br.ReadUInt16(),
                HashTableOffset = br.ReadUInt32(),
                BlockTableOffset = br.ReadUInt32(),
                HashTableSize = br.ReadUInt32(),
                BlockTableSize = br.ReadUInt32(),
            };

#if DEBUG
            if (header.MpqVersion == 0)
            {
                // Check validity
                // TODO: deal with protected archive DataOffset value.
                if (header.DataOffset != Size)
                {
                    throw new MpqParserException(string.Format("Invalid MPQ header field: DataOffset. Was {0}, expected {1}", header.DataOffset, Size));
                }

                if (header.ArchiveSize != header.BlockTableOffset + (MpqEntry.Size * header.BlockTableSize))
                {
                    throw new MpqParserException(string.Format("Invalid MPQ header field: ArchiveSize. Was {0}, expected {1}", header.ArchiveSize, header.BlockTableOffset + (MpqEntry.Size * header.BlockTableSize)));
                }

                if (header.HashTableOffset != header.ArchiveSize - (MpqHash.Size * header.HashTableSize) - (MpqEntry.Size * header.BlockTableSize))
                {
                    throw new MpqParserException(string.Format("Invalid MPQ header field: HashTablePos. Was {0}, expected {1}", header.HashTableOffset, header.ArchiveSize - (MpqHash.Size * header.HashTableSize) - (MpqEntry.Size * header.BlockTableSize)));
                }

                if (header.BlockTableOffset != header.HashTableOffset + (MpqHash.Size * header.HashTableSize))
                {
                    throw new MpqParserException(string.Format("Invalid MPQ header field: BlockTablePos. Was {0}, expected {1}", header.BlockTableOffset, header.HashTableOffset + (MpqHash.Size * header.HashTableSize)));
                }
            }
#endif

            if (header.MpqVersion == 1)
            {
                header.ExtendedBlockTableOffset = br.ReadInt64();
                header.HashTableOffsetHigh = br.ReadInt16();
                header.BlockTableOffsetHigh = br.ReadInt16();
            }

            return header;
        }

        /// <summary>
        /// Writes the header to the writer.
        /// </summary>
        /// <param name="writer">The writer to which the header will be written.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="writer"/> is null.</exception>
        public void WriteTo(BinaryWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.Write(MpqId);
            writer.Write(DataOffset);
            writer.Write(ArchiveSize);
            writer.Write(MpqVersion);
            writer.Write(BlockSize);
            writer.Write(HashTableOffset);
            writer.Write(BlockTableOffset);
            writer.Write(HashTableSize);
            writer.Write(BlockTableSize);
        }

        internal bool IsArchiveAfterHeader()
        {
            return DataOffset == MpqHeader.Size || HashTableOffset != MpqHeader.Size;
        }
    }
}