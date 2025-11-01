// ------------------------------------------------------------------------------
// <copyright file="EqualsValueClauseRewriter.cs" company="Drake53">
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
        /// <param name="equalsValueClause">The <see cref="JassEqualsValueClauseSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassEqualsValueClauseSyntax"/>, or the input <paramref name="equalsValueClause"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="equalsValueClause"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteEqualsValueClause(JassEqualsValueClauseSyntax? equalsValueClause, [NotNullIfNotNull("equalsValueClause")] out JassEqualsValueClauseSyntax? result)
        {
            if (equalsValueClause is null)
            {
                result = null;
                return false;
            }

            if (RewriteToken(equalsValueClause.EqualsToken, out var equalsToken) |
                RewriteExpression(equalsValueClause.Expression, out var expression))
            {
                result = new JassEqualsValueClauseSyntax(
                    equalsToken,
                    expression);

                return true;
            }

            result = equalsValueClause;
            return false;
        }
    }
}