// ------------------------------------------------------------------------------
// <copyright file="CreateAllUnits.cs" company="Drake53">
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
        protected internal virtual void GenerateCreateAllUnits(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteFunction(GeneratedFunctionName.CreateAllUnits);

            if (ShouldGenerateCreateNeutralPassiveBuildings(map))
            {
                writer.WriteCall(GeneratedFunctionName.CreateNeutralPassiveBuildings);
            }

            if (ShouldGenerateCreatePlayerBuildings(map))
            {
                writer.WriteCall(GeneratedFunctionName.CreatePlayerBuildings);
            }

            if (ShouldGenerateCreateNeutralHostile(map))
            {
                writer.WriteCall(GeneratedFunctionName.CreateNeutralHostile);
            }

            if (ShouldGenerateCreateNeutralPassive(map))
            {
                writer.WriteCall(GeneratedFunctionName.CreateNeutralPassive);
            }

            if (ShouldGenerateCreatePlayerUnits(map))
            {
                writer.WriteCall(GeneratedFunctionName.CreatePlayerUnits);
            }

            writer.EndFunction();
        }

        protected internal virtual bool ShouldGenerateCreateAllUnits(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (map.Info is null || map.Info.FormatVersion >= MapInfoFormatVersion.v28)
            {
                return map.Units is not null
                    && map.Units.Units.Any(ShouldGenerateCreateAllUnitsForUnit);
            }

            return map.Info.FormatVersion >= MapInfoFormatVersion.v15;
        }

        protected internal virtual bool ShouldGenerateCreateAllUnitsForUnit(UnitData unitData)
        {
            if (unitData is null)
            {
                throw new ArgumentNullException(nameof(unitData));
            }

            return unitData.IsUnit()
                && !unitData.IsPlayerStartLocation();
        }
    }
}