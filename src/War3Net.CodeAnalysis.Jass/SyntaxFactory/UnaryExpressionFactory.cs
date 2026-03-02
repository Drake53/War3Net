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

        public static JassUnaryExpressionSyntax UnaryNotExpression(JassSyntaxToken operatorToken, JassExpressionSyntax expression)
        {
            var expressionKind = JassSyntaxFacts.GetUnaryExpressionKind(operatorToken.SyntaxKind);
            if (expressionKind == JassSyntaxKind.None)
            {
                throw new ArgumentException($"'{operatorToken.SyntaxKind}' is not a valid operator kind for unary expressions.", nameof(operatorToken));
            }

            return new JassUnaryExpressionSyntax(
                operatorToken,
                expression);
        }
    }
}