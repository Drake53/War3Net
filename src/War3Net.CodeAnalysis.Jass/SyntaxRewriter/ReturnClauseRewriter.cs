// ------------------------------------------------------------------------------
// <copyright file="ReturnClauseRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="returnClause">The <see cref="JassReturnClauseSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassReturnClauseSyntax"/>, or the input <paramref name="returnClause"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="returnClause"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteReturnClause(JassReturnClauseSyntax returnClause, out JassReturnClauseSyntax result)
        {
            if (RewriteToken(returnClause.ReturnsToken, out var returnsToken) |
                RewriteType(returnClause.ReturnType, out var returnType))
            {
                result = new JassReturnClauseSyntax(
                    returnsToken,
                    returnType);

                return true;
            }

            result = returnClause;
            return false;
        }
    }
}