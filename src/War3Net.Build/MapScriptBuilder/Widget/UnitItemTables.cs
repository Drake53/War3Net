using System;
using System.Linq;
using War3Net.Build.Extensions;
using War3Net.Build.Widget;
using War3Net.CodeAnalysis;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateUnitItemTables(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var mapUnits = map.Units;
            if (mapUnits is null)
            {
                throw new ArgumentException($"DropItems functions cannot be generated without {nameof(MapUnits)}.", nameof(map));
            }

            for (var i = 0; i < mapUnits.Units.Count; i++)
            {
                var unit = mapUnits.Units[i];
                if (ShouldGenerateUnitItemTablesForUnit(map, unit))
                {
                    GenerateItemTableDropItems(map, unit, i, writer);
                }
            }

            writer.WriteLine();
        }

        protected internal virtual bool ShouldGenerateUnitItemTables(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Units is not null
                && map.Units.Units.Any(unit => ShouldGenerateUnitItemTablesForUnit(map, unit));
        }

        protected internal virtual bool ShouldGenerateUnitItemTablesForUnit(Map map, UnitData unitData)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (unitData is null)
            {
                throw new ArgumentNullException(nameof(unitData));
            }

            return unitData.IsUnit() && unitData.HasItemTableSets();
        }
    }
}