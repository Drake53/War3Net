// ------------------------------------------------------------------------------
// <copyright file="ParenthesizedExpressionParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.VJass.Extensions;
using War3Net.CodeAnalysis.VJass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.VJass
{
    internal partial class VJassParser
    {
        internal static Parser<char, VJassExpressionSyntax> GetParenthesizedExpressionParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassExpressionSyntax> expressionParser)
        {
            return Map(
                (openParenToken, expression, closeParenToken) => (VJassExpressionSyntax)new VJassParenthesizedExpressionSyntax(
                    openParenToken,
                    expression,
                    closeParenToken),
                Symbol.OpenParen.AsToken(triviaParser, VJassSyntaxKind.OpenParenToken, VJassSymbol.OpenParen),
                expressionParser,
                Symbol.CloseParen.AsToken(triviaParser, VJassSyntaxKind.CloseParenToken, VJassSymbol.CloseParen));
        }
    }
}