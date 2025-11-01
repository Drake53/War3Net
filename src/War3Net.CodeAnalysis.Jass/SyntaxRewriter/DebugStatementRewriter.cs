// ------------------------------------------------------------------------------
// <copyright file="DebugStatementRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="debugStatement">The <see cref="JassDebugStatementSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassStatementSyntax"/>, or the input <paramref name="debugStatement"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="debugStatement"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteDebugStatement(JassDebugStatementSyntax debugStatement, out JassStatementSyntax result)
        {
            if (RewriteToken(debugStatement.DebugToken, out var debugToken) |
                RewriteStatement(debugStatement.Statement, out var statement))
            {
                result = new JassDebugStatementSyntax(
                    debugToken,
                    statement);

                return true;
            }

            result = debugStatement;
            return false;
        }
    }
}