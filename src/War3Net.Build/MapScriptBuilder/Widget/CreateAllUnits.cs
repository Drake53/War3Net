// ------------------------------------------------------------------------------
// <copyright file="CreateAllUnits.cs" company="Drake53">
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
        protected internal virtual JassFunctionDeclarationSyntax CreateAllUnits(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var statements = new List<IStatementSyntax>();

            if (CreateNeutralPassiveBuildingsCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(nameof(CreateNeutralPassiveBuildings)));
            }

            if (CreatePlayerBuildingsCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(nameof(CreatePlayerBuildings)));
            }

            if (CreateNeutralHostileCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(nameof(CreateNeutralHostile)));
            }

            if (CreateNeutralPassiveCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(nameof(CreateNeutralPassive)));
            }

            if (CreatePlayerUnitsCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(nameof(CreatePlayerUnits)));
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(CreateAllUnits)), statements);
        }

        protected internal virtual bool CreateAllUnitsCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (map.Info is null || map.Info.FormatVersion >= MapInfoFormatVersion.v28)
            {
                return map.Units is not null
                    && map.Units.Units.Any(unit => CreateAllUnitsConditionSingleUnit(map, unit));
            }

            return map.Info.FormatVersion >= MapInfoFormatVersion.v15;
        }

        protected internal virtual bool CreateAllUnitsConditionSingleUnit(Map map, UnitData unitData)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (unitData is null)
            {
                throw new ArgumentNullException(nameof(unitData));
            }

            return unitData.IsUnit() && !unitData.IsPlayerStartLocation();
        }
    }
}