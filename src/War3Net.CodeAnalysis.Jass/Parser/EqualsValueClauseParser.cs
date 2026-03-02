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