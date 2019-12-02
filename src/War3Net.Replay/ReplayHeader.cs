// ------------------------------------------------------------------------------
// <copyright file="ReplayHeader.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

namespace War3Net.Replay
{
    public sealed class ReplayHeader
    {
        private const string HeaderString = "Warcraft III recorded game\u001a\0";

        private uint _dataOffset;
        private uint _compressedSize;
        private uint _headerVersion;
        private uint _decompressedSize;
        private uint _dataBlocksCount;

        private uint _gameIdentifier;
        private uint _gameVersion;
        private ushort _build;
        private ushort _flags;
        private uint _gameLength;
        private uint _checksum;

        private ReplayHeader()
        {
        }

        public uint DataOffset => _dataOffset;

        public uint CompressedSize => _compressedSize;

        public uint DecompressedSize => _decompressedSize;

        public uint DataBlocksCount => _dataBlocksCount;

        public static ReplayHeader Parse(Stream stream)
        {
            using var reader = new BinaryReader(stream);
            return FromReader(reader);
        }

        public static ReplayHeader FromReader(BinaryReader reader)
        {
            if (new string(reader.ReadChars(HeaderString.Length)) != HeaderString)
            {
                throw new InvalidDataException("Invalid file header.");
            }

            var header = new ReplayHeader();
            header._dataOffset = reader.ReadUInt32();
            header._compressedSize = reader.ReadUInt32();
            header._headerVersion = reader.ReadUInt32();
            header._decompressedSize = reader.ReadUInt32();
            header._dataBlocksCount = reader.ReadUInt32();

            if (header._headerVersion == 0x01)
            {
                header._gameIdentifier = reader.ReadUInt32();
                header._gameVersion = reader.ReadUInt32();
                header._build = reader.ReadUInt16();
                header._flags = reader.ReadUInt16();
                header._gameLength = reader.ReadUInt32();
                header._checksum = reader.ReadUInt32();
            }
            else
            {
                throw new NotSupportedException("Only header version 1 is supported.");
            }

            return header;
        }
    }
}