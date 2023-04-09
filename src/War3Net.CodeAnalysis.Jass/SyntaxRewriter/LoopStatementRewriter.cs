// ------------------------------------------------------------------------------
// <copyright file="LoopStatementRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="loopStatement">The <see cref="JassLoopStatementSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassStatementSyntax"/>, or the input <paramref name="loopStatement"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="loopStatement"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteLoopStatement(JassLoopStatementSyntax loopStatement, out JassStatementSyntax result)
        {
            if (RewriteToken(loopStatement.LoopToken, out var loopToken) |
                RewriteStatementList(loopStatement.Statements, out var statements) |
                RewriteToken(loopStatement.EndLoopToken, out var endLoopToken))
            {
                result = new JassLoopStatementSyntax(
                    loopToken,
                    statements,
                    endLoopToken);

                return true;
            }

            result = loopStatement;
            return false;
        }
    }
}