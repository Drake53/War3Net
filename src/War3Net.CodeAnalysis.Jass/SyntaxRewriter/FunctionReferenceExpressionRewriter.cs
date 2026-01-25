// ------------------------------------------------------------------------------
// <copyright file="FunctionReferenceExpressionRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="functionReferenceExpression">The <see cref="JassFunctionReferenceExpressionSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassExpressionSyntax"/>, or the input <paramref name="functionReferenceExpression"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="functionReferenceExpression"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteFunctionReferenceExpression(JassFunctionReferenceExpressionSyntax functionReferenceExpression, out JassExpressionSyntax result)
        {
            if (RewriteToken(functionReferenceExpression.FunctionToken, out var functionToken) |
                RewriteIdentifierName(functionReferenceExpression.IdentifierName, out var identifierName))
            {
                result = new JassFunctionReferenceExpressionSyntax(
                    functionToken,
                    identifierName);

                return true;
            }

            result = functionReferenceExpression;
            return false;
        }
    }
}