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
        protected internal virtual void GenerateCreatePlayerUnits(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteFunction(GeneratedFunctionName.CreatePlayerUnits);

            for (var i = 0; i < MaxPlayerSlots; i++)
            {
                if (ShouldGenerateCreateUnitsForPlayer(map, i))
                {
                    writer.WriteCall(GeneratedFunctionName.CreateUnitsForPlayer(i));
                }
            }

            writer.EndFunction();
        }

        protected internal virtual bool ShouldGenerateCreatePlayerUnits(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (map.Info is not null && map.Info.FormatVersion >= MapInfoFormatVersion.v28)
            {
                return map.Units is not null
                    && map.Units.Units.Any(unit => CreatePlayerUnitConditionSingleUnit(map, unit));
            }

            return true;
        }

        protected internal virtual bool CreatePlayerUnitConditionSingleUnit(Map map, UnitData unitData)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (unitData is null)
            {
                throw new ArgumentNullException(nameof(unitData));
            }

            return unitData.OwnerId < MaxPlayerSlots && unitData.IsUnit() && !unitData.IsPlayerStartLocation() && !unitData.IsBuilding(map.UnitObjectData);
        }
    }
}