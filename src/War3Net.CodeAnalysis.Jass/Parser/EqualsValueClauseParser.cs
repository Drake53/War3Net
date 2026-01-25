// ------------------------------------------------------------------------------
// <copyright file="EqualsValueClauseParser.cs" company="Drake53">
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
        internal static Parser<char, JassEqualsValueClauseSyntax> GetEqualsValueClauseParser(
            Parser<char, JassSyntaxTriviaList> triviaParser,
            Parser<char, JassExpressionSyntax> expressionParser)
        {
            return Map(
                (equalsToken, expression) => new JassEqualsValueClauseSyntax(
                    equalsToken,
                    expression),
                Symbol.Equals.AsToken(triviaParser, JassSyntaxKind.EqualsToken, JassSymbol.Equals),
                expressionParser);
        }
    }
}