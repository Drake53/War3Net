// ------------------------------------------------------------------------------
// <copyright file="CreateNeutralHostileBuildings.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;
using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.Build.Widget;
using War3Net.CodeAnalysis;
using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateCreateNeutralHostileBuildings(Map map, IndentedTextWriter writer)
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
                throw new ArgumentException($"Function '{GeneratedFunctionName.CreateNeutralHostileBuildings}' cannot be generated without {nameof(MapUnits)}.", nameof(map));
            }

            writer.WriteFunction(GeneratedFunctionName.CreateNeutralHostileBuildings);

            GenerateCreateUnits(
                map,
                mapUnits.Units.IncludeId().Where(pair => ShouldGenerateCreateNeutralHostileBuildingsForUnit(map, pair.Obj)),
                GlobalVariableName.PlayerNeutralHostile,
                writer);

            writer.EndFunction();
        }

        protected internal virtual bool ShouldGenerateCreateNeutralHostileBuildings(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (map.Info is not null && map.Info.FormatVersion < MapInfoFormatVersion.v15)
            {
                return false;
            }

            return map.Units is not null
                && map.Units.Units.Any(unit => ShouldGenerateCreateNeutralHostileBuildingsForUnit(map, unit));
        }

        protected internal virtual bool ShouldGenerateCreateNeutralHostileBuildingsForUnit(Map map, UnitData unitData)
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
            return unitData.OwnerId == neutralHostileId && unitData.IsUnit() && unitData.IsBuilding(map.UnitObjectData);
        }
    }
}