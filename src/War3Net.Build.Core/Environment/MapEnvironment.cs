// ------------------------------------------------------------------------------
// <copyright file="MapEnvironment.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using War3Net.Build.Common;
using War3Net.Build.Extensions;
using War3Net.Build.Providers;
using War3Net.Common.Extensions;

namespace War3Net.Build.Environment
{
    public sealed class MapEnvironment
    {
        public const string FileName = "war3map.w3e";

        public static readonly int FileFormatSignature = "W3E!".FromRawcode();

        /// <summary>
        /// Initializes a new instance of the <see cref="MapEnvironment"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public MapEnvironment(MapEnvironmentFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        internal MapEnvironment(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        public MapEnvironmentFormatVersion FormatVersion { get; set; }

        public Tileset Tileset { get; set; }

        public bool IsCustomTileset { get; set; }

        public List<TerrainType> TerrainTypes { get; init; } = new();

        public List<CliffType> CliffTypes { get; init; } = new();

        public uint Width { get; set; }

        public uint Height { get; set; }

        public float Left { get; set; }

        public float Bottom { get; set; }

        public List<TerrainTile> TerrainTiles { get; init; } = new();

        public float Right
        {
            get => Left + MapWidth;
            set => Left = value - MapWidth;
        }

        public float Top
        {
            get => Bottom + MapHeight;
            set => Bottom = value - MapHeight;
        }

        public float MapWidth => TerrainTile.TileWidth * Width;

        public float MapHeight => TerrainTile.TileHeight * Height;

        public bool IsDefaultTileset()
        {
            return AreListsEqual(TerrainTypes, TerrainTypeProvider.GetTerrainTypes(Tileset).ToArray())
                && AreListsEqual(CliffTypes, TerrainTypeProvider.GetCliffTypes(Tileset).ToArray());
        }

        public override string ToString() => FileName;

        internal void ReadFrom(BinaryReader reader)
        {
            if (reader.ReadInt32() != FileFormatSignature)
            {
                throw new InvalidDataException($"Expected file header signature at the start of a '{FileName}' file.");
            }

            FormatVersion = reader.ReadInt32<MapEnvironmentFormatVersion>();
            Tileset = (Tileset)reader.ReadChar();
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
            writer.Write((char)Tileset);
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

        private static bool AreListsEqual(IList list1, IList list2)
        {
            var count = list1.Count;
            if (count != list2.Count)
            {
                return false;
            }

            for (var i = 0; i < count; i++)
            {
                if (!Equals(list1[i], list2[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}