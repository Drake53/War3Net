// ------------------------------------------------------------------------------
// <copyright file="PreviewIcons.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using War3Net.Build.Environment;
using War3Net.Build.Info;
using War3Net.Build.Providers;
using War3Net.Build.Widget;

namespace War3Net.Build
{
    public static partial class MapFactory
    {
        public static MapPreviewIcons PreviewIcons(
            MapInfo mapInfo,
            MapEnvironment mapEnvironment,
            MapUnits mapUnits)
        {
            if (mapInfo is null)
            {
                throw new ArgumentNullException(nameof(mapInfo));
            }

            if (mapEnvironment is null)
            {
                throw new ArgumentNullException(nameof(mapEnvironment));
            }

            if (mapUnits is null)
            {
                throw new ArgumentNullException(nameof(mapUnits));
            }

            if (mapInfo.CameraBoundsComplements is null)
            {
                throw new ArgumentException("Map info must have camera bounds complements.", nameof(mapInfo));
            }

            var left = mapEnvironment.Left + (TerrainTile.TileWidth * mapInfo.CameraBoundsComplements.Left);
            var bottom = mapEnvironment.Bottom + (TerrainTile.TileHeight * mapInfo.CameraBoundsComplements.Bottom);

            var width = mapEnvironment.Right - (TerrainTile.TileWidth * mapInfo.CameraBoundsComplements.Right) - left;
            var height = mapEnvironment.Top - (TerrainTile.TileHeight * mapInfo.CameraBoundsComplements.Top) - bottom;
            var size = width > height ? width : height;

            if (width < size)
            {
                left -= 0.5f * (size - width);
            }

            if (height < size)
            {
                bottom -= 0.5f * (size - height);
            }

            var icons = new List<PreviewIcon>();
            foreach (var unit in mapUnits.Units)
            {
                if (MapPreviewIconProvider.TryGetIcon(unit.TypeId, unit.OwnerId, out var iconType, out var color))
                {
                    var x = 256 * (unit.Position.X - left) / size;
                    var y = 256 * (unit.Position.Y - bottom) / size;

                    icons.Add(new PreviewIcon
                    {
                        IconType = iconType,
                        X = (int)x,
                        Y = 256 - (int)y,
                        Color = color,
                    });
                }
            }

            return new MapPreviewIcons(MapPreviewIconsFormatVersion.v0)
            {
                Icons = icons,
            };
        }
    }
}