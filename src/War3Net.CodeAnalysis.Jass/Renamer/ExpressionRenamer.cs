// ------------------------------------------------------------------------------
// <copyright file="ExpressionRenamer.cs" company="Drake53">
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
        private bool TryRenameExpression(JassExpressionSyntax? expression, [NotNullWhen(true)] out JassExpressionSyntax? renamedExpression)
        {
            return expression switch
            {
                JassFunctionReferenceExpressionSyntax functionReferenceExpression => TryRenameFunctionReferenceExpression(functionReferenceExpression, out renamedExpression),
                JassInvocationExpressionSyntax invocationExpression => TryRenameInvocationExpression(invocationExpression, out renamedExpression),
                JassElementAccessExpressionSyntax elementAccessExpression => TryRenameElementAccessExpression(elementAccessExpression, out renamedExpression),
                JassIdentifierNameSyntax identifierName => TryRenameIdentifierName(identifierName, out renamedExpression),
                JassParenthesizedExpressionSyntax parenthesizedExpression => TryRenameParenthesizedExpression(parenthesizedExpression, out renamedExpression),
                JassUnaryExpressionSyntax unaryExpression => TryRenameUnaryExpression(unaryExpression, out renamedExpression),
                JassBinaryExpressionSyntax binaryExpression => TryRenameBinaryExpression(binaryExpression, out renamedExpression),

                _ => TryRenameDummy(expression, out renamedExpression),
            };
        }
    }
}