// ------------------------------------------------------------------------------
// <copyright file="ItemIntegerFieldApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Enums.Object;

namespace War3Net.Runtime.Api.Common.Enums.Object
{
    public static class ItemIntegerFieldApi
    {
        public static readonly ItemIntegerField ITEM_IF_LEVEL = ConvertItemIntegerField((int)ItemIntegerField.Type.LEVEL);
        public static readonly ItemIntegerField ITEM_IF_NUMBER_OF_CHARGES = ConvertItemIntegerField((int)ItemIntegerField.Type.NUMBER_OF_CHARGES);
        public static readonly ItemIntegerField ITEM_IF_COOLDOWN_GROUP = ConvertItemIntegerField((int)ItemIntegerField.Type.COOLDOWN_GROUP);
        public static readonly ItemIntegerField ITEM_IF_MAX_HIT_POINTS = ConvertItemIntegerField((int)ItemIntegerField.Type.MAX_HIT_POINTS);
        public static readonly ItemIntegerField ITEM_IF_HIT_POINTS = ConvertItemIntegerField((int)ItemIntegerField.Type.HIT_POINTS);
        public static readonly ItemIntegerField ITEM_IF_PRIORITY = ConvertItemIntegerField((int)ItemIntegerField.Type.PRIORITY);
        public static readonly ItemIntegerField ITEM_IF_ARMOR_TYPE = ConvertItemIntegerField((int)ItemIntegerField.Type.ARMOR_TYPE);
        public static readonly ItemIntegerField ITEM_IF_TINTING_COLOR_RED = ConvertItemIntegerField((int)ItemIntegerField.Type.TINTING_COLOR_RED);
        public static readonly ItemIntegerField ITEM_IF_TINTING_COLOR_GREEN = ConvertItemIntegerField((int)ItemIntegerField.Type.TINTING_COLOR_GREEN);
        public static readonly ItemIntegerField ITEM_IF_TINTING_COLOR_BLUE = ConvertItemIntegerField((int)ItemIntegerField.Type.TINTING_COLOR_BLUE);
        public static readonly ItemIntegerField ITEM_IF_TINTING_COLOR_ALPHA = ConvertItemIntegerField((int)ItemIntegerField.Type.TINTING_COLOR_ALPHA);

        public static ItemIntegerField ConvertItemIntegerField(int i)
        {
            return ItemIntegerField.GetItemIntegerField(i);
        }
    }
}