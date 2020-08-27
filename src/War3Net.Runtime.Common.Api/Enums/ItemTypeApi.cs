// ------------------------------------------------------------------------------
// <copyright file="ItemTypeApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using System;

using War3Net.Runtime.Common.Enums;

namespace War3Net.Runtime.Common.Api.Enums
{
    public static class ItemTypeApi
    {
        public static readonly ItemType ITEM_TYPE_PERMANENT = ConvertItemType((int)ItemType.Type.Permanent);
        public static readonly ItemType ITEM_TYPE_CHARGED = ConvertItemType((int)ItemType.Type.Charged);
        public static readonly ItemType ITEM_TYPE_POWERUP = ConvertItemType((int)ItemType.Type.Powerup);
        public static readonly ItemType ITEM_TYPE_ARTIFACT = ConvertItemType((int)ItemType.Type.Artifact);
        public static readonly ItemType ITEM_TYPE_PURCHASABLE = ConvertItemType((int)ItemType.Type.Purchasable);
        public static readonly ItemType ITEM_TYPE_CAMPAIGN = ConvertItemType((int)ItemType.Type.Campaign);
        public static readonly ItemType ITEM_TYPE_MISCELLANEOUS = ConvertItemType((int)ItemType.Type.Miscellaneous);
        public static readonly ItemType ITEM_TYPE_UNKNOWN = ConvertItemType((int)ItemType.Type.Unknown);
        public static readonly ItemType ITEM_TYPE_ANY = ConvertItemType((int)ItemType.Type.Any);

        [Obsolete("Use " + nameof(ITEM_TYPE_POWERUP) + " instead")]
        public static readonly ItemType ITEM_TYPE_TOME = ConvertItemType((int)ItemType.Type.Powerup);

        public static ItemType ConvertItemType(int i)
        {
            return ItemType.GetItemType(i);
        }
    }
}