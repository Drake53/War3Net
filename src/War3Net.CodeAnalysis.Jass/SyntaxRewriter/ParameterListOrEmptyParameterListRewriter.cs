// ------------------------------------------------------------------------------
// <copyright file="ParameterListOrEmptyParameterListRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="parameterListOrEmptyParameterList">The <see cref="JassParameterListOrEmptyParameterListSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassParameterListOrEmptyParameterListSyntax"/>, or the input <paramref name="parameterListOrEmptyParameterList"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="parameterListOrEmptyParameterList"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteParameterListOrEmptyParameterList(JassParameterListOrEmptyParameterListSyntax parameterListOrEmptyParameterList, out JassParameterListOrEmptyParameterListSyntax result)
        {
            return parameterListOrEmptyParameterList switch
            {
                JassEmptyParameterListSyntax emptyParameterList => RewriteEmptyParameterList(emptyParameterList, out result),
                JassParameterListSyntax parameterList => RewriteParameterList(parameterList, out result),
            };
        }
    }
}