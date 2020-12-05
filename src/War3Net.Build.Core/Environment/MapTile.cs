// ------------------------------------------------------------------------------
// <copyright file="MapTile.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

namespace War3Net.Build.Environment
{
    public sealed class MapTile
    {
        public const int TileWidth = 128;
        public const int TileHeight = 128;

        /// <summary>
        /// Size in bytes.
        /// </summary>
        public const int Size = 7;

        private ushort _heightData;
        private ushort _waterDataAndEdgeFlag;
        private byte _textureDataAndFlags;
        private byte _variationData;
        private byte _cliffData;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapTile"/> class.
        /// </summary>
        public MapTile()
        {
        }

        internal MapTile(BinaryReader reader, MapEnvironmentFormatVersion formatVersion)
        {
            ReadFrom(reader, formatVersion);
        }

        public float Height
        {
            get => (_heightData - 8192f) / 512f;
            set => _heightData = (ushort)((value * 512f) + 8192f);
        }

        public float WaterHeight
        {
            get => ((_waterDataAndEdgeFlag & 0x3FFF) - 8192f) / 512f;
            set => _waterDataAndEdgeFlag = (ushort)(((int)((value * 512f) + 8192f) & 0x3FFF) | (_waterDataAndEdgeFlag & 0x4000));
        }

        public bool IsEdgeTile
        {
            get => (_waterDataAndEdgeFlag & 0x4000) != 0;
            set => _waterDataAndEdgeFlag = (ushort)(value ? _waterDataAndEdgeFlag | 0x4000 : _waterDataAndEdgeFlag & 0x3FFF);
        }

        public int Texture
        {
            get => _textureDataAndFlags & 0x0F;
            set => _textureDataAndFlags = (value >= 0 && value <= 0x0F) ? (byte)(value | (_textureDataAndFlags & 0xF0)) : throw new ArgumentOutOfRangeException(nameof(value));
        }

        public bool IsRamp
        {
            get => (_textureDataAndFlags & 0x10) != 0;
            set => _textureDataAndFlags = (byte)(value ? _textureDataAndFlags | 0x10 : _textureDataAndFlags & 0xEF);
        }

        public bool IsBlighted
        {
            get => (_textureDataAndFlags & 0x20) != 0;
            set => _textureDataAndFlags = (byte)(value ? _textureDataAndFlags | 0x20 : _textureDataAndFlags & 0xDF);
        }

        public bool IsWater
        {
            get => (_textureDataAndFlags & 0x40) != 0;
            set => _textureDataAndFlags = (byte)(value ? _textureDataAndFlags | 0x40 : _textureDataAndFlags & 0xBF);
        }

        public bool IsBoundary
        {
            get => (_textureDataAndFlags & 0x80) != 0;
            set => _textureDataAndFlags = (byte)(value ? _textureDataAndFlags | 0x80 : _textureDataAndFlags & 0x7F);
        }

        public int Variation
        {
            get => _variationData & 0x0F;
            set => _variationData = (value >= 0 && value <= 0x0F) ? (byte)(value | (_variationData & 0xF0)) : throw new ArgumentOutOfRangeException(nameof(value));
        }

        public int CliffVariation
        {
            get => (_variationData & 0xF0) >> 4;
            set => _variationData = (value >= 0 && value <= 0x0F) ? (byte)((value << 4) | (_variationData & 0x0F)) : throw new ArgumentOutOfRangeException(nameof(value));
        }

        public int CliffLevel
        {
            get => _cliffData & 0x0F;
            set => _cliffData = (value >= 0 && value <= 0x0F) ? (byte)(value | (_cliffData & 0xF0)) : throw new ArgumentOutOfRangeException(nameof(value));
        }

        public int CliffTexture
        {
            get => (_cliffData & 0xF0) >> 4;
            set => _cliffData = (value >= 0 && value <= 0x0F) ? (byte)((value << 4) | (_cliffData & 0x0F)) : throw new ArgumentOutOfRangeException(nameof(value));
        }

        internal void ReadFrom(BinaryReader reader, MapEnvironmentFormatVersion formatVersion)
        {
            _heightData = reader.ReadUInt16();
            _waterDataAndEdgeFlag = reader.ReadUInt16();
            _textureDataAndFlags = reader.ReadByte();
            _variationData = reader.ReadByte();
            _cliffData = reader.ReadByte();
        }

        internal void WriteTo(BinaryWriter writer, MapEnvironmentFormatVersion formatVersion)
        {
            writer.Write(_heightData);
            writer.Write(_waterDataAndEdgeFlag);
            writer.Write(_textureDataAndFlags);
            writer.Write(_variationData);
            writer.Write(_cliffData);
        }
    }
}