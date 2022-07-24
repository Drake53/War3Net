// ------------------------------------------------------------------------------
// <copyright file="UnaryOperatorParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.VJass.Extensions;
using War3Net.CodeAnalysis.VJass.Syntax;

namespace War3Net.CodeAnalysis.VJass
{
    internal partial class VJassParser
    {
        internal static Parser<char, VJassSyntaxToken> GetUnaryPlusOperatorParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Symbol.Plus.AsToken(triviaParser, VJassSyntaxKind.PlusToken, VJassSymbol.Plus);
        }

        internal static Parser<char, VJassSyntaxToken> GetUnaryMinusOperatorParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Symbol.Minus.AsToken(triviaParser, VJassSyntaxKind.MinusToken, VJassSymbol.Minus);
        }

        internal static Parser<char, VJassSyntaxToken> GetUnaryNotOperatorParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Keyword.Not.AsToken(triviaParser, VJassSyntaxKind.NotKeyword);
        }
    }
}