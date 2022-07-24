// ------------------------------------------------------------------------------
// <copyright file="ExitStatementParser.cs" company="Drake53">
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
        internal static Parser<char, VJassExitStatementSyntax> GetExitStatementParser(
            Parser<char, VJassExpressionSyntax> expressionParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser)
        {
            return Map(
                (exitWhenToken, expression, trailingTrivia) => new VJassExitStatementSyntax(
                    exitWhenToken,
                    expression.AppendTrivia(trailingTrivia)),
                Keyword.ExitWhen.AsToken(triviaParser, VJassSyntaxKind.ExitWhenKeyword),
                expressionParser,
                trailingTriviaParser);
        }
    }
}