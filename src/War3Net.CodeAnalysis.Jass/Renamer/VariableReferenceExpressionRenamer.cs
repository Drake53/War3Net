// ------------------------------------------------------------------------------
// <copyright file="VariableReferenceExpressionRenamer.cs" company="Drake53">
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
        private bool TryRenameVariableReferenceExpression(JassVariableReferenceExpressionSyntax variableReferenceExpression, [NotNullWhen(true)] out IExpressionSyntax? renamedVariableReferenceExpression)
        {
            if (TryRenameVariableIdentifierName(variableReferenceExpression.IdentifierName, out var renamedIdentifierName))
            {
                renamedVariableReferenceExpression = new JassVariableReferenceExpressionSyntax(renamedIdentifierName);
                return true;
            }

            renamedVariableReferenceExpression = null;
            return false;
        }
    }
}