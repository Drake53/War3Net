// ------------------------------------------------------------------------------
// <copyright file="ElseIfClauseRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="elseIfClause">The <see cref="JassElseIfClauseSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassElseIfClauseSyntax"/>, or the input <paramref name="elseIfClause"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="elseIfClause"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteElseIfClause(JassElseIfClauseSyntax elseIfClause, out JassElseIfClauseSyntax result)
        {
            if (RewriteElseIfClauseDeclarator(elseIfClause.ElseIfClauseDeclarator, out var elseIfClauseDeclarator) |
                RewriteStatementList(elseIfClause.Statements, out var statements))
            {
                result = new JassElseIfClauseSyntax(
                    elseIfClauseDeclarator,
                    statements);

                return true;
            }

            result = elseIfClause;
            return false;
        }
    }
}