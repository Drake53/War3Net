// ------------------------------------------------------------------------------
// <copyright file="EmptyParameterListRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="emptyParameterList">The <see cref="JassEmptyParameterListSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassParameterListOrEmptyParameterListSyntax"/>, or the input <paramref name="emptyParameterList"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="emptyParameterList"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteEmptyParameterList(JassEmptyParameterListSyntax emptyParameterList, out JassParameterListOrEmptyParameterListSyntax result)
        {
            if (RewriteToken(emptyParameterList.TakesToken, out var takesToken) |
                RewriteToken(emptyParameterList.NothingToken, out var nothingToken))
            {
                result = new JassEmptyParameterListSyntax(
                    takesToken,
                    nothingToken);

                return true;
            }

            result = emptyParameterList;
            return false;
        }
    }
}