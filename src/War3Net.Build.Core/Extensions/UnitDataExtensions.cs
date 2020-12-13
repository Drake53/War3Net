// ------------------------------------------------------------------------------
// <copyright file="UnitDataExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Build.Widget;
using War3Net.Common.Extensions;

namespace War3Net.Build.Extensions
{
    public static class UnitDataExtensions
    {
        private static readonly int PlayerStartLocationTypeId = "sloc".FromRawcode();
        private static readonly int GoldMineTypeId = "ngol".FromRawcode();
        private static readonly int RandomUnitTypeId = "uDNR".FromRawcode();
        private static readonly int RandomBuildingTypeId = "bDNR".FromRawcode();
        private static readonly int RandomItemTypeId = "iDNR".FromRawcode();

        public static bool IsUnit(this UnitData unitData) => unitData.HeroLevel > 0;

        public static bool IsItem(this UnitData unitData) => unitData.HeroLevel == 0;

        public static bool IsPlayerStartLocation(this UnitData unitData) => unitData.TypeId == PlayerStartLocationTypeId;

        public static bool IsGoldMine(this UnitData unitData) => unitData.TypeId == GoldMineTypeId;

        public static bool IsRandomUnit(this UnitData unitData) => unitData.TypeId == RandomUnitTypeId;

        public static bool IsRandomBuilding(this UnitData unitData) => unitData.TypeId == RandomBuildingTypeId;

        public static bool IsRandomItem(this UnitData unitData) => unitData.TypeId == RandomItemTypeId;
    }
}