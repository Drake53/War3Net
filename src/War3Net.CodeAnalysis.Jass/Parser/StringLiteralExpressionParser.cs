// ------------------------------------------------------------------------------
// <copyright file="StringLiteralExpressionParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassExpressionSyntax> GetStringLiteralExpressionParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            var escapeSequenceParser = OneOf(
                Symbol.DoubleQuote.ThenReturn($"\\{JassSymbol.DoubleQuoteChar}"),
                Symbol.SingleQuote.ThenReturn($"\\{JassSymbol.SingleQuoteChar}"),
                Char('r').ThenReturn("\\r"),
                Char('n').ThenReturn("\\n"),
                Char('t').ThenReturn("\\t"),
                Char('b').ThenReturn("\\b"),
                Char('f').ThenReturn("\\f"),
                Char('\\').ThenReturn("\\\\"),
                Any.Then(c => Fail<string>($"\"\\{c}\" is not a valid escape sequence")))
                .Labelled("escape sequence");

            var stringLiteralParser = Char('\\').Then(escapeSequenceParser).Or(AnyCharExcept(JassSymbol.DoubleQuoteChar).Map(char.ToString)).ManyString().Between(Symbol.DoubleQuote)
                .Labelled("string literal");

            return Map(
                (value, trivia) => (JassExpressionSyntax)new JassLiteralExpressionSyntax(
                    new JassSyntaxToken(JassSyntaxKind.StringLiteralToken, $"{JassSymbol.DoubleQuoteChar}{value}{JassSymbol.DoubleQuoteChar}", trivia)),
                stringLiteralParser,
                triviaParser)
                .Labelled("string literal");
        }
    }
}