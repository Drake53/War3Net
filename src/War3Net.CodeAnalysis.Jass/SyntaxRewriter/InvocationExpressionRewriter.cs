// ------------------------------------------------------------------------------
// <copyright file="InvocationExpressionRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="invocationExpression">The <see cref="JassInvocationExpressionSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassExpressionSyntax"/>, or the input <paramref name="invocationExpression"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="invocationExpression"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteInvocationExpression(JassInvocationExpressionSyntax invocationExpression, out JassExpressionSyntax result)
        {
            if (RewriteIdentifierName(invocationExpression.IdentifierName, out var identifierName) |
                RewriteArgumentList(invocationExpression.ArgumentList, out var argumentList))
            {
                result = new JassInvocationExpressionSyntax(
                    identifierName,
                    argumentList);

                return true;
            }

            result = invocationExpression;
            return false;
        }
    }
}