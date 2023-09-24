// ------------------------------------------------------------------------------
// <copyright file="DebugStatementParser.cs" company="Drake53">
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
        internal static Parser<char, JassStatementSyntax> GetDebugStatementParser(
            Parser<char, JassStatementSyntax> setStatementParser,
            Parser<char, JassStatementSyntax> callStatementParser,
            Parser<char, JassStatementSyntax> ifStatementParser,
            Parser<char, JassStatementSyntax> loopStatementParser,
            Parser<char, JassSyntaxTriviaList> triviaParser,
            Parser<char, JassSyntaxTriviaList> trailingTriviaParser)
        {
            return Map(
                (debugToken, statement, trailingTrivia) => (JassStatementSyntax)new JassDebugStatementSyntax(
                    debugToken,
                    statement.AppendTrailingTrivia(trailingTrivia)),
                Keyword.Debug.AsToken(triviaParser, JassSyntaxKind.DebugKeyword),
                OneOf(
                    setStatementParser,
                    callStatementParser,
                    ifStatementParser,
                    loopStatementParser),
                trailingTriviaParser);
        }
    }
}