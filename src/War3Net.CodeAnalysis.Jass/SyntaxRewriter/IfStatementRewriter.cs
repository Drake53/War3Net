// ------------------------------------------------------------------------------
// <copyright file="IfStatementRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="ifStatement">The <see cref="JassIfStatementSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassStatementSyntax"/>, or the input <paramref name="ifStatement"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="ifStatement"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteIfStatement(JassIfStatementSyntax ifStatement, out JassStatementSyntax result)
        {
            if (RewriteIfClause(ifStatement.IfClause, out var ifClause) |
                RewriteElseIfClauseList(ifStatement.ElseIfClauses, out var elseIfClauses) |
                RewriteElseClause(ifStatement.ElseClause, out var elseClause) |
                RewriteToken(ifStatement.EndIfToken, out var endIfToken))
            {
                result = new JassIfStatementSyntax(
                    ifClause,
                    elseIfClauses,
                    elseClause,
                    endIfToken);

                return true;
            }

            result = ifStatement;
            return false;
        }
    }
}