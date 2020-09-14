// ------------------------------------------------------------------------------
// <copyright file="PackedFileHeader.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

using War3Net.Common.Extensions;

namespace War3Net.Replay
{
    // Header for replays, saved games, and gamecache.
    public sealed class PackedFileHeader
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
        private ushort _flags; // 0x0000 singleplayer, 0x8000 multiplayer
        private uint _gameLength; // in milliseconds, set to 0 if file is not a replay
        private uint _checksum;

        private PackedFileHeader()
        {
        }

        public uint DataOffset => _dataOffset;

        public uint CompressedSize => _compressedSize;

        public uint DecompressedSize => _decompressedSize;

        public uint DataBlocksCount => _dataBlocksCount;

        public bool IsReforgedReplay => _gameVersion >= 10032;

        public static PackedFileHeader Parse(Stream stream)
        {
            using var reader = new BinaryReader(stream);
            return FromReader(reader);
        }

        public static PackedFileHeader FromReader(BinaryReader reader)
        {
            if (new string(reader.ReadChars(HeaderString.Length)) != HeaderString)
            {
                throw new InvalidDataException("Invalid file header.");
            }

            using var crcStream = new MemoryStream();
            using var crcWriter = new BinaryWriter(crcStream);

            var header = new PackedFileHeader();
            header._dataOffset = reader.ReadUInt32();
            header._compressedSize = reader.ReadUInt32();
            header._headerVersion = reader.ReadUInt32();
            header._decompressedSize = reader.ReadUInt32();
            header._dataBlocksCount = reader.ReadUInt32();

            crcWriter.WriteString(HeaderString);
            crcWriter.Write(header._dataOffset);
            crcWriter.Write(header._compressedSize);
            crcWriter.Write(header._headerVersion);
            crcWriter.Write(header._decompressedSize);
            crcWriter.Write(header._dataBlocksCount);

            if (header._headerVersion == 0x00)
            {
                if (header._dataOffset != 0x40)
                {
                    throw new InvalidDataException("Invalid header size.");
                }

                if (reader.ReadUInt16() != 0)
                {
                    throw new InvalidDataException();
                }

                header._gameVersion = reader.ReadUInt16();

                crcWriter.Write((ushort)0);
                crcWriter.Write((ushort)header._gameVersion);
            }
            else if (header._headerVersion == 0x01)
            {
                if (header._dataOffset != 0x44)
                {
                    throw new InvalidDataException("Invalid header size.");
                }

                header._gameIdentifier = reader.ReadUInt32();
                header._gameVersion = reader.ReadUInt32();

                crcWriter.Write(header._gameIdentifier);
                crcWriter.Write(header._gameVersion);
            }
            else
            {
                throw new NotSupportedException("Only header version 1 is supported.");
            }

            header._build = reader.ReadUInt16();
            header._flags = reader.ReadUInt16();
            header._gameLength = reader.ReadUInt32();
            header._checksum = reader.ReadUInt32();

            crcWriter.Write(header._build);
            crcWriter.Write(header._flags);
            crcWriter.Write(header._gameLength);
            crcWriter.Write(0U);

            crcStream.Position = 0;
            var crc = new Ionic.Crc.CRC32().GetCrc32(crcStream);
            if (crc != header._checksum)
            {
                throw new InvalidDataException("CRC failed");
            }

            return header;
        }
    }
}