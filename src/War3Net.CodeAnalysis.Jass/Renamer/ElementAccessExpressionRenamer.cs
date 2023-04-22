// ------------------------------------------------------------------------------
// <copyright file="ElementAccessExpressionRenamer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameElementAccessExpression(JassElementAccessExpressionSyntax elementAccessExpression, [NotNullWhen(true)] out JassExpressionSyntax? renamedElementAccessExpression)
        {
            if (TryRenameVariableIdentifierName(elementAccessExpression.IdentifierName, out var renamedIdentifierName) |
                TryRenameElementAccessClause(elementAccessExpression.ElementAccessClause, out var renamedElementAccessClause))
            {
                renamedElementAccessExpression = new JassElementAccessExpressionSyntax(
                    renamedIdentifierName ?? elementAccessExpression.IdentifierName,
                    renamedElementAccessClause ?? elementAccessExpression.ElementAccessClause);

                return true;
            }

            renamedElementAccessExpression = null;
            return false;
        }
    }
}