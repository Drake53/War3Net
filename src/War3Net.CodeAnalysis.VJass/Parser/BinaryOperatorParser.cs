// ------------------------------------------------------------------------------
// <copyright file="BinaryOperatorParser.cs" company="Drake53">
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
        internal static Parser<char, VJassSyntaxToken> GetBinaryAddOperatorParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Symbol.Plus.AsToken(triviaParser, VJassSyntaxKind.PlusToken, VJassSymbol.Plus);
        }

        internal static Parser<char, VJassSyntaxToken> GetBinarySubtractOperatorParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Symbol.Minus.AsToken(triviaParser, VJassSyntaxKind.MinusToken, VJassSymbol.Minus);
        }

        internal static Parser<char, VJassSyntaxToken> GetBinaryMultiplicationOperatorParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Symbol.Asterisk.AsToken(triviaParser, VJassSyntaxKind.AsteriskToken, VJassSymbol.Asterisk);
        }

        internal static Parser<char, VJassSyntaxToken> GetBinaryDivisionOperatorParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Try(Symbol.Slash.Before(Not(Lookahead(Symbol.Slash)))).AsToken(triviaParser, VJassSyntaxKind.SlashToken, VJassSymbol.Slash);
        }

        internal static Parser<char, VJassSyntaxToken> GetBinaryGreaterOrEqualOperatorParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Try(Symbol.GreaterThanEquals).AsToken(triviaParser, VJassSyntaxKind.GreaterThanEqualsToken);
        }

        internal static Parser<char, VJassSyntaxToken> GetBinaryLessOrEqualOperatorParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Try(Symbol.LessThanEquals).AsToken(triviaParser, VJassSyntaxKind.LessThanEqualsToken);
        }

        internal static Parser<char, VJassSyntaxToken> GetBinaryEqualsOperatorParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Try(Symbol.EqualsEquals).AsToken(triviaParser, VJassSyntaxKind.EqualsEqualsToken);
        }

        internal static Parser<char, VJassSyntaxToken> GetBinaryNotEqualsOperatorParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Symbol.ExclamationEquals.AsToken(triviaParser, VJassSyntaxKind.ExclamationEqualsToken);
        }

        internal static Parser<char, VJassSyntaxToken> GetBinaryGreaterThanOperatorParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Symbol.GreaterThan.AsToken(triviaParser, VJassSyntaxKind.GreaterThanToken, VJassSymbol.GreaterThan);
        }

        internal static Parser<char, VJassSyntaxToken> GetBinaryLessThanOperatorParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Symbol.LessThan.AsToken(triviaParser, VJassSyntaxKind.LessThanToken, VJassSymbol.LessThan);
        }

        internal static Parser<char, VJassSyntaxToken> GetBinaryAndOperatorParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Keyword.And.AsToken(triviaParser, VJassSyntaxKind.AndKeyword);
        }

        internal static Parser<char, VJassSyntaxToken> GetBinaryOrOperatorParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Keyword.Or.AsToken(triviaParser, VJassSyntaxKind.OrKeyword);
        }
    }
}