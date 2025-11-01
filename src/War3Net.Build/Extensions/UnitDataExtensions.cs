// ------------------------------------------------------------------------------
// <copyright file="UnitDataExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using War3Net.Build.Object;
using War3Net.Build.Resources;
using War3Net.Build.Widget;
using War3Net.Common.Extensions;
using War3Net.IO.Slk;

namespace War3Net.Build.Extensions
{
    public static class UnitDataExtensions
    {
        private static Lazy<HashSet<int>> _buildingTypeIds = new(GetBuildingTypeIds);
        private static Lazy<Dictionary<int, int>> _defaultPlayerColorIds = new(GetDefaultPlayerColorIds);

        public static string GetVariableName(this UnitData unitData)
        {
            return $"{(unitData.IsItem() ? "gg_item_" : "gg_unit_")}{unitData.TypeId.ToRawcode()}_{unitData.CreationNumber:D4}";
        }

        public static string GetDropItemsFunctionName(this UnitData unitData, int id)
        {
            return unitData.MapItemTableId == -1
                ? $"Unit{id:D6}_DropItems"
                : $"ItemTable{unitData.MapItemTableId:D6}_DropItems";
        }

        public static bool IsBuilding(this UnitData unitData, UnitObjectData? unitObjectData)
        {
            if (unitData.IsRandomBuilding())
            {
                return true;
            }

            if (unitObjectData is not null)
            {
                var unit = unitObjectData.BaseUnits.FirstOrDefault(unit => unit.OldId == unitData.TypeId)
                         ?? unitObjectData.NewUnits.FirstOrDefault(unit => unit.NewId == unitData.TypeId);

                if (unit is not null)
                {
                    var isBuildingModification = unit.Modifications.FirstOrDefault(modification => modification.Id == "ubdg".FromRawcode());
                    if (isBuildingModification is not null)
                    {
                        return isBuildingModification.ValueAsInt == 1;
                    }
                    else
                    {
                        return _buildingTypeIds.Value.Contains(unit.OldId);
                    }
                }
            }

            return _buildingTypeIds.Value.Contains(unitData.TypeId);
        }

        public static bool IsPassiveBuilding(this UnitData unitData, UnitObjectData? unitObjectData)
        {
            return unitData.IsBuilding(unitObjectData);
        }

        public static bool TryGetDefaultPlayerColorId(this UnitData unitData, out int playerColorId)
        {
            if (unitData.IsRandomBuilding())
            {
                playerColorId = 0;
                return true;
            }

            return _defaultPlayerColorIds.Value.TryGetValue(unitData.TypeId, out playerColorId);
        }

        private static HashSet<int> GetBuildingTypeIds()
        {
            var unitUI = new SylkParser().Parse(new MemoryStream(War3Resources.UnitUI));

            var typeIdColumn = unitUI["unitUIID"].Single();
            var buildingShadowColumn = unitUI["buildingShadow"].Single();

            var typeIds = new HashSet<int>();

            // Zone indicator does not have a building shadow.
            typeIds.Add("nzin".FromRawcode());

            foreach (var row in unitUI.Skip(1))
            {
                if (row[buildingShadowColumn] is string s && !string.IsNullOrEmpty(s) && s != "_")
                {
                    typeIds.Add(((string)row[typeIdColumn]).FromRawcode());
                }
            }

            return typeIds;
        }

        private static Dictionary<int, int> GetDefaultPlayerColorIds()
        {
            var unitUI = new SylkParser().Parse(new MemoryStream(War3Resources.UnitUI));

            var typeIdColumn = unitUI["unitUIID"].Single();
            var playerColorIdColumn = unitUI["teamColor"].Single();

            var playerColorIds = new Dictionary<int, int>();

            foreach (var row in unitUI.Skip(1))
            {
                if (row[playerColorIdColumn] is int playerColorId)
                {
                    playerColorIds.Add(((string)row[typeIdColumn]).FromRawcode(), playerColorId);
                }
            }

            return playerColorIds;
        }
    }
}