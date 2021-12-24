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
        private bool TryRenameArrayReferenceExpression(JassArrayReferenceExpressionSyntax arrayReferenceExpression, [NotNullWhen(true)] out IExpressionSyntax? renamedArrayReferenceExpression)
        {
            if (TryRenameVariableIdentifierName(arrayReferenceExpression.IdentifierName, out var renamedIdentifierName) |
                TryRenameExpression(arrayReferenceExpression.Indexer, out var renamedIndexer))
            {
                renamedArrayReferenceExpression = new JassArrayReferenceExpressionSyntax(
                    renamedIdentifierName ?? arrayReferenceExpression.IdentifierName,
                    renamedIndexer ?? arrayReferenceExpression.Indexer);

                return true;
            }

            renamedArrayReferenceExpression = null;
            return false;
        }
    }
}