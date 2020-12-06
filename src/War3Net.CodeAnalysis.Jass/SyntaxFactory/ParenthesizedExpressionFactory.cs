// ------------------------------------------------------------------------------
// <copyright file="ParenthesizedExpressionFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static NewExpressionSyntax ParenthesizedExpression(NewExpressionSyntax expression)
        {
            return new NewExpressionSyntax(
                new ExpressionSyntax(
                    new ParenthesizedExpressionSyntax(
                        Token(SyntaxTokenType.ParenthesisOpenSymbol),
                        expression,
                        Token(SyntaxTokenType.ParenthesisCloseSymbol))),
                Empty());
        }
    }
}