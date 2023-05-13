// ------------------------------------------------------------------------------
// <copyright file="CallStatementParser.cs" company="Drake53">
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
        internal static Parser<char, JassStatementSyntax> GetCallStatementParser(
            Parser<char, JassIdentifierNameSyntax> identifierNameParser,
            Parser<char, JassArgumentListSyntax> argumentListParser,
            Parser<char, JassSyntaxTriviaList> triviaParser,
            Parser<char, JassSyntaxTriviaList> trailingTriviaParser)
        {
            return Map(
                (callToken, identifierName, argumentList, trailingTrivia) => (JassStatementSyntax)new JassCallStatementSyntax(
                    callToken,
                    identifierName,
                    argumentList.AppendTrailingTrivia(trailingTrivia)),
                Keyword.Call.AsToken(triviaParser, JassSyntaxKind.CallKeyword),
                identifierNameParser,
                argumentListParser,
                trailingTriviaParser);
        }
    }
}