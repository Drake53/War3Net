// ------------------------------------------------------------------------------
// <copyright file="MpqEntry.cs" company="Foole (fooleau@gmail.com)">
// Copyright (c) 2006 Foole (fooleau@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

namespace War3Net.IO.Mpq
{
    public class MpqEntry
    {
        /// <summary>
        /// The length (in bytes) of an <see cref="MpqEntry"/>.
        /// </summary>
        public const uint Size = 16;

        private uint _fileOffset; // Relative to the header offset
        private string _filename;

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqEntry"/> class.
        /// </summary>
        /// <param name="br"></param>
        /// <param name="headerOffset"></param>
        public MpqEntry(BinaryReader br, uint headerOffset)
        {
            _fileOffset = br.ReadUInt32();
            FilePos = headerOffset + _fileOffset;
            CompressedSize = br.ReadUInt32();
            FileSize = br.ReadUInt32();
            Flags = (MpqFileFlags)br.ReadUInt32();

            IsAdded = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqEntry"/> class.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="compressedSize"></param>
        /// <param name="fileSize"></param>
        /// <param name="flags"></param>
        internal MpqEntry(string filename, uint compressedSize, uint fileSize, MpqFileFlags flags)
        {
            CompressedSize = compressedSize;
            FileSize = fileSize;
            Flags = flags;

            Filename = filename;
        }

        /// <summary>
        /// 
        /// </summary>
        public static MpqEntry Dummy => new MpqEntry(null, 0, 0, MpqFileFlags.Exists); //remove exists flag??

        /// <summary>
        /// Gets the compressed file size of this <see cref="MpqEntry"/>.
        /// </summary>
        public uint CompressedSize { get; private set; }

        /// <summary>
        /// Gets the uncompressed file size of this <see cref="MpqEntry"/>.
        /// </summary>
        public uint FileSize { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public MpqFileFlags Flags { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public uint EncryptionSeed { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string Filename
        {
            get => _filename;
            set
            {
                _filename = value;
                EncryptionSeed = CalculateEncryptionSeed();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="MpqEntry"/> has the flag <see cref="MpqFileFlags.Encrypted"/>.
        /// </summary>
        public bool IsEncrypted => Flags.HasFlag(MpqFileFlags.Encrypted);

        /// <summary>
        /// Gets a value indicating whether this <see cref="MpqEntry"/> has the flag <see cref="MpqFileFlags.Compressed"/>.
        /// </summary>
        public bool IsCompressed => (Flags & MpqFileFlags.Compressed) != 0;

        /// <summary>
        /// 
        /// </summary>
        public bool Exists => Flags != 0;

        /// <summary>
        /// Gets a value indicating whether this <see cref="MpqEntry"/> has the flag <see cref="MpqFileFlags.SingleUnit"/>.
        /// </summary>
        public bool IsSingleUnit => Flags.HasFlag(MpqFileFlags.SingleUnit);

        /// <summary>
        /// Gets the absolute position of this <see cref="MpqEntry"/>'s file in the <see cref="MpqArchive"/>.
        /// </summary>
        internal uint FilePos { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="MpqEntry"/> has been added to an <see cref="MpqArchive"/>.
        /// </summary>
        internal bool IsAdded { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerOffset"></param>
        /// <param name="fileOffset"></param>
        /// <exception cref="InvalidOperationException">Thrown when the <see cref="IsAdded"/> property is true.</exception>
        public void SetPos(uint headerOffset, uint fileOffset)
        {
            if (IsAdded)
            {
                throw new InvalidOperationException("Cannot change the FilePos for an MpqEntry after it's been set.");
            }

            _fileOffset = fileOffset;
            FilePos = headerOffset + fileOffset;

            IsAdded = true;
            if (EncryptionSeed == 0)
            {
                EncryptionSeed = CalculateEncryptionSeed();
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Filename ?? (!Exists ? "(Deleted file)" : $"Unknown file @ {FilePos}");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="writer"></param>
        internal void WriteEntry(BinaryWriter writer)
        {
            writer.Write(_fileOffset);
            writer.Write(CompressedSize);
            writer.Write(FileSize);
            writer.Write((uint)Flags);
        }

        private uint CalculateEncryptionSeed()
        {
            if (Filename == null || (!IsAdded && (Flags & MpqFileFlags.BlockOffsetAdjustedKey) == MpqFileFlags.BlockOffsetAdjustedKey))
            {
                return 0;
            }

            var seed = StormBuffer.HashString(Path.GetFileName(Filename), 0x300);
            if ((Flags & MpqFileFlags.BlockOffsetAdjustedKey) == MpqFileFlags.BlockOffsetAdjustedKey)
            {
                seed = (seed + _fileOffset) ^ FileSize;
            }

            return seed;
        }
    }
}