// ------------------------------------------------------------------------------
// <copyright file="SetStatementParser.cs" company="Drake53">
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
        internal static Parser<char, JassStatementSyntax> GetSetStatementParser(
            Parser<char, JassIdentifierNameSyntax> identifierNameParser,
            Parser<char, JassElementAccessClauseSyntax> elementAccessClauseParser,
            Parser<char, JassEqualsValueClauseSyntax> equalsValueClauseParser,
            Parser<char, JassSyntaxTriviaList> triviaParser,
            Parser<char, JassSyntaxTriviaList> trailingTriviaParser)
        {
            return Map(
                (setToken, identifierName, elementAccessClause, value, trailingTrivia) => (JassStatementSyntax)new JassSetStatementSyntax(
                    setToken,
                    identifierName,
                    elementAccessClause.GetValueOrDefault(),
                    value.AppendTrailingTrivia(trailingTrivia)),
                Keyword.Set.AsToken(triviaParser, JassSyntaxKind.SetKeyword),
                identifierNameParser,
                elementAccessClauseParser.Optional(),
                equalsValueClauseParser,
                trailingTriviaParser);
        }
    }
}