// ------------------------------------------------------------------------------
// <copyright file="ArrayReferenceExpressionFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static NewExpressionSyntax ArrayReferenceExpression(string variableName, NewExpressionSyntax index)
        {
            return new NewExpressionSyntax(
                new ExpressionSyntax(
                    new ArrayReferenceSyntax(
                        Token(SyntaxTokenType.AlphanumericIdentifier, variableName),
                        Token(SyntaxTokenType.SquareBracketOpenSymbol),
                        index,
                        Token(SyntaxTokenType.SquareBracketCloseSymbol))),
                Empty());
        }
    }
}