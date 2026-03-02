namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public ExpressionSyntax Transpile(JassBinaryExpressionSyntax binaryExpression)
        {
            return SyntaxFactory.BinaryExpression(
                TranspileBinaryExpressionKind(binaryExpression.SyntaxKind),
                Transpile(binaryExpression.Left),
                Transpile(
                    TranspileBinaryOperatorKind(binaryExpression.OperatorToken.SyntaxKind),
                    binaryExpression.OperatorToken),
                Transpile(binaryExpression.Right));
        }
    }
}