// ------------------------------------------------------------------------------
// <copyright file="CharacterLiteralExpressionParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassExpressionSyntax> GetCharacterLiteralExpressionParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
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
                (value, trivia) => (JassExpressionSyntax)new JassLiteralExpressionSyntax(new JassSyntaxToken(JassSyntaxKind.CharacterLiteralToken, $"'{value}'", trivia)),
                Try(OneOf(
                    Char('\\').Then(escapeCharacterParser),
                    AnyCharExcept(JassSymbol.SingleQuoteChar)).Between(Symbol.SingleQuote)),
                triviaParser)
                .Labelled("character literal");
        }
    }
}