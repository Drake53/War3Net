// ------------------------------------------------------------------------------
// <copyright file="ItemClassExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.ComponentModel;

using War3Net.Build.Widget;

namespace War3Net.Build.Extensions
{
    public static class ItemClassExtensions
    {
        public static string GetVariableName(this ItemClass itemClass, bool useLegacyTomeItemType = false)
        {
            return itemClass switch
            {
                ItemClass.Permanent => ItemTypeName.Permanent,
                ItemClass.Charged => ItemTypeName.Charged,
                ItemClass.PowerUp => useLegacyTomeItemType ? ItemTypeName.Tome : ItemTypeName.Powerup,
                ItemClass.Artifact => ItemTypeName.Artifact,
                ItemClass.Purchasable => ItemTypeName.Purchasable,
                ItemClass.Campaign => ItemTypeName.Campaign,
                ItemClass.Miscellaneous => ItemTypeName.Miscellaneous,
                ItemClass.Any => ItemTypeName.Any,

                _ => throw new InvalidEnumArgumentException(nameof(itemClass), (int)itemClass, typeof(ItemClass)),
            };
        }

        private class ItemTypeName
        {
            internal const string Permanent = "ITEM_TYPE_PERMANENT";
            internal const string Charged = "ITEM_TYPE_CHARGED";
            internal const string Tome = "ITEM_TYPE_TOME";
            internal const string Powerup = "ITEM_TYPE_POWERUP";
            internal const string Artifact = "ITEM_TYPE_ARTIFACT";
            internal const string Purchasable = "ITEM_TYPE_PURCHASABLE";
            internal const string Campaign = "ITEM_TYPE_CAMPAIGN";
            internal const string Miscellaneous = "ITEM_TYPE_MISCELLANEOUS";
            internal const string Any = "ITEM_TYPE_ANY";
        }
    }
}