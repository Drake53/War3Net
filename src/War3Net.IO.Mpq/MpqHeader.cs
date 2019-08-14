// ------------------------------------------------------------------------------
// <copyright file="MpqHeader.cs" company="Foole (fooleau@gmail.com)">
// Copyright (c) 2006 Foole (fooleau@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
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

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqHeader"/> class.
        /// </summary>
        /// <param name="fileArchiveSize">The length (in bytes) of the file archive.</param>
        /// <param name="hashTableEntries">The amount of <see cref="MpqHash"/> objects in the <see cref="HashTable"/>.</param>
        /// <param name="blockTableEntries">The amount of <see cref="MpqEntry"/> objects in the <see cref="BlockTable"/>.</param>
        /// <param name="blockSize"></param>
        /// <param name="archiveBeforeTables"></param>
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
                HashTablePos = Size + fileArchiveSize;
                BlockTablePos = Size + fileArchiveSize + hashTableSize;
                HashTableSize = hashTableEntries;
                BlockTableSize = blockTableEntries;
            }
            else
            {
                // MPQ contents are in order: header, HT, BT, archive
                throw new NotImplementedException();
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
        /// Gets size of the <see cref="MpqHeader"/>, indicating the offset of the first file.
        /// </summary>
        public uint DataOffset { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public uint ArchiveSize { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public ushort MpqVersion { get; private set; } // Most are 0.  Burning Crusade = 1

        /// <summary>
        /// 
        /// </summary>
        public ushort BlockSize { get; private set; } // Size of file block is 0x200 << BlockSize

        /// <summary>
        /// 
        /// </summary>
        public uint HashTablePos { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public uint BlockTablePos { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public uint HashTableSize { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public uint BlockTableSize { get; private set; }

        // Version 1 fields
        // The extended block table is an array of Int16 - higher bits of the offests in the block table.
        public long ExtendedBlockTableOffset { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public short HashTableOffsetHigh { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public short BlockTableOffsetHigh { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        public static MpqHeader FromReader(BinaryReader br)
        {
            var id = br?.ReadUInt32() ?? throw new ArgumentNullException(nameof(br));
            if (id != MpqId)
            {
                return null;
            }

            var header = new MpqHeader
            {
                ID = id,
                DataOffset = br.ReadUInt32(),
                ArchiveSize = br.ReadUInt32(),
                MpqVersion = br.ReadUInt16(),
                BlockSize = br.ReadUInt16(),
                HashTablePos = br.ReadUInt32(),
                BlockTablePos = br.ReadUInt32(),
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

                if (header.ArchiveSize != header.BlockTablePos + (MpqEntry.Size * header.BlockTableSize))
                {
                    throw new MpqParserException(string.Format("Invalid MPQ header field: ArchiveSize. Was {0}, expected {1}", header.ArchiveSize, header.BlockTablePos + (MpqEntry.Size * header.BlockTableSize)));
                }

                if (header.HashTablePos != header.ArchiveSize - (MpqHash.Size * header.HashTableSize) - (MpqEntry.Size * header.BlockTableSize))
                {
                    throw new MpqParserException(string.Format("Invalid MPQ header field: HashTablePos. Was {0}, expected {1}", header.HashTablePos, header.ArchiveSize - (MpqHash.Size * header.HashTableSize) - (MpqEntry.Size * header.BlockTableSize)));
                }

                if (header.BlockTablePos != header.HashTablePos + (MpqHash.Size * header.HashTableSize))
                {
                    throw new MpqParserException(string.Format("Invalid MPQ header field: BlockTablePos. Was {0}, expected {1}", header.BlockTablePos, header.HashTablePos + (MpqHash.Size * header.HashTableSize)));
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
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public void WriteToStream(BinaryWriter writer)
        {
            writer.Write(MpqId);
            writer.Write(DataOffset);
            writer.Write(ArchiveSize);
            writer.Write(MpqVersion);
            writer.Write(BlockSize);
            writer.Write(HashTablePos);
            writer.Write(BlockTablePos);
            writer.Write(HashTableSize);
            writer.Write(BlockTableSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerOffset"></param>
        public bool SetHeaderOffset(long headerOffset)
        {
            // A protected archive. Seen in some custom wc3 maps.
            const uint ProtectedOffset = 0x6d9e4b86;

            HashTablePos += (uint)headerOffset;
            BlockTablePos += (uint)headerOffset;
            if (DataOffset == ProtectedOffset)
            {
                DataOffset = (uint)(Size + headerOffset);
            }

            return true;
        }
    }
}