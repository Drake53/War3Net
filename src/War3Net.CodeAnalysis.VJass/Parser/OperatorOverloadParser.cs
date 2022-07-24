// ------------------------------------------------------------------------------
// <copyright file="OperatorOverloadParser.cs" company="Drake53">
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
        internal static Parser<char, VJassSyntaxToken> GetEqualsOperatorOverloadParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Symbol.EqualsEquals.AsToken(triviaParser, VJassSyntaxKind.EqualsEqualsToken);
        }

        internal static Parser<char, VJassSyntaxToken> GetLessThanOperatorOverloadParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Symbol.LessThan.AsToken(triviaParser, VJassSyntaxKind.LessThanToken, VJassSymbol.LessThan);
        }
    }
}