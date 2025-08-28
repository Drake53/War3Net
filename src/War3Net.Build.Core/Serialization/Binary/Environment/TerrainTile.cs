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
            _formatVersion = formatVersion;
            ReadFrom(reader, formatVersion);
        }

        internal void ReadFrom(BinaryReader reader, MapEnvironmentFormatVersion formatVersion)
        {
            _heightData = reader.ReadUInt16();
            _waterDataAndEdgeFlag = reader.ReadUInt16();
            if (formatVersion >= MapEnvironmentFormatVersion.v12)
            {
                _textureDataAndFlags = reader.ReadUInt16();
            }
            else
            {
                _textureDataAndFlags = reader.ReadByte();
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
                writer.Write(_textureDataAndFlags);
            }
            else
            {
                writer.Write((byte)_textureDataAndFlags);
            }
            writer.Write(_variationData);
            writer.Write(_cliffData);
        }
    }
}