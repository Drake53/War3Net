// ------------------------------------------------------------------------------
// <copyright file="CallStatementRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="callStatement">The <see cref="JassCallStatementSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassStatementSyntax"/>, or the input <paramref name="callStatement"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="callStatement"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteCallStatement(JassCallStatementSyntax callStatement, out JassStatementSyntax result)
        {
            if (RewriteToken(callStatement.CallToken, out var callToken) |
                RewriteIdentifierName(callStatement.IdentifierName, out var identifierName) |
                RewriteArgumentList(callStatement.ArgumentList, out var argumentList))
            {
                result = new JassCallStatementSyntax(
                    callToken,
                    identifierName,
                    argumentList);

                return true;
            }

            result = callStatement;
            return false;
        }
    }
}