// ------------------------------------------------------------------------------
// <copyright file="ElementAccessExpressionRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="elementAccessExpression">The <see cref="JassElementAccessExpressionSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassExpressionSyntax"/>, or the input <paramref name="elementAccessExpression"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="elementAccessExpression"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteElementAccessExpression(JassElementAccessExpressionSyntax elementAccessExpression, out JassExpressionSyntax result)
        {
            if (RewriteIdentifierName(elementAccessExpression.IdentifierName, out var identifierName) |
                RewriteElementAccessClause(elementAccessExpression.ElementAccessClause, out var elementAccessClause))
            {
                result = new JassElementAccessExpressionSyntax(
                    identifierName,
                    elementAccessClause);

                return true;
            }

            result = elementAccessExpression;
            return false;
        }
    }
}