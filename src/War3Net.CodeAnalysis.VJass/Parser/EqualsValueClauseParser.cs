// ------------------------------------------------------------------------------
// <copyright file="EqualsValueClauseParser.cs" company="Drake53">
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
        internal static Parser<char, VJassEqualsValueClauseSyntax> GetEqualsValueClauseParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassExpressionSyntax> expressionParser)
        {
            return Map(
                (equalsToken, expression) => new VJassEqualsValueClauseSyntax(
                    equalsToken,
                    expression),
                Symbol.Equals.AsToken(triviaParser, VJassSyntaxKind.EqualsToken, VJassSymbol.Equals),
                expressionParser);
        }
    }
}