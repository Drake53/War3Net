// ------------------------------------------------------------------------------
// <copyright file="MapEnvironment.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Common;
using War3Net.Build.Providers;
using War3Net.Common.Extensions;

namespace War3Net.Build.Environment
{
    public sealed partial class MapEnvironment
    {
        public const string FileExtension = ".w3e";
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