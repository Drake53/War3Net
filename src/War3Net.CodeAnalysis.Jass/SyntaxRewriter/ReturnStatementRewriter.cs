// ------------------------------------------------------------------------------
// <copyright file="ReturnStatementRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="returnStatement">The <see cref="JassReturnStatementSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassStatementSyntax"/>, or the input <paramref name="returnStatement"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="returnStatement"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteReturnStatement(JassReturnStatementSyntax returnStatement, out JassStatementSyntax result)
        {
            if (RewriteToken(returnStatement.ReturnToken, out var returnToken) |
                RewriteExpression(returnStatement.Value, out var value))
            {
                result = new JassReturnStatementSyntax(
                    returnToken,
                    value);

                return true;
            }

            result = returnStatement;
            return false;
        }
    }
}