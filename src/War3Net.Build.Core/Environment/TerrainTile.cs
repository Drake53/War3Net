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

        private ushort _heightData;
        private ushort _waterDataAndEdgeFlag;
        private ushort _textureDataAndFlags;
        private byte _variationData;
        private byte _cliffData;

        private MapEnvironmentFormatVersion _formatVersion;

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrainTile"/> class.
        /// </summary>
        public TerrainTile(MapEnvironmentFormatVersion formatVersion)
        {
            _formatVersion = formatVersion;
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
            get
            {
                if (_formatVersion >= MapEnvironmentFormatVersion.v12)
                {
                    return _textureDataAndFlags & 0x3F;
                }
                return _textureDataAndFlags & 0x0F;
            }

            set
            {
                if (_formatVersion >= MapEnvironmentFormatVersion.v12)
                {
                    _textureDataAndFlags = (value >= 0 && value <= 0x3F) ? (ushort)(value | (_textureDataAndFlags & 0xFFC0)) : throw new ArgumentOutOfRangeException(nameof(value));
                }
                else
                {
                    _textureDataAndFlags = (value >= 0 && value <= 0x0F) ? (ushort)(value | (_textureDataAndFlags & 0xF0)) : throw new ArgumentOutOfRangeException(nameof(value));
                }
            }
        }

        public bool IsRamp
        {
            get
            {
                if (_formatVersion >= MapEnvironmentFormatVersion.v12)
                {
                    return (_textureDataAndFlags & 0x40) != 0;
                }
                return (_textureDataAndFlags & 0x10) != 0;
            }

            set
            {
                if (_formatVersion >= MapEnvironmentFormatVersion.v12)
                {
                    _textureDataAndFlags = (ushort)(value ? _textureDataAndFlags | 0x40 : _textureDataAndFlags & 0xBF);
                }

                _textureDataAndFlags = (ushort)(value ? _textureDataAndFlags | 0x10 : _textureDataAndFlags & 0xEF);
            }
        }

        public bool IsBlighted
        {
            get
            {
                if (_formatVersion >= MapEnvironmentFormatVersion.v12)
                {
                    return (_textureDataAndFlags & 0x80) != 0;
                }
                return (_textureDataAndFlags & 0x20) != 0;
            }

            set
            {
                if (_formatVersion >= MapEnvironmentFormatVersion.v12)
                {
                    _textureDataAndFlags = (ushort)(value ? _textureDataAndFlags | 0x80 : _textureDataAndFlags & 0x7F);
                }
                _textureDataAndFlags = (ushort)(value ? _textureDataAndFlags | 0x20 : _textureDataAndFlags & 0xDF);
            }
        }

        public bool IsWater
        {
            get
            {
                if (_formatVersion >= MapEnvironmentFormatVersion.v12)
                {
                    return (_textureDataAndFlags & 0x100) != 0;
                }
                return (_textureDataAndFlags & 0x40) != 0;
            }

            set
            {
                if (_formatVersion >= MapEnvironmentFormatVersion.v12)
                {
                    _textureDataAndFlags = (ushort)(value ? _textureDataAndFlags | 0x100 : _textureDataAndFlags & 0xEFF);
                }
                else
                {
                    _textureDataAndFlags = (ushort)(value ? _textureDataAndFlags | 0x40 : _textureDataAndFlags & 0xBF);
                }
            }
        }

        public bool IsBoundary
        {
            get
            {
                if (_formatVersion >= MapEnvironmentFormatVersion.v12)
                {
                    return (_textureDataAndFlags & 0x200) != 0;
                }
                return (_textureDataAndFlags & 0x80) != 0;
            }

            set
            {
                if (_formatVersion >= MapEnvironmentFormatVersion.v12)
                {
                    _textureDataAndFlags = (ushort)(value ? _textureDataAndFlags | 0x200 : _textureDataAndFlags & 0xDFF);
                }
                else
                {
                    _textureDataAndFlags = (ushort)(value ? _textureDataAndFlags | 0x80 : _textureDataAndFlags & 0x7F);
                }
            }
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