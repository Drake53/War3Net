// ------------------------------------------------------------------------------
// <copyright file="ElementAccessClauseFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassElementAccessClauseSyntax ElementAccessClause(JassExpressionSyntax expression)
        {
            return new JassElementAccessClauseSyntax(
                Token(JassSyntaxKind.OpenBracketToken),
                expression,
                Token(JassSyntaxKind.CloseBracketToken));
        }

        public static JassElementAccessClauseSyntax ElementAccessClause(JassSyntaxToken openBracketToken, JassExpressionSyntax expression, JassSyntaxToken closeBracketToken)
        {
            ThrowHelper.ThrowIfInvalidToken(openBracketToken, JassSyntaxKind.OpenBracketToken);
            ThrowHelper.ThrowIfInvalidToken(closeBracketToken, JassSyntaxKind.CloseBracketToken);

            return new JassElementAccessClauseSyntax(
                openBracketToken,
                expression,
                closeBracketToken);
        }
    }
}