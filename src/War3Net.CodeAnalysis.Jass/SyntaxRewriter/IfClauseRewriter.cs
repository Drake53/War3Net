// ------------------------------------------------------------------------------
// <copyright file="IfClauseRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="ifClause">The <see cref="JassIfClauseSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassIfClauseSyntax"/>, or the input <paramref name="ifClause"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="ifClause"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteIfClause(JassIfClauseSyntax ifClause, out JassIfClauseSyntax result)
        {
            if (RewriteIfClauseDeclarator(ifClause.IfClauseDeclarator, out var ifClauseDeclarator) |
                RewriteStatementList(ifClause.Statements, out var statements))
            {
                result = new JassIfClauseSyntax(
                    ifClauseDeclarator,
                    statements);

                return true;
            }

            result = ifClause;
            return false;
        }
    }
}