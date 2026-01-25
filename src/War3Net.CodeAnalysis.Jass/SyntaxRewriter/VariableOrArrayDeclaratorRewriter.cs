// ------------------------------------------------------------------------------
// <copyright file="VariableOrArrayDeclaratorRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="variableOrArrayDeclarator">The <see cref="JassVariableOrArrayDeclaratorSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassVariableOrArrayDeclaratorSyntax"/>, or the input <paramref name="variableOrArrayDeclarator"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="variableOrArrayDeclarator"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteVariableOrArrayDeclarator(JassVariableOrArrayDeclaratorSyntax variableOrArrayDeclarator, out JassVariableOrArrayDeclaratorSyntax result)
        {
            return variableOrArrayDeclarator switch
            {
                JassArrayDeclaratorSyntax arrayDeclarator => RewriteArrayDeclarator(arrayDeclarator, out result),
                JassVariableDeclaratorSyntax variableDeclarator => RewriteVariableDeclarator(variableDeclarator, out result),
            };
        }
    }
}