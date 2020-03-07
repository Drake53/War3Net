// ------------------------------------------------------------------------------
// <copyright file="MapPreviewIconProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Drawing;

using War3Net.Build.Common;
using War3Net.Build.Environment;

namespace War3Net.Build.Providers
{
    public static class MapPreviewIconProvider
    {
        private static readonly Dictionary<string, MapPreviewIconType> _iconTypes = GetIconTypesDictionary();
        private static readonly List<Color> _playerColors = GetPlayerColorsList();

        public static bool TryGetIcon(string unitTypeId, int ownerId, out MapPreviewIconType iconType, out Color color)
        {
            color = Color.FromArgb(-1);
            if (_iconTypes.TryGetValue(unitTypeId, out iconType))
            {
                if (iconType == MapPreviewIconType.PlayerStartLocation)
                {
                    color = _playerColors[ownerId];
                }

                return true;
            }

            return false;
        }

        private static Dictionary<string, MapPreviewIconType> GetIconTypesDictionary()
        {
            return new Dictionary<string, MapPreviewIconType>()
            {
                { "ugol", MapPreviewIconType.GoldMine },
                { "egol", MapPreviewIconType.GoldMine },
                { "ngol", MapPreviewIconType.GoldMine },
                { "ngme", MapPreviewIconType.NeutralBuilding },
                { "nfoh", MapPreviewIconType.NeutralBuilding },
                { "nmoo", MapPreviewIconType.NeutralBuilding },
                { "ngad", MapPreviewIconType.NeutralBuilding },
                { "nwgt", MapPreviewIconType.NeutralBuilding },
                { "ndrr", MapPreviewIconType.NeutralBuilding },
                { "nmer", MapPreviewIconType.NeutralBuilding },
                { "ntav", MapPreviewIconType.NeutralBuilding },
                { "nmrk", MapPreviewIconType.NeutralBuilding },
                { "nshp", MapPreviewIconType.NeutralBuilding },
                { "sloc", MapPreviewIconType.PlayerStartLocation },
            };
        }

        private static List<Color> GetPlayerColorsList()
        {
            return new List<Color>
            {
                PlayerColor.Red,
                PlayerColor.Blue,
                PlayerColor.Teal,
                PlayerColor.Purple,
                PlayerColor.Yellow,
                PlayerColor.Orange,
                PlayerColor.Green,
                PlayerColor.Pink,
                PlayerColor.Gray,
                PlayerColor.LightBlue,
                PlayerColor.DarkGreen,
                PlayerColor.Brown,
                PlayerColor.Maroon,
                PlayerColor.Navy,
                PlayerColor.Turquoise,
                PlayerColor.Violet,
                PlayerColor.Wheat,
                PlayerColor.Peach,
                PlayerColor.Mint,
                PlayerColor.Lavender,
                PlayerColor.Coal,
                PlayerColor.Snow,
                PlayerColor.Emerald,
                PlayerColor.Peanut,

                PlayerColor.Black,
                PlayerColor.Black,
                PlayerColor.Black,
                PlayerColor.Black,
            };
        }
    }
}