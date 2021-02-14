// ------------------------------------------------------------------------------
// <copyright file="CreatePlayerUnits.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected virtual JassFunctionDeclarationSyntax CreatePlayerUnits(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var statements = new List<IStatementSyntax>();

            for (var i = 0; i < MaxPlayerSlots; i++)
            {
                if (CreateUnitsForPlayerCondition(map, i))
                {
                    statements.Add(SyntaxFactory.CallStatement(nameof(CreateUnitsForPlayer) + i));
                }
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(CreatePlayerUnits)), statements);
        }

        protected virtual bool CreatePlayerUnitsCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return true;
        }
    }
}