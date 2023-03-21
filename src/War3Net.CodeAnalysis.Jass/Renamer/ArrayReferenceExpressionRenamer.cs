// ------------------------------------------------------------------------------
// <copyright file="ArrayReferenceExpressionRenamer.cs" company="Drake53">
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
        private bool TryRenameArrayReferenceExpression(JassArrayReferenceExpressionSyntax arrayReferenceExpression, [NotNullWhen(true)] out JassExpressionSyntax? renamedArrayReferenceExpression)
        {
            if (TryRenameVariableIdentifierName(arrayReferenceExpression.IdentifierName, out var renamedIdentifierName) |
                TryRenameElementAccessClause(arrayReferenceExpression.ElementAccessClause, out var renamedElementAccessClause))
            {
                renamedArrayReferenceExpression = new JassArrayReferenceExpressionSyntax(
                    renamedIdentifierName ?? arrayReferenceExpression.IdentifierName,
                    renamedElementAccessClause ?? arrayReferenceExpression.ElementAccessClause);

                return true;
            }

            renamedArrayReferenceExpression = null;
            return false;
        }
    }
}