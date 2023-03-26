// ------------------------------------------------------------------------------
// <copyright file="CreatePlayerUnits.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
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
        protected internal virtual JassFunctionDeclarationSyntax CreatePlayerUnits(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var statements = new List<JassStatementSyntax>();

            for (var i = 0; i < MaxPlayerSlots; i++)
            {
                if (CreateUnitsForPlayerCondition(map, i))
                {
                    statements.Add(SyntaxFactory.CallStatement(nameof(CreateUnitsForPlayer) + i));
                }
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(CreatePlayerUnits)), statements);
        }

        protected internal virtual bool CreatePlayerUnitsCondition(Map map)
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