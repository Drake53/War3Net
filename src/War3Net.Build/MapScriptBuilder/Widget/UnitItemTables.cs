// ------------------------------------------------------------------------------
// <copyright file="UnitItemTables.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Extensions;
using War3Net.Build.Widget;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual IEnumerable<JassFunctionDeclarationSyntax> UnitItemTables(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapUnits = map.Units;
            if (mapUnits is null)
            {
                throw new ArgumentException($"Function '{nameof(UnitItemTables)}' cannot be generated without {nameof(MapUnits)}.", nameof(map));
            }

            for (var i = 0; i < mapUnits.Units.Count; i++)
            {
                var unit = mapUnits.Units[i];
                if (UnitItemTablesConditionSingleUnit(map, unit))
                {
                    yield return ItemTableDropItems(map, unit, i);
                }
            }
        }

        protected internal virtual bool UnitItemTablesCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Units is not null
                && map.Units.Units.Any(unit => UnitItemTablesConditionSingleUnit(map, unit));
        }

        protected internal virtual bool UnitItemTablesConditionSingleUnit(Map map, UnitData unitData)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (unitData is null)
            {
                throw new ArgumentNullException(nameof(unitData));
            }

            return unitData.IsUnit() && unitData.ItemTableSets.Any(itemTableSet => itemTableSet.Items.Any());
        }
    }
}