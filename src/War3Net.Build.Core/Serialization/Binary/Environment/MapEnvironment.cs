// ------------------------------------------------------------------------------
// <copyright file="MapEnvironment.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Build.Common;
using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Environment
{
    public sealed partial class MapEnvironment
    {
        internal MapEnvironment(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        internal void ReadFrom(BinaryReader reader)
        {
            if (reader.ReadInt32() != FileFormatSignature)
            {
                throw new InvalidDataException($"Expected file header signature at the start of a '{FileName}' file.");
            }

            FormatVersion = reader.ReadInt32<MapEnvironmentFormatVersion>();
            Tileset = reader.ReadByte<Tileset>();
            IsCustomTileset = reader.ReadBool();

            nint terrainTypeCount = reader.ReadInt32();
            for (nint i = 0; i < terrainTypeCount; i++)
            {
                TerrainTypes.Add(reader.ReadInt32<TerrainType>());
            }

            nint cliffTypeCount = reader.ReadInt32();
            for (nint i = 0; i < cliffTypeCount; i++)
            {
                CliffTypes.Add(reader.ReadInt32<CliffType>());
            }

            Width = reader.ReadUInt32() - 1;
            Height = reader.ReadUInt32() - 1;
            Left = reader.ReadSingle();
            Bottom = reader.ReadSingle();

            for (nint y = 0; y <= Width; y++)
            {
                for (nint x = 0; x <= Height; x++)
                {
                    TerrainTiles.Add(reader.ReadTerrainTile(FormatVersion));
                }
            }
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write(FileFormatSignature);
            writer.Write((int)FormatVersion);
            writer.Write((byte)Tileset);
            writer.WriteBool(IsCustomTileset);

            writer.Write(TerrainTypes.Count);
            foreach (var terrainType in TerrainTypes)
            {
                writer.Write((int)terrainType);
            }

            writer.Write(CliffTypes.Count);
            foreach (var cliffType in CliffTypes)
            {
                writer.Write((int)cliffType);
            }

            writer.Write(Width + 1);
            writer.Write(Height + 1);
            writer.Write(Left);
            writer.Write(Bottom);

            foreach (var terrainTile in TerrainTiles)
            {
                writer.Write(terrainTile, FormatVersion);
            }
        }
    }
}