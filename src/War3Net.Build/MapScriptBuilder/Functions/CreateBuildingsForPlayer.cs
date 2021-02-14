// ------------------------------------------------------------------------------
// <copyright file="CreateBuildingsForPlayer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;

using War3Net.Build.Extensions;
using War3Net.Build.Widget;
using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected virtual JassFunctionDeclarationSyntax CreateBuildingsForPlayer(Map map, int playerId)
        {
            var functionName = nameof(CreateBuildingsForPlayer) + playerId;

            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapUnits = map.Units;
            if (mapUnits is null)
            {
                throw new ArgumentException($"Function '{functionName}' cannot be generated without {nameof(MapUnits)}.", nameof(map));
            }

            return SyntaxFactory.FunctionDeclaration(
                SyntaxFactory.FunctionDeclarator(functionName),
                CreateUnits(
                    map,
                    mapUnits.Units.IncludeId().Where(pair => CreateBuildingsForPlayerConditionSingleUnit(map, playerId, pair.Obj)),
                    SyntaxFactory.LiteralExpression(playerId)));
        }

        protected virtual bool CreateBuildingsForPlayerCondition(Map map, int playerId)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Units is not null
                && map.Units.Units.Any(unit => CreateBuildingsForPlayerConditionSingleUnit(map, playerId, unit));
        }

        protected virtual bool CreateBuildingsForPlayerConditionSingleUnit(Map map, int playerId, UnitData unitData)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (unitData is null)
            {
                throw new ArgumentNullException(nameof(unitData));
            }

            return unitData.OwnerId == playerId && unitData.IsUnit() && unitData.IsBuilding() && !unitData.IsPlayerStartLocation();
        }
    }
}