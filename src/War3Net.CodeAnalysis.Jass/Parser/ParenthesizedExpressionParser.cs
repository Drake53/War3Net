using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassExpressionSyntax> GetParenthesizedExpressionParser(
            Parser<char, JassSyntaxTriviaList> triviaParser,
            Parser<char, JassExpressionSyntax> expressionParser)
        {
            return Map(
                (openParenToken, expression, closeParenToken) => (JassExpressionSyntax)new JassParenthesizedExpressionSyntax(
                    openParenToken,
                    expression,
                    closeParenToken),
                Symbol.OpenParen.AsToken(triviaParser, JassSyntaxKind.OpenParenToken, JassSymbol.OpenParen),
                expressionParser,
                Symbol.CloseParen.AsToken(triviaParser, JassSyntaxKind.CloseParenToken, JassSymbol.CloseParen));
        }
    }
}