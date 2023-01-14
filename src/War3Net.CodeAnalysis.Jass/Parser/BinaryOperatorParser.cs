// ------------------------------------------------------------------------------
// <copyright file="BinaryOperatorParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        //internal static Parser<char, BinaryOperatorType> GetBinaryOperatorParser(Parser<char, Unit> whitespaceParser)
        //{
        //    return OneOf(
        //        GetBinaryAddOperatorParser(whitespaceParser),
        //        GetBinarySubtractOperatorParser(whitespaceParser),
        //        GetBinaryMultiplicationOperatorParser(whitespaceParser),
        //        GetBinaryDivisionOperatorParser(whitespaceParser),
        //        GetBinaryGreaterOrEqualOperatorParser(whitespaceParser),
        //        GetBinaryLessOrEqualOperatorParser(whitespaceParser),
        //        GetBinaryEqualsOperatorParser(whitespaceParser),
        //        GetBinaryNotEqualsOperatorParser(whitespaceParser),
        //        GetBinaryGreaterThanOperatorParser(whitespaceParser),
        //        GetBinaryLessThanOperatorParser(whitespaceParser),
        //        GetBinaryAndOperatorParser(whitespaceParser),
        //        GetBinaryOrOperatorParser(whitespaceParser));
        //}

        internal static Parser<char, JassSyntaxToken> GetBinaryAddOperatorParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Symbol.Plus.AsToken(triviaParser, JassSyntaxKind.PlusToken, JassSymbol.Plus);
        }

        internal static Parser<char, JassSyntaxToken> GetBinarySubtractOperatorParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Symbol.Minus.AsToken(triviaParser, JassSyntaxKind.MinusToken, JassSymbol.Minus);
        }

        internal static Parser<char, JassSyntaxToken> GetBinaryMultiplicationOperatorParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Symbol.Asterisk.AsToken(triviaParser, JassSyntaxKind.AsteriskToken, JassSymbol.Asterisk);
        }

        internal static Parser<char, JassSyntaxToken> GetBinaryDivisionOperatorParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Try(Symbol.Slash.Before(Not(Lookahead(Symbol.Slash)))).AsToken(triviaParser, JassSyntaxKind.SlashToken, JassSymbol.Slash);
        }

        internal static Parser<char, JassSyntaxToken> GetBinaryGreaterOrEqualOperatorParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Try(Symbol.GreaterThanEquals).AsToken(triviaParser, JassSyntaxKind.GreaterThanEqualsToken);
        }

        internal static Parser<char, JassSyntaxToken> GetBinaryLessOrEqualOperatorParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Try(Symbol.LessThanEquals).AsToken(triviaParser, JassSyntaxKind.LessThanEqualsToken);
        }

        internal static Parser<char, JassSyntaxToken> GetBinaryEqualsOperatorParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Symbol.EqualsEquals.AsToken(triviaParser, JassSyntaxKind.EqualsEqualsToken);
        }

        internal static Parser<char, JassSyntaxToken> GetBinaryNotEqualsOperatorParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Symbol.ExclamationEquals.AsToken(triviaParser, JassSyntaxKind.ExclamationEqualsToken);
        }

        internal static Parser<char, JassSyntaxToken> GetBinaryGreaterThanOperatorParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Symbol.GreaterThan.AsToken(triviaParser, JassSyntaxKind.GreaterThanToken, JassSymbol.GreaterThan);
        }

        internal static Parser<char, JassSyntaxToken> GetBinaryLessThanOperatorParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Symbol.LessThan.AsToken(triviaParser, JassSyntaxKind.LessThanToken, JassSymbol.LessThan);
        }

        internal static Parser<char, JassSyntaxToken> GetBinaryAndOperatorParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Keyword.And.AsToken(triviaParser, JassSyntaxKind.AndKeyword);
        }

        internal static Parser<char, JassSyntaxToken> GetBinaryOrOperatorParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Keyword.Or.AsToken(triviaParser, JassSyntaxKind.OrKeyword);
        }
    }
}