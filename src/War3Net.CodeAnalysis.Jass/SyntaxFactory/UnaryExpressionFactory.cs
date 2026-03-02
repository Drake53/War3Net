namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassUnaryExpressionSyntax UnaryPlusExpression(JassExpressionSyntax expression)
        {
            return new JassUnaryExpressionSyntax(
                Token(JassSyntaxKind.PlusToken),
                expression);
        }

        public static JassUnaryExpressionSyntax UnaryMinusExpression(JassExpressionSyntax expression)
        {
            return new JassUnaryExpressionSyntax(
                Token(JassSyntaxKind.MinusToken),
                expression);
        }

        public static JassUnaryExpressionSyntax UnaryNotExpression(JassExpressionSyntax expression)
        {
            return new JassUnaryExpressionSyntax(
                Token(JassSyntaxKind.NotKeyword),
                expression);
        }

        public static JassUnaryExpressionSyntax UnaryExpression(JassSyntaxToken operatorToken, JassExpressionSyntax expression)
        {
            ThrowHelper.ThrowIfInvalidUnaryOperatorToken(operatorToken);

            return new JassUnaryExpressionSyntax(
                operatorToken,
                expression);
        }
    }
}