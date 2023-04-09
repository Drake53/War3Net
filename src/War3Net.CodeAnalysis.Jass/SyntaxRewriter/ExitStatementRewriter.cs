// ------------------------------------------------------------------------------
// <copyright file="ExitStatementRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="exitStatement">The <see cref="JassExitStatementSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassStatementSyntax"/>, or the input <paramref name="exitStatement"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="exitStatement"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteExitStatement(JassExitStatementSyntax exitStatement, out JassStatementSyntax result)
        {
            if (RewriteToken(exitStatement.ExitWhenToken, out var exitWhenToken) |
                RewriteExpression(exitStatement.Condition, out var condition))
            {
                result = new JassExitStatementSyntax(
                    exitWhenToken,
                    condition);

                return true;
            }

            result = exitStatement;
            return false;
        }
    }
}