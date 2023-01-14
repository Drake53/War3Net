// ------------------------------------------------------------------------------
// <copyright file="UnaryOperatorParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        //internal static Parser<char, UnaryOperatorType> GetUnaryOperatorParser(Parser<char, Unit> whitespaceParser)
        //{
        //    return OneOf(
        //        GetUnaryPlusOperatorParser(whitespaceParser),
        //        GetUnaryMinusOperatorParser(whitespaceParser),
        //        GetUnaryNotOperatorParser(whitespaceParser));
        //}

        internal static Parser<char, JassSyntaxToken> GetUnaryPlusOperatorParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Symbol.Plus.AsToken(triviaParser, JassSyntaxKind.PlusToken, JassSymbol.Plus);
        }

        internal static Parser<char, JassSyntaxToken> GetUnaryMinusOperatorParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Symbol.Minus.AsToken(triviaParser, JassSyntaxKind.MinusToken, JassSymbol.Minus);
        }

        internal static Parser<char, JassSyntaxToken> GetUnaryNotOperatorParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Keyword.Not.AsToken(triviaParser, JassSyntaxKind.NotKeyword);
        }
    }
}