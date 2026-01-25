// ------------------------------------------------------------------------------
// <copyright file="IfClauseDeclaratorRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="ifClauseDeclarator">The <see cref="JassIfClauseDeclaratorSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassIfClauseDeclaratorSyntax"/>, or the input <paramref name="ifClauseDeclarator"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="ifClauseDeclarator"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteIfClauseDeclarator(JassIfClauseDeclaratorSyntax ifClauseDeclarator, out JassIfClauseDeclaratorSyntax result)
        {
            if (RewriteToken(ifClauseDeclarator.IfToken, out var ifToken) |
                RewriteExpression(ifClauseDeclarator.Condition, out var condition) |
                RewriteToken(ifClauseDeclarator.ThenToken, out var thenToken))
            {
                result = new JassIfClauseDeclaratorSyntax(
                    ifToken,
                    condition,
                    thenToken);

                return true;
            }

            result = ifClauseDeclarator;
            return false;
        }
    }
}