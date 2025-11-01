// ------------------------------------------------------------------------------
// <copyright file="ParenthesizedExpressionFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassParenthesizedExpressionSyntax ParenthesizedExpression(JassExpressionSyntax expression)
        {
            return new JassParenthesizedExpressionSyntax(
                Token(JassSyntaxKind.OpenParenToken),
                expression,
                Token(JassSyntaxKind.CloseParenToken));
        }

        public static JassParenthesizedExpressionSyntax ParenthesizedExpression(JassSyntaxToken openParenToken, JassExpressionSyntax expression, JassSyntaxToken closeParenToken)
        {
            ThrowHelper.ThrowIfInvalidToken(openParenToken, JassSyntaxKind.OpenParenToken);
            ThrowHelper.ThrowIfInvalidToken(closeParenToken, JassSyntaxKind.CloseParenToken);

            return new JassParenthesizedExpressionSyntax(
                openParenToken,
                expression,
                closeParenToken);
        }
    }
}