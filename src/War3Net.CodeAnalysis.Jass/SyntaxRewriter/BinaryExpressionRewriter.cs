// ------------------------------------------------------------------------------
// <copyright file="BinaryExpressionRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="binaryExpression">The <see cref="JassBinaryExpressionSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassExpressionSyntax"/>, or the input <paramref name="binaryExpression"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="binaryExpression"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteBinaryExpression(JassBinaryExpressionSyntax binaryExpression, out JassExpressionSyntax result)
        {
            if (RewriteExpression(binaryExpression.Left, out var left) |
                RewriteToken(binaryExpression.OperatorToken, out var operatorToken) |
                RewriteExpression(binaryExpression.Right, out var right))
            {
                result = new JassBinaryExpressionSyntax(
                    left,
                    operatorToken,
                    right);

                return true;
            }

            result = binaryExpression;
            return false;
        }
    }
}