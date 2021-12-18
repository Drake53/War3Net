// ------------------------------------------------------------------------------
// <copyright file="CreateNeutralUnits.cs" company="Drake53">
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
        protected internal virtual JassFunctionDeclarationSyntax CreateNeutralUnits(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var statements = new List<IStatementSyntax>();

            if (CreateNeutralHostileCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(nameof(CreateNeutralHostile)));
            }

            if (CreateNeutralPassiveCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(nameof(CreateNeutralPassive)));
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(CreateNeutralUnits)), statements);
        }

        protected internal virtual bool CreateNeutralUnitsCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return false;
        }
    }
}