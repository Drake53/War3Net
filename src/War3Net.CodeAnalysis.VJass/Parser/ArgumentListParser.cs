// ------------------------------------------------------------------------------
// <copyright file="ArgumentListParser.cs" company="Drake53">
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
        internal static Parser<char, VJassArgumentListSyntax> GetArgumentListParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassExpressionSyntax> expressionParser)
        {
            return Map(
                (openParenToken, argumentList, closeParenToken) => new VJassArgumentListSyntax(
                    openParenToken,
                    argumentList,
                    closeParenToken),
                Symbol.OpenParen.AsToken(triviaParser, VJassSyntaxKind.OpenParenToken, VJassSymbol.OpenParen),
                expressionParser.SeparatedList(Symbol.Comma.AsToken(triviaParser, VJassSyntaxKind.CommaToken, VJassSymbol.Comma)),
                Symbol.CloseParen.AsToken(triviaParser, VJassSyntaxKind.CloseParenToken, VJassSymbol.CloseParen));
        }

        internal static Parser<char, VJassArgumentListSyntax> GetArgumentListParser(
            Parser<char, VJassExpressionSyntax> stringLiteralExpressionParser,
            Parser<char, ISyntaxTrivia> optionalWhitespaceTriviaParser,
            Parser<char, VJassSyntaxTriviaList> preprocessorDirectiveEndTriviaListParser)
        {
            return Map(
                (openParenToken, argumentList, closeParenToken) => new VJassArgumentListSyntax(
                    openParenToken,
                    argumentList,
                    closeParenToken),
                Symbol.OpenParen.AsToken(optionalWhitespaceTriviaParser, VJassSyntaxKind.OpenParenToken, VJassSymbol.OpenParen),
                stringLiteralExpressionParser.SeparatedList(Symbol.Comma.AsToken(optionalWhitespaceTriviaParser, VJassSyntaxKind.CommaToken, VJassSymbol.Comma)),
                Symbol.CloseParen.AsToken(preprocessorDirectiveEndTriviaListParser, VJassSyntaxKind.CloseParenToken, VJassSymbol.CloseParen));
        }
    }
}