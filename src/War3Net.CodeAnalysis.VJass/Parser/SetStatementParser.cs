// ------------------------------------------------------------------------------
// <copyright file="SetStatementParser.cs" company="Drake53">
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
        internal static Parser<char, VJassSetStatementSyntax> GetSetStatementParser(
            Parser<char, VJassExpressionSyntax> expressionParser,
            Parser<char, VJassEqualsValueClauseSyntax> equalsValueClauseParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser)
        {
            return Map(
                (setToken, identifier, equals, trailingTrivia) => new VJassSetStatementSyntax(
                    setToken,
                    identifier,
                    equals.AppendTrivia(trailingTrivia)),
                Keyword.Set.AsToken(triviaParser, VJassSyntaxKind.SetKeyword),
                expressionParser,
                equalsValueClauseParser,
                trailingTriviaParser);
        }
    }
}