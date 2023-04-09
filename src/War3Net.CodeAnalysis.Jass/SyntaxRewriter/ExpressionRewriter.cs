// ------------------------------------------------------------------------------
// <copyright file="ExpressionRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="expression">The <see cref="JassExpressionSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassExpressionSyntax"/>, or the input <paramref name="expression"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="expression"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteExpression(JassExpressionSyntax expression, out JassExpressionSyntax result)
        {
            return expression switch
            {
                JassArrayReferenceExpressionSyntax arrayReferenceExpression => RewriteArrayReferenceExpression(arrayReferenceExpression, out result),
                JassBinaryExpressionSyntax binaryExpression => RewriteBinaryExpression(binaryExpression, out result),
                JassFunctionReferenceExpressionSyntax functionReferenceExpression => RewriteFunctionReferenceExpression(functionReferenceExpression, out result),
                JassIdentifierNameSyntax identifierName => RewriteIdentifierNameAsExpression(identifierName, out result),
                JassInvocationExpressionSyntax invocationExpression => RewriteInvocationExpression(invocationExpression, out result),
                JassLiteralExpressionSyntax literalExpression => RewriteLiteralExpression(literalExpression, out result),
                JassParenthesizedExpressionSyntax parenthesizedExpression => RewriteParenthesizedExpression(parenthesizedExpression, out result),
                JassUnaryExpressionSyntax unaryExpression => RewriteUnaryExpression(unaryExpression, out result),
            };
        }
    }
}