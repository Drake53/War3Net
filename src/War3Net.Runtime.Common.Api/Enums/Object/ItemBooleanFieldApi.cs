// ------------------------------------------------------------------------------
// <copyright file="ItemBooleanFieldApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Common.Enums.Object;

namespace War3Net.Runtime.Common.Api.Enums.Object
{
    public static class ItemBooleanFieldApi
    {
        public static readonly ItemBooleanField ITEM_BF_DROPPED_WHEN_CARRIER_DIES = ConvertItemBooleanField((int)ItemBooleanField.Type.DROPPED_WHEN_CARRIER_DIES);
        public static readonly ItemBooleanField ITEM_BF_CAN_BE_DROPPED = ConvertItemBooleanField((int)ItemBooleanField.Type.CAN_BE_DROPPED);
        public static readonly ItemBooleanField ITEM_BF_PERISHABLE = ConvertItemBooleanField((int)ItemBooleanField.Type.PERISHABLE);
        public static readonly ItemBooleanField ITEM_BF_INCLUDE_AS_RANDOM_CHOICE = ConvertItemBooleanField((int)ItemBooleanField.Type.INCLUDE_AS_RANDOM_CHOICE);
        public static readonly ItemBooleanField ITEM_BF_USE_AUTOMATICALLY_WHEN_ACQUIRED = ConvertItemBooleanField((int)ItemBooleanField.Type.USE_AUTOMATICALLY_WHEN_ACQUIRED);
        public static readonly ItemBooleanField ITEM_BF_CAN_BE_SOLD_TO_MERCHANTS = ConvertItemBooleanField((int)ItemBooleanField.Type.CAN_BE_SOLD_TO_MERCHANTS);
        public static readonly ItemBooleanField ITEM_BF_ACTIVELY_USED = ConvertItemBooleanField((int)ItemBooleanField.Type.ACTIVELY_USED);

        public static ItemBooleanField ConvertItemBooleanField(int i)
        {
            return ItemBooleanField.GetItemBooleanField(i);
        }
    }
}