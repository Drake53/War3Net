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
        private static readonly Dictionary<int, PreviewIconType> _iconTypes = GetIconTypesDictionary();

        public static bool TryGetIcon(int unitTypeId, int ownerId, out PreviewIconType iconType, out Color color)
        {
            color = Color.FromArgb(-1);
            if (_iconTypes.TryGetValue(unitTypeId, out iconType))
            {
                if (iconType == PreviewIconType.PlayerStartLocation)
                {
                    color = PlayerColor.FromKnownColor((KnownPlayerColor)ownerId);
                }

                return true;
            }

            return false;
        }

        private static Dictionary<int, PreviewIconType> GetIconTypesDictionary()
        {
            return new Dictionary<int, PreviewIconType>()
            {
                { "egol".FromRawcode(), PreviewIconType.GoldMine }, // Entangled Gold Mine
                { "ngol".FromRawcode(), PreviewIconType.GoldMine }, // Gold Mine
                { "ugol".FromRawcode(), PreviewIconType.GoldMine }, // Haunted Gold Mine
                { "ndrg".FromRawcode(), PreviewIconType.NeutralBuilding }, // Green Dragon Roost
                { "ndrk".FromRawcode(), PreviewIconType.NeutralBuilding }, // Black Dragon Roost
                { "ndro".FromRawcode(), PreviewIconType.NeutralBuilding }, // Nether Dragon Roost
                { "ndrr".FromRawcode(), PreviewIconType.NeutralBuilding }, // Red Dragon Roost
                { "ndru".FromRawcode(), PreviewIconType.NeutralBuilding }, // Blue Dragon Roost
                { "ndrz".FromRawcode(), PreviewIconType.NeutralBuilding }, // Bronze Dragon Roost
                { "nfoh".FromRawcode(), PreviewIconType.NeutralBuilding }, // Fountain of Health
                { "ngad".FromRawcode(), PreviewIconType.NeutralBuilding }, // Goblin Laboratory
                { "ngme".FromRawcode(), PreviewIconType.NeutralBuilding }, // Goblin Merchant
                { "nmer".FromRawcode(), PreviewIconType.NeutralBuilding }, // Mercenary Camp (Lordaeron Summer)
                { "nmra".FromRawcode(), PreviewIconType.NeutralBuilding }, // Mercenary Camp (Dungeon)
                { "nmrb".FromRawcode(), PreviewIconType.NeutralBuilding }, // Mercenary Camp (Underground)
                { "nmrc".FromRawcode(), PreviewIconType.NeutralBuilding }, // Mercenary Camp (Sunken Ruins)
                { "nmrd".FromRawcode(), PreviewIconType.NeutralBuilding }, // Mercenary Camp (Icecrown Glacier)
                { "nmre".FromRawcode(), PreviewIconType.NeutralBuilding }, // Mercenary Camp (Outland)
                { "nmrf".FromRawcode(), PreviewIconType.NeutralBuilding }, // Mercenary Camp (Black Citadel)
                { "nmr2".FromRawcode(), PreviewIconType.NeutralBuilding }, // Mercenary Camp (Lordaeron Fall)
                { "nmr3".FromRawcode(), PreviewIconType.NeutralBuilding }, // Mercenary Camp (Lordaeron Winter)
                { "nmr4".FromRawcode(), PreviewIconType.NeutralBuilding }, // Mercenary Camp (Barrens)
                { "nmr5".FromRawcode(), PreviewIconType.NeutralBuilding }, // Mercenary Camp (Ashenvale)
                { "nmr6".FromRawcode(), PreviewIconType.NeutralBuilding }, // Mercenary Camp (Felwood)
                { "nmr7".FromRawcode(), PreviewIconType.NeutralBuilding }, // Mercenary Camp (Northrend)
                { "nmr8".FromRawcode(), PreviewIconType.NeutralBuilding }, // Mercenary Camp (Cityscape)
                { "nmr9".FromRawcode(), PreviewIconType.NeutralBuilding }, // Mercenary Camp (Dalaran)
                { "nmr0".FromRawcode(), PreviewIconType.NeutralBuilding }, // Mercenary Camp (Village)
                { "nmoo".FromRawcode(), PreviewIconType.NeutralBuilding }, // Fountain of Mana
                { "nmrk".FromRawcode(), PreviewIconType.NeutralBuilding }, // Marketplace
                { "nshp".FromRawcode(), PreviewIconType.NeutralBuilding }, // Goblin Shipyard
                { "ntav".FromRawcode(), PreviewIconType.NeutralBuilding }, // Tavern
                { "nwgt".FromRawcode(), PreviewIconType.NeutralBuilding }, // Way Gate
                { "sloc".FromRawcode(), PreviewIconType.PlayerStartLocation }, // Start Location
            };
        }
    }
}