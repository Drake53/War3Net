// ------------------------------------------------------------------------------
// <copyright file="Environment.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq;

using War3Net.Build.Environment;
using War3Net.Build.Info;
using War3Net.Build.Providers;

namespace War3Net.Build
{
    public static partial class MapFactory
    {
        public static MapEnvironment Environment(
            MapInfo mapInfo,
            TerrainType? terrainType = null,
            int cliffLevel = 2,
            WaterLevel waterLevel = WaterLevel.None,
            bool randomHeightField = false)
        {
            if (mapInfo is null)
            {
                throw new ArgumentNullException(nameof(mapInfo));
            }

            if (mapInfo.CameraBoundsComplements is null)
            {
                throw new ArgumentException("Map info must have camera bounds complements.", nameof(mapInfo));
            }

            if (!terrainType.HasValue)
            {
                terrainType = TerrainTypeProvider.GetDefaultTerrainType(mapInfo.Tileset);
            }

            var terrainTypes = TerrainTypeProvider.GetTerrainTypes(mapInfo.Tileset).ToList();
            var cliffTypes = TerrainTypeProvider.GetCliffTypes(mapInfo.Tileset).ToList();

            var tileTexture = terrainTypes.IndexOf(terrainType.Value);
            if (tileTexture == -1)
            {
                throw new InvalidEnumArgumentException(nameof(terrainType), (int)terrainType, typeof(TerrainType));
            }

            if (cliffLevel < 0 || cliffLevel > 14)
            {
                throw new ArgumentOutOfRangeException(nameof(cliffLevel));
            }

            if (!Enum.IsDefined(waterLevel))
            {
                throw new InvalidEnumArgumentException(nameof(waterLevel), (int)waterLevel, typeof(WaterLevel));
            }

            if (cliffLevel == 14 && randomHeightField)
            {
                throw new ArgumentException("Random height field is not available when cliff level is set to the maximum value.", nameof(randomHeightField));
            }

            var leftBound = mapInfo.CameraBoundsComplements.Left;
            var rightBound = mapInfo.CameraBoundsComplements.Right;
            var bottomBound = mapInfo.CameraBoundsComplements.Bottom;
            var topBound = mapInfo.CameraBoundsComplements.Top;

            var width = mapInfo.PlayableMapAreaWidth + leftBound + rightBound;
            var height = mapInfo.PlayableMapAreaHeight + bottomBound + topBound;

            var mapEnvironment = new MapEnvironment(MapEnvironmentFormatVersion.v11)
            {
                Tileset = mapInfo.Tileset,
                TerrainTypes = terrainTypes,
                CliffTypes = cliffTypes,
                Width = (uint)width,
                Height = (uint)height,
                Left = (TerrainTile.TileWidth * width) / -2f,
                Bottom = (TerrainTile.TileHeight * height) / -2f,
            };

            var rightEdge = width - rightBound;
            var topEdge = height - topBound;

            for (nint y = 0; y <= height; y++)
            {
                for (nint x = 0; x <= width; x++)
                {
                    var isEdgeTile = x != width && y != height && (x < leftBound || x >= rightEdge || y < bottomBound || y >= topEdge);

                    mapEnvironment.TerrainTiles.Add(new TerrainTile(mapEnvironment.FormatVersion)
                    {
                        Height = 0,
                        WaterHeight = 0,
                        IsEdgeTile = isEdgeTile,
                        Texture = tileTexture,
                        IsRamp = false,
                        IsBlighted = false,
                        IsWater = false,
                        IsBoundary = false,
                        Variation = 0,
                        CliffVariation = 0,
                        CliffLevel = cliffLevel,
                        CliffTexture = 15,
                    });
                }
            }

            return mapEnvironment;
        }
    }
}