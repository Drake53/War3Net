// ------------------------------------------------------------------------------
// <copyright file="LiteralExpressionRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="literalExpression">The <see cref="JassLiteralExpressionSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassExpressionSyntax"/>, or the input <paramref name="literalExpression"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="literalExpression"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteLiteralExpression(JassLiteralExpressionSyntax literalExpression, out JassExpressionSyntax result)
        {
            if (RewriteToken(literalExpression.Token, out var token))
            {
                result = new JassLiteralExpressionSyntax(token);
                return true;
            }

            result = literalExpression;
            return false;
        }
    }
}