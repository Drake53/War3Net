// ------------------------------------------------------------------------------
// <copyright file="ItemClassExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.ComponentModel;

using War3Net.Build.Widget;

using static War3Api.Common;

namespace War3Net.Build.Extensions
{
    public static class ItemClassExtensions
    {
        public static string GetVariableName(this ItemClass itemClass, bool useLegacyTomeItemType = false)
        {
            return itemClass switch
            {
                ItemClass.Permanent => nameof(ITEM_TYPE_PERMANENT),
                ItemClass.Charged => nameof(ITEM_TYPE_CHARGED),
                ItemClass.PowerUp => useLegacyTomeItemType ? nameof(ITEM_TYPE_TOME) : nameof(ITEM_TYPE_POWERUP),
                ItemClass.Artifact => nameof(ITEM_TYPE_ARTIFACT),
                ItemClass.Purchasable => nameof(ITEM_TYPE_PURCHASABLE),
                ItemClass.Campaign => nameof(ITEM_TYPE_CAMPAIGN),
                ItemClass.Miscellaneous => nameof(ITEM_TYPE_MISCELLANEOUS),
                ItemClass.Any => nameof(ITEM_TYPE_ANY),

                _ => throw new InvalidEnumArgumentException(nameof(itemClass), (int)itemClass, typeof(ItemClass)),
            };
        }
    }
}