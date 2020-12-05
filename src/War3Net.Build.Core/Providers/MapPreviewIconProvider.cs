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
using War3Net.Common.Extensions;

namespace War3Net.Build.Providers
{
    public static class MapPreviewIconProvider
    {
        private static readonly Dictionary<int, MapPreviewIconType> _iconTypes = GetIconTypesDictionary();
        private static readonly List<Color> _playerColors = GetPlayerColorsList();

        public static bool TryGetIcon(int unitTypeId, int ownerId, out MapPreviewIconType iconType, out Color color)
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

        private static Dictionary<int, MapPreviewIconType> GetIconTypesDictionary()
        {
            return new Dictionary<int, MapPreviewIconType>()
            {
                { "ugol".FromRawcode(), MapPreviewIconType.GoldMine },
                { "egol".FromRawcode(), MapPreviewIconType.GoldMine },
                { "ngol".FromRawcode(), MapPreviewIconType.GoldMine },
                { "ngme".FromRawcode(), MapPreviewIconType.NeutralBuilding },
                { "nfoh".FromRawcode(), MapPreviewIconType.NeutralBuilding },
                { "nmoo".FromRawcode(), MapPreviewIconType.NeutralBuilding },
                { "ngad".FromRawcode(), MapPreviewIconType.NeutralBuilding },
                { "nwgt".FromRawcode(), MapPreviewIconType.NeutralBuilding },
                { "ndrr".FromRawcode(), MapPreviewIconType.NeutralBuilding },
                { "nmer".FromRawcode(), MapPreviewIconType.NeutralBuilding },
                { "ntav".FromRawcode(), MapPreviewIconType.NeutralBuilding },
                { "nmrk".FromRawcode(), MapPreviewIconType.NeutralBuilding },
                { "nshp".FromRawcode(), MapPreviewIconType.NeutralBuilding },
                { "sloc".FromRawcode(), MapPreviewIconType.PlayerStartLocation },
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