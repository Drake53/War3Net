// ------------------------------------------------------------------------------
// <copyright file="CharacterLiteralExpressionParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.VJass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.VJass
{
    internal partial class VJassParser
    {
        internal static Parser<char, VJassExpressionSyntax> GetCharacterLiteralExpressionParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            var escapeCharacterParser = OneOf(
                Symbol.DoubleQuote,
                Symbol.SingleQuote,
                Char('r').ThenReturn('\r'),
                Char('n').ThenReturn('\n'),
                Char('t').ThenReturn('\t'),
                Char('b').ThenReturn('\b'),
                Char('f').ThenReturn('\f'),
                Char('\\').ThenReturn('\\'));

            return Map(
                (value, trivia) => (VJassExpressionSyntax)new VJassLiteralExpressionSyntax(new VJassSyntaxToken(VJassSyntaxKind.CharacterLiteralToken, $"'{value}'", trivia)),
                Try(OneOf(
                    Char('\\').Then(escapeCharacterParser),
                    AnyCharExcept(VJassSymbol.SingleQuoteChar)).Between(Symbol.SingleQuote)),
                triviaParser)
                .Labelled("character literal");
        }
    }
}