// ------------------------------------------------------------------------------
// <copyright file="UnitCategoryApi.cs" company="Drake53">
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
    public static class UnitCategoryApi
    {
        public static readonly UnitCategory UNIT_CATEGORY_GIANT = ConvertUnitCategory((int)UnitCategory.Type.Giant);
        public static readonly UnitCategory UNIT_CATEGORY_UNDEAD = ConvertUnitCategory((int)UnitCategory.Type.Undead);
        public static readonly UnitCategory UNIT_CATEGORY_SUMMONED = ConvertUnitCategory((int)UnitCategory.Type.Summoned);
        public static readonly UnitCategory UNIT_CATEGORY_MECHANICAL = ConvertUnitCategory((int)UnitCategory.Type.Mechanical);
        public static readonly UnitCategory UNIT_CATEGORY_PEON = ConvertUnitCategory((int)UnitCategory.Type.Peon);
        public static readonly UnitCategory UNIT_CATEGORY_SAPPER = ConvertUnitCategory((int)UnitCategory.Type.Sapper);
        public static readonly UnitCategory UNIT_CATEGORY_TOWNHALL = ConvertUnitCategory((int)UnitCategory.Type.Townhall);
        public static readonly UnitCategory UNIT_CATEGORY_ANCIENT = ConvertUnitCategory((int)UnitCategory.Type.Ancient);
        public static readonly UnitCategory UNIT_CATEGORY_NEUTRAL = ConvertUnitCategory((int)UnitCategory.Type.Neutral);
        public static readonly UnitCategory UNIT_CATEGORY_WARD = ConvertUnitCategory((int)UnitCategory.Type.Ward);
        public static readonly UnitCategory UNIT_CATEGORY_STANDON = ConvertUnitCategory((int)UnitCategory.Type.StandOn);
        public static readonly UnitCategory UNIT_CATEGORY_TAUREN = ConvertUnitCategory((int)UnitCategory.Type.Tauren);

        public static UnitCategory ConvertUnitCategory(int i)
        {
            return UnitCategory.GetUnitCategory(i);
        }
    }
}