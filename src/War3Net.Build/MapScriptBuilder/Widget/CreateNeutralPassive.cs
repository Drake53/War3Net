// ------------------------------------------------------------------------------
// <copyright file="CreateNeutralPassive.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;

using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.Build.Widget;
using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual JassFunctionDeclarationSyntax CreateNeutralPassive(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapUnits = map.Units;
            if (mapUnits is null)
            {
                throw new ArgumentException($"Function '{nameof(CreateNeutralPassive)}' cannot be generated without {nameof(MapUnits)}.", nameof(map));
            }

            return SyntaxFactory.FunctionDeclaration(
                SyntaxFactory.FunctionDeclarator(nameof(CreateNeutralPassive)),
                CreateUnits(
                    map,
                    mapUnits.Units.IncludeId().Where(pair => CreateNeutralPassiveConditionSingleUnit(map, pair.Obj)),
                    SyntaxFactory.VariableReferenceExpression(GlobalVariableName.PlayerNeutralPassive)));
        }

        protected internal virtual bool CreateNeutralPassiveCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (map.Info is not null && map.Info.FormatVersion < MapInfoFormatVersion.v15)
            {
                return true;
            }

            return map.Units is not null
                && map.Units.Units.Any(unit => CreateNeutralPassiveConditionSingleUnit(map, unit));
        }

        protected internal virtual bool CreateNeutralPassiveConditionSingleUnit(Map map, UnitData unitData)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (unitData is null)
            {
                throw new ArgumentNullException(nameof(unitData));
            }

            var allowBuildings = map.Info is not null && map.Info.FormatVersion < MapInfoFormatVersion.v15;

            var neutralPassiveId = MaxPlayerSlots + 3;
            return unitData.OwnerId == neutralPassiveId && unitData.IsUnit() && (allowBuildings || !unitData.IsPassiveBuilding(map.UnitObjectData));
        }
    }
}