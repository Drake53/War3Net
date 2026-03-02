using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassStatementSyntax> GetReturnStatementParser(
            Parser<char, JassExpressionSyntax> expressionParser,
            Parser<char, JassSyntaxTriviaList> triviaParser,
            Parser<char, JassSyntaxTriviaList> trailingTriviaParser)
        {
            return Map(
                (returnToken, expression, trailingTrivia) => (JassStatementSyntax)new JassReturnStatementSyntax(
                    returnToken,
                    expression.GetValueOrDefault()).AppendTrailingTrivia(trailingTrivia),
                Keyword.Return.AsToken(triviaParser, JassSyntaxKind.ReturnKeyword),
                expressionParser.Optional(),
                trailingTriviaParser);
        }
    }
}