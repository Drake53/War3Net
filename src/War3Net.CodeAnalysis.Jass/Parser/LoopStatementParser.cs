// ------------------------------------------------------------------------------
// <copyright file="LoopStatementParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;

using Pidgin;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassStatementSyntax> GetLoopStatementParser(
            Parser<char, JassStatementSyntax> statementParser,
            Parser<char, JassSyntaxTriviaList> leadingTriviaParser,
            Parser<char, JassSyntaxTriviaList> trailingTriviaParser)
        {
            return statementParser.UntilWithLeading(
                leadingTriviaParser,
                Keyword.Loop.AsToken(trailingTriviaParser, JassSyntaxKind.LoopKeyword),
                Keyword.EndLoop.AsToken(trailingTriviaParser, JassSyntaxKind.EndLoopKeyword),
                (leadingTrivia, statement) => statement.WithLeadingTrivia(leadingTrivia),
                (loopToken, statements, leadingTrivia, endLoopToken) => (JassStatementSyntax)new JassLoopStatementSyntax(
                    loopToken,
                    statements.ToImmutableArray(),
                    endLoopToken.WithLeadingTrivia(leadingTrivia)));
        }
    }
}