// ------------------------------------------------------------------------------
// <copyright file="CreateNeutralHostile.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;
using War3Net.Build.Extensions;
using War3Net.Build.Widget;
using War3Net.CodeAnalysis;
using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateCreateNeutralHostile(Map map, IndentedTextWriter writer)
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
                throw new ArgumentException($"Function '{GeneratedFunctionName.CreateNeutralHostile}' cannot be generated without {nameof(MapUnits)}.", nameof(map));
            }

            writer.WriteFunction(GeneratedFunctionName.CreateNeutralHostile);

            GenerateCreateUnits(
                map,
                mapUnits.Units.IncludeId().Where(pair => ShouldGenerateCreateNeutralHostileForUnit(map, pair.Obj)),
                GlobalVariableName.PlayerNeutralHostile,
                writer);

            writer.EndFunction();
        }

        protected internal virtual bool ShouldGenerateCreateNeutralHostile(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Units is not null
                && map.Units.Units.Any(unit => ShouldGenerateCreateNeutralHostileForUnit(map, unit));
        }

        protected internal virtual bool ShouldGenerateCreateNeutralHostileForUnit(Map map, UnitData unitData)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (unitData is null)
            {
                throw new ArgumentNullException(nameof(unitData));
            }

            var neutralHostileId = MaxPlayerSlots;
            return unitData.OwnerId == neutralHostileId && unitData.IsUnit();
        }
    }
}