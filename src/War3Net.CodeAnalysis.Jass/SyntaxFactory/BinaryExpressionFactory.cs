namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassBinaryExpressionSyntax BinaryAddExpression(JassExpressionSyntax left, JassExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(
                left,
                Token(JassSyntaxKind.PlusToken),
                right);
        }

        public static JassBinaryExpressionSyntax BinarySubtractExpression(JassExpressionSyntax left, JassExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(
                left,
                Token(JassSyntaxKind.MinusToken),
                right);
        }

        public static JassBinaryExpressionSyntax BinaryMultiplyExpression(JassExpressionSyntax left, JassExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(
                left,
                Token(JassSyntaxKind.AsteriskToken),
                right);
        }

        public static JassBinaryExpressionSyntax BinaryDivideExpression(JassExpressionSyntax left, JassExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(
                left,
                Token(JassSyntaxKind.SlashToken),
                right);
        }

        public static JassBinaryExpressionSyntax BinaryGreaterThanExpression(JassExpressionSyntax left, JassExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(
                left,
                Token(JassSyntaxKind.GreaterThanToken),
                right);
        }

        public static JassBinaryExpressionSyntax BinaryLessThanExpression(JassExpressionSyntax left, JassExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(
                left,
                Token(JassSyntaxKind.LessThanToken),
                right);
        }

        public static JassBinaryExpressionSyntax BinaryEqualsExpression(JassExpressionSyntax left, JassExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(
                left,
                Token(JassSyntaxKind.EqualsEqualsToken),
                right);
        }

        public static JassBinaryExpressionSyntax BinaryNotEqualsExpression(JassExpressionSyntax left, JassExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(
                left,
                Token(JassSyntaxKind.ExclamationEqualsToken),
                right);
        }

        public static JassBinaryExpressionSyntax BinaryGreaterThanOrEqualExpression(JassExpressionSyntax left, JassExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(
                left,
                Token(JassSyntaxKind.GreaterThanEqualsToken),
                right);
        }

        public static JassBinaryExpressionSyntax BinaryLessThanOrEqualExpression(JassExpressionSyntax left, JassExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(
                left,
                Token(JassSyntaxKind.LessThanEqualsToken),
                right);
        }

        public static JassBinaryExpressionSyntax BinaryAndExpression(JassExpressionSyntax left, JassExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(
                left,
                Token(JassSyntaxKind.AndKeyword),
                right);
        }

        public static JassBinaryExpressionSyntax BinaryOrExpression(JassExpressionSyntax left, JassExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(
                left,
                Token(JassSyntaxKind.OrKeyword),
                right);
        }

        public static JassBinaryExpressionSyntax BinaryExpression(JassExpressionSyntax left, JassSyntaxToken operatorToken, JassExpressionSyntax right)
        {
            ThrowHelper.ThrowIfInvalidBinaryOperatorToken(operatorToken);

            return new JassBinaryExpressionSyntax(
                left,
                operatorToken,
                right);
        }
    }
}