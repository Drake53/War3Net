// ------------------------------------------------------------------------------
// <copyright file="ReturnStatementParser.cs" company="Drake53">
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
        internal static Parser<char, VJassReturnStatementSyntax> GetReturnStatementParser(
            Parser<char, VJassExpressionSyntax> expressionParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser)
        {
            return Map(
                (returnToken, expression, trailingTrivia) => new VJassReturnStatementSyntax(
                    returnToken,
                    expression.GetValueOrDefault()).AppendTrivia(trailingTrivia),
                Keyword.Return.AsToken(triviaParser, VJassSyntaxKind.ReturnKeyword),
                expressionParser.Optional(),
                trailingTriviaParser);
        }
    }
}