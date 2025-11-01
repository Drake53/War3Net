// ------------------------------------------------------------------------------
// <copyright file="TerrainTile.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.Build.Environment
{
    public sealed partial class TerrainTile
    {
        internal TerrainTile(BinaryReader reader, MapEnvironmentFormatVersion formatVersion)
        {
            ReadFrom(reader, formatVersion);
        }

        internal void ReadFrom(BinaryReader reader, MapEnvironmentFormatVersion formatVersion)
        {
            _heightData = reader.ReadUInt16();
            _waterDataAndEdgeFlag = reader.ReadUInt16();

            if (formatVersion >= MapEnvironmentFormatVersion.v12)
            {
                var textureDataAndFlags = reader.ReadByte();
                var remainingFlags = reader.ReadByte();

                _textureData = (byte)(textureDataAndFlags & 0x3F);
                _tileFlags = (TileFlags)(((textureDataAndFlags & 0xC0) >> 6) | ((remainingFlags & 0x03) << 2));
            }
            else
            {
                var textureDataAndFlags = reader.ReadByte();

                _textureData = (byte)(textureDataAndFlags & 0x0F);
                _tileFlags = (TileFlags)((textureDataAndFlags & 0xF0) >> 4);
            }

            _variationData = reader.ReadByte();
            _cliffData = reader.ReadByte();
        }

        internal void WriteTo(BinaryWriter writer, MapEnvironmentFormatVersion formatVersion)
        {
            writer.Write(_heightData);
            writer.Write(_waterDataAndEdgeFlag);

            if (formatVersion >= MapEnvironmentFormatVersion.v12)
            {
                writer.Write((byte)((_textureData & 0x3F) | (((byte)_tileFlags & 0x03) << 6)));
                writer.Write((byte)(((byte)_tileFlags & 0x0C) >> 2));
            }
            else
            {
                writer.Write((byte)((_textureData & 0x0F) | (((byte)_tileFlags & 0x0F) << 4)));
            }

            writer.Write(_variationData);
            writer.Write(_cliffData);
        }
    }
}