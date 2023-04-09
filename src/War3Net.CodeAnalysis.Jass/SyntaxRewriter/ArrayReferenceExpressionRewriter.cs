// ------------------------------------------------------------------------------
// <copyright file="ArrayReferenceExpressionRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="arrayReferenceExpression">The <see cref="JassArrayReferenceExpressionSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassExpressionSyntax"/>, or the input <paramref name="arrayReferenceExpression"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="arrayReferenceExpression"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteArrayReferenceExpression(JassArrayReferenceExpressionSyntax arrayReferenceExpression, out JassExpressionSyntax result)
        {
            if (RewriteIdentifierName(arrayReferenceExpression.IdentifierName, out var identifierName) |
                RewriteElementAccessClause(arrayReferenceExpression.ElementAccessClause, out var elementAccessClause))
            {
                result = new JassArrayReferenceExpressionSyntax(
                    identifierName,
                    elementAccessClause);

                return true;
            }

            result = arrayReferenceExpression;
            return false;
        }
    }
}