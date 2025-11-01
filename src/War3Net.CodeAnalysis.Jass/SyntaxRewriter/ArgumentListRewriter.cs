// ------------------------------------------------------------------------------
// <copyright file="ArgumentListRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="argumentList">The <see cref="JassArgumentListSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassArgumentListSyntax"/>, or the input <paramref name="argumentList"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="argumentList"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteArgumentList(JassArgumentListSyntax argumentList, out JassArgumentListSyntax result)
        {
            if (RewriteToken(argumentList.OpenParenToken, out var openParenToken) |
                RewriteSeparatedArgumentList(argumentList.ArgumentList, out var separatedArgumentList) |
                RewriteToken(argumentList.CloseParenToken, out var closeParenToken))
            {
                result = new JassArgumentListSyntax(
                    openParenToken,
                    separatedArgumentList,
                    closeParenToken);

                return true;
            }

            result = argumentList;
            return false;
        }
    }
}