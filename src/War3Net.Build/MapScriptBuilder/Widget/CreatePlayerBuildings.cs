// ------------------------------------------------------------------------------
// <copyright file="CreatePlayerBuildings.cs" company="Drake53">
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
        protected internal virtual void GenerateCreatePlayerBuildings(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteFunction(GeneratedFunctionName.CreatePlayerBuildings);

            for (var i = 0; i < MaxPlayerSlots; i++)
            {
                if (ShouldGenerateCreateBuildingsForPlayer(map, i))
                {
                    writer.WriteCall(GeneratedFunctionName.CreateBuildingsForPlayer(i));
                }
            }

            writer.EndFunction();
        }

        protected internal virtual bool ShouldGenerateCreatePlayerBuildings(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (map.Info is not null && map.Info.FormatVersion >= MapInfoFormatVersion.v15 && map.Info.FormatVersion < MapInfoFormatVersion.v28)
            {
                return true;
            }

            return map.Units is not null
                && map.Units.Units.Any(unit => ShouldGenerateCreatePlayerBuildingsForUnit(map, unit));
        }

        protected internal virtual bool ShouldGenerateCreatePlayerBuildingsForUnit(Map map, UnitData unitData)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (unitData is null)
            {
                throw new ArgumentNullException(nameof(unitData));
            }

            return unitData.OwnerId < MaxPlayerSlots && unitData.IsUnit() && unitData.IsBuilding(map.UnitObjectData);
        }
    }
}