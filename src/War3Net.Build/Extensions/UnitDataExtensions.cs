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
        public static string GetVariableName(this UnitData unitData)
        {
            return $"gg_unit_{unitData.TypeId.ToRawcode()}_{unitData.CreationNumber:D4}";
        }

        public static string GetDropItemsFunctionName(this UnitData unitData, int id)
        {
            return unitData.MapItemTableId == -1
                ? $"Unit{id:D6}_DropItems"
                : $"ItemTable{unitData.MapItemTableId:D6}_DropItems";
        }

        public static bool IsBuilding(this UnitData unitData)
        {
            return false;
        }

        public static bool IsPassiveBuilding(this UnitData unitData)
        {
            return false;
        }
    }
}