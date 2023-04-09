// ------------------------------------------------------------------------------
// <copyright file="ParenthesizedExpressionRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="parenthesizedExpression">The <see cref="JassParenthesizedExpressionSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassExpressionSyntax"/>, or the input <paramref name="parenthesizedExpression"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="parenthesizedExpression"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteParenthesizedExpression(JassParenthesizedExpressionSyntax parenthesizedExpression, out JassExpressionSyntax result)
        {
            if (RewriteToken(parenthesizedExpression.OpenParenToken, out var openParenToken) |
                RewriteExpression(parenthesizedExpression.Expression, out var expression) |
                RewriteToken(parenthesizedExpression.CloseParenToken, out var closeParenToken))
            {
                result = new JassParenthesizedExpressionSyntax(
                    openParenToken,
                    expression,
                    closeParenToken);

                return true;
            }

            result = parenthesizedExpression;
            return false;
        }
    }
}