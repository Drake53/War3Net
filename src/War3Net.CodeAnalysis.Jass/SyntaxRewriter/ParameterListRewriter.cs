// ------------------------------------------------------------------------------
// <copyright file="ParameterListRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="parameterList">The <see cref="JassParameterListSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassParameterListOrEmptyParameterListSyntax"/>, or the input <paramref name="parameterList"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="parameterList"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteParameterList(JassParameterListSyntax parameterList, out JassParameterListOrEmptyParameterListSyntax result)
        {
            if (RewriteToken(parameterList.TakesToken, out var takesToken) |
                RewriteSeparatedParameterList(parameterList.ParameterList, out var separatedParameterList))
            {
                result = new JassParameterListSyntax(
                    takesToken,
                    separatedParameterList);

                return true;
            }

            result = parameterList;
            return false;
        }
    }
}