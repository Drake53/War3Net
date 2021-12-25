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
                { "ugol".FromRawcode(), PreviewIconType.GoldMine },
                { "egol".FromRawcode(), PreviewIconType.GoldMine },
                { "ngol".FromRawcode(), PreviewIconType.GoldMine },
                { "ngme".FromRawcode(), PreviewIconType.NeutralBuilding },
                { "nfoh".FromRawcode(), PreviewIconType.NeutralBuilding },
                { "nmoo".FromRawcode(), PreviewIconType.NeutralBuilding },
                { "ngad".FromRawcode(), PreviewIconType.NeutralBuilding },
                { "nwgt".FromRawcode(), PreviewIconType.NeutralBuilding },
                { "ndrr".FromRawcode(), PreviewIconType.NeutralBuilding },
                { "nmer".FromRawcode(), PreviewIconType.NeutralBuilding },
                { "ntav".FromRawcode(), PreviewIconType.NeutralBuilding },
                { "nmrk".FromRawcode(), PreviewIconType.NeutralBuilding },
                { "nshp".FromRawcode(), PreviewIconType.NeutralBuilding },
                { "sloc".FromRawcode(), PreviewIconType.PlayerStartLocation },
            };
        }
    }
}