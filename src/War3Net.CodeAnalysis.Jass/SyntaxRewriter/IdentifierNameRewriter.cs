// ------------------------------------------------------------------------------
// <copyright file="IdentifierNameRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="identifierName">The <see cref="JassIdentifierNameSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassIdentifierNameSyntax"/>, or the input <paramref name="identifierName"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="identifierName"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteIdentifierName(JassIdentifierNameSyntax identifierName, out JassIdentifierNameSyntax result)
        {
            if (RewriteToken(identifierName.Token, out var token))
            {
                result = new JassIdentifierNameSyntax(token);
                return true;
            }

            result = identifierName;
            return false;
        }

        /// <param name="identifierName">The <see cref="JassIdentifierNameSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassExpressionSyntax"/>, or the input <paramref name="identifierName"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="identifierName"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteIdentifierNameAsExpression(JassIdentifierNameSyntax identifierName, out JassExpressionSyntax result)
        {
            if (RewriteToken(identifierName.Token, out var token))
            {
                result = new JassIdentifierNameSyntax(token);
                return true;
            }

            result = identifierName;
            return false;
        }
    }
}