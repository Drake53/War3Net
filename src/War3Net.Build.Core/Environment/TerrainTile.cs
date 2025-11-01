// ------------------------------------------------------------------------------
// <copyright file="TerrainTile.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.Build.Environment
{
    public sealed partial class TerrainTile
    {
        public const int TileWidth = 128;
        public const int TileHeight = 128;

        /// <summary>
        /// Size in bytes.
        /// </summary>
        public const int Size = 7;

        private ushort _heightData;
        private ushort _waterDataAndEdgeFlag;
        private byte _textureData;
        private TileFlags _tileFlags;
        private byte _variationData;
        private byte _cliffData;

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrainTile"/> class.
        /// </summary>
        public TerrainTile()
        {
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
            get => _textureData;
            set => _textureData = (value >= 0 && value < 64) ? (byte)value : throw new ArgumentOutOfRangeException(nameof(value));
        }

        public TileFlags Flags
        {
            get => _tileFlags;
            set => _tileFlags = value;
        }

        public bool IsRamp
        {
            get => _tileFlags.HasFlag(TileFlags.Ramp);
            set => _tileFlags = value ? _tileFlags | TileFlags.Ramp : _tileFlags & ~TileFlags.Ramp;
        }

        public bool IsBlighted
        {
            get => _tileFlags.HasFlag(TileFlags.Blighted);
            set => _tileFlags = value ? _tileFlags | TileFlags.Blighted : _tileFlags & ~TileFlags.Blighted;
        }

        public bool IsWater
        {
            get => _tileFlags.HasFlag(TileFlags.Water);
            set => _tileFlags = value ? _tileFlags | TileFlags.Water : _tileFlags & ~TileFlags.Water;
        }

        public bool IsBoundary
        {
            get => _tileFlags.HasFlag(TileFlags.Boundary);
            set => _tileFlags = value ? _tileFlags | TileFlags.Boundary : _tileFlags & ~TileFlags.Boundary;
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
    }
}