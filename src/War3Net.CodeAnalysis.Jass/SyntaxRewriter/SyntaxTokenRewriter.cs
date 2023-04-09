// ------------------------------------------------------------------------------
// <copyright file="SyntaxTokenRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="token">The <see cref="JassSyntaxToken"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassSyntaxToken"/>, or the input <paramref name="token"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="token"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteToken(JassSyntaxToken token, out JassSyntaxToken result)
        {
            if (RewriteLeadingTrivia(token.LeadingTrivia, out var leadingTrivia) |
                RewriteTrailingTrivia(token.TrailingTrivia, out var trailingTrivia))
            {
                result = new JassSyntaxToken(
                    leadingTrivia,
                    token.SyntaxKind,
                    token.Text,
                    trailingTrivia);

                return true;
            }

            result = token;
            return false;
        }
    }
}