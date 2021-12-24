// ------------------------------------------------------------------------------
// <copyright file="InvocationExpressionRenamer.cs" company="Drake53">
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
        private bool TryRenameInvocationExpression(JassInvocationExpressionSyntax invocationExpression, [NotNullWhen(true)] out IExpressionSyntax? renamedInvocationExpression)
        {
            if (TryRenameFunctionIdentifierName(invocationExpression.IdentifierName, out var renamedIdentifierName) |
                TryRenameArgumentList(invocationExpression.Arguments, out var renamedArguments))
            {
                renamedInvocationExpression = new JassInvocationExpressionSyntax(
                    renamedIdentifierName ?? invocationExpression.IdentifierName,
                    renamedArguments ?? invocationExpression.Arguments);

                return true;
            }

            renamedInvocationExpression = null;
            return false;
        }
    }
}