// ------------------------------------------------------------------------------
// <copyright file="LoopStatementParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;

using Pidgin;

using War3Net.CodeAnalysis.VJass.Extensions;
using War3Net.CodeAnalysis.VJass.Syntax;

namespace War3Net.CodeAnalysis.VJass
{
    internal partial class VJassParser
    {
        internal static Parser<char, VJassStatementSyntax> GetLoopStatementParser(
            Parser<char, VJassStatementSyntax> statementParser,
            Parser<char, VJassSyntaxTriviaList> leadingTriviaParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser)
        {
            return statementParser.UntilWithLeading(
                leadingTriviaParser,
                Keyword.Loop.AsToken(trailingTriviaParser, VJassSyntaxKind.LoopKeyword),
                Keyword.EndLoop.AsToken(trailingTriviaParser, VJassSyntaxKind.EndLoopKeyword),
                (leadingTrivia, statement) => statement.WithLeadingTrivia(leadingTrivia),
                (loopToken, statements, leadingTrivia, endLoopToken) => (VJassStatementSyntax)new VJassLoopStatementSyntax(
                    loopToken,
                    statements.ToImmutableArray(),
                    endLoopToken.WithLeadingTrivia(leadingTrivia)));
        }
    }
}