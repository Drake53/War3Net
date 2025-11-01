// ------------------------------------------------------------------------------
// <copyright file="SetStatementRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="setStatement">The <see cref="JassSetStatementSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassStatementSyntax"/>, or the input <paramref name="setStatement"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="setStatement"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteSetStatement(JassSetStatementSyntax setStatement, out JassStatementSyntax result)
        {
            if (RewriteToken(setStatement.SetToken, out var setToken) |
                RewriteIdentifierName(setStatement.IdentifierName, out var identifierName) |
                RewriteElementAccessClause(setStatement.ElementAccessClause, out var elementAccessClause) |
                RewriteEqualsValueClause(setStatement.Value, out var value))
            {
                result = new JassSetStatementSyntax(
                    setToken,
                    identifierName,
                    elementAccessClause,
                    value);

                return true;
            }

            result = setStatement;
            return false;
        }
    }
}