using Pidgin;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;
using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassIfClauseDeclaratorSyntax> GetIfClauseDeclaratorParser(
            Parser<char, JassExpressionSyntax> expressionParser,
            Parser<char, JassSyntaxTriviaList> triviaParser,
            Parser<char, JassSyntaxTriviaList> trailingTriviaParser)
        {
            return Map(
                (ifToken, condition, thenToken) => new JassIfClauseDeclaratorSyntax(
                    ifToken,
                    condition,
                    thenToken),
                Keyword.If.AsToken(triviaParser, JassSyntaxKind.IfKeyword),
                expressionParser,
                Keyword.Then.AsToken(trailingTriviaParser, JassSyntaxKind.ThenKeyword));
        }
    }
}