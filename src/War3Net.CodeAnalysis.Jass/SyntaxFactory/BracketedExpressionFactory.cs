// ------------------------------------------------------------------------------
// <copyright file="BracketedExpressionFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static BracketedExpressionSyntax BracketedExpression(NewExpressionSyntax arrayIndex)
        {
            return new BracketedExpressionSyntax(Token(SyntaxTokenType.SquareBracketOpenSymbol), arrayIndex, Token(SyntaxTokenType.SquareBracketCloseSymbol));
        }
    }
}