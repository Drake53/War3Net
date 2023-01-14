// ------------------------------------------------------------------------------
// <copyright file="ElseIfClauseDeclaratorParser.cs" company="Drake53">
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
        internal static Parser<char, JassElseIfClauseDeclaratorSyntax> GetElseIfClauseDeclaratorParser(
            Parser<char, JassExpressionSyntax> expressionParser,
            Parser<char, JassSyntaxTriviaList> triviaParser,
            Parser<char, JassSyntaxTriviaList> trailingTriviaParser)
        {
            return Map(
                (elseIfToken, condition, thenToken) => new JassElseIfClauseDeclaratorSyntax(
                    elseIfToken,
                    condition,
                    thenToken),
                Keyword.ElseIf.AsToken(triviaParser, JassSyntaxKind.ElseIfKeyword),
                expressionParser,
                Keyword.Then.AsToken(trailingTriviaParser, JassSyntaxKind.ThenKeyword));
        }
    }
}