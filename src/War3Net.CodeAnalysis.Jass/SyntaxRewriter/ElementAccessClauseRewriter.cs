// ------------------------------------------------------------------------------
// <copyright file="ElementAccessClauseRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="elementAccessClause">The <see cref="JassElementAccessClauseSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassElementAccessClauseSyntax"/>, or the input <paramref name="elementAccessClause"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="elementAccessClause"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteElementAccessClause(JassElementAccessClauseSyntax? elementAccessClause, [NotNullIfNotNull("elementAccessClause")] out JassElementAccessClauseSyntax? result)
        {
            if (elementAccessClause is null)
            {
                result = null;
                return false;
            }

            if (RewriteToken(elementAccessClause.OpenBracketToken, out var openBracketToken) |
                RewriteExpression(elementAccessClause.Expression, out var expression) |
                RewriteToken(elementAccessClause.CloseBracketToken, out var closeBracketToken))
            {
                result = new JassElementAccessClauseSyntax(
                    openBracketToken,
                    expression,
                    closeBracketToken);

                return true;
            }

            result = elementAccessClause;
            return false;
        }
    }
}