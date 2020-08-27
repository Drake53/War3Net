// ------------------------------------------------------------------------------
// <copyright file="ArmorTypeApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Common.Enums;

namespace War3Net.Runtime.Common.Api.Enums
{
    public static class ArmorTypeApi
    {
        public static readonly ArmorType ARMOR_TYPE_WHOKNOWS = ConvertArmorType((int)ArmorType.Type.Undefined);
        public static readonly ArmorType ARMOR_TYPE_FLESH = ConvertArmorType((int)ArmorType.Type.Flesh);
        public static readonly ArmorType ARMOR_TYPE_METAL = ConvertArmorType((int)ArmorType.Type.Metal);
        public static readonly ArmorType ARMOR_TYPE_WOOD = ConvertArmorType((int)ArmorType.Type.Wood);
        public static readonly ArmorType ARMOR_TYPE_ETHREAL = ConvertArmorType((int)ArmorType.Type.Ethereal);
        public static readonly ArmorType ARMOR_TYPE_STONE = ConvertArmorType((int)ArmorType.Type.Stone);

        public static ArmorType ConvertArmorType(int i)
        {
            return ArmorType.GetArmorType(i);
        }
    }
}