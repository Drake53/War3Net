// ------------------------------------------------------------------------------
// <copyright file="ElseClauseRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="elseClause">The <see cref="JassElseClauseSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassElseClauseSyntax"/>, or the input <paramref name="elseClause"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="elseClause"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteElseClause(JassElseClauseSyntax elseClause, out JassElseClauseSyntax result)
        {
            if (RewriteToken(elseClause.ElseToken, out var elseToken) |
                RewriteStatementList(elseClause.Statements, out var statements))
            {
                result = new JassElseClauseSyntax(
                    elseToken,
                    statements);

                return true;
            }

            result = elseClause;
            return false;
        }
    }
}