// ------------------------------------------------------------------------------
// <copyright file="CreatePlayerBuildings.cs" company="Drake53">
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
        protected internal virtual JassFunctionDeclarationSyntax CreatePlayerBuildings(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var statements = new List<JassStatementSyntax>();

            for (var i = 0; i < MaxPlayerSlots; i++)
            {
                if (CreateBuildingsForPlayerCondition(map, i))
                {
                    statements.Add(SyntaxFactory.CallStatement(nameof(CreateBuildingsForPlayer) + i));
                }
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(CreatePlayerBuildings)), statements);
        }

        protected internal virtual bool CreatePlayerBuildingsCondition(Map map)
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
                && map.Units.Units.Any(unit => CreatePlayerBuildingsConditionSingleUnit(map, unit));
        }

        protected internal virtual bool CreatePlayerBuildingsConditionSingleUnit(Map map, UnitData unitData)
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