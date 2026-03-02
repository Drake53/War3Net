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
        protected internal virtual void GenerateCreateNeutralPassiveBuildings(Map map, IndentedTextWriter writer)
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
                throw new ArgumentException($"Function '{GeneratedFunctionName.CreateNeutralPassiveBuildings}' cannot be generated without {nameof(MapUnits)}.", nameof(map));
            }

            writer.WriteFunction(GeneratedFunctionName.CreateNeutralPassiveBuildings);

            GenerateCreateUnits(
                map,
                mapUnits.Units.IncludeId().Where(pair => ShouldGenerateCreateNeutralPassiveBuildingsForUnit(map, pair.Obj)),
                GlobalVariableName.PlayerNeutralPassive,
                writer);

            writer.EndFunction();
        }

        protected internal virtual bool ShouldGenerateCreateNeutralPassiveBuildings(Map map)
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
                && map.Units.Units.Any(unit => ShouldGenerateCreateNeutralPassiveBuildingsForUnit(map, unit));
        }

        protected internal virtual bool ShouldGenerateCreateNeutralPassiveBuildingsForUnit(Map map, UnitData unitData)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (unitData is null)
            {
                throw new ArgumentNullException(nameof(unitData));
            }

            var neutralPassiveId = MaxPlayerSlots + 3;
            return unitData.OwnerId == neutralPassiveId && unitData.IsUnit() && unitData.IsPassiveBuilding(map.UnitObjectData);
        }
    }
}