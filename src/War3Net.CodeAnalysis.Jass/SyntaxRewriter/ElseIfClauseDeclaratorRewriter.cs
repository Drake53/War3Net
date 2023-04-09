// ------------------------------------------------------------------------------
// <copyright file="ElseIfClauseDeclaratorRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="elseIfClauseDeclarator">The <see cref="JassElseIfClauseDeclaratorSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassElseIfClauseDeclaratorSyntax"/>, or the input <paramref name="elseIfClauseDeclarator"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="elseIfClauseDeclarator"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteElseIfClauseDeclarator(JassElseIfClauseDeclaratorSyntax elseIfClauseDeclarator, out JassElseIfClauseDeclaratorSyntax result)
        {
            if (RewriteToken(elseIfClauseDeclarator.ElseIfToken, out var elseIfToken) |
                RewriteExpression(elseIfClauseDeclarator.Condition, out var condition) |
                RewriteToken(elseIfClauseDeclarator.ThenToken, out var thenToken))
            {
                result = new JassElseIfClauseDeclaratorSyntax(
                    elseIfToken,
                    condition,
                    thenToken);

                return true;
            }

            result = elseIfClauseDeclarator;
            return false;
        }
    }
}