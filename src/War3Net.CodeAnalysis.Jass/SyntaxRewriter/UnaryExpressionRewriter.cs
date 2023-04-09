// ------------------------------------------------------------------------------
// <copyright file="UnaryExpressionRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="unaryExpression">The <see cref="JassUnaryExpressionSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassExpressionSyntax"/>, or the input <paramref name="unaryExpression"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="unaryExpression"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteUnaryExpression(JassUnaryExpressionSyntax unaryExpression, out JassExpressionSyntax result)
        {
            if (RewriteToken(unaryExpression.OperatorToken, out var operatorToken) |
                RewriteExpression(unaryExpression.Expression, out var expression))
            {
                result = new JassUnaryExpressionSyntax(
                    operatorToken,
                    expression);

                return true;
            }

            result = unaryExpression;
            return false;
        }
    }
}