namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public ExpressionSyntax Transpile(JassUnaryExpressionSyntax unaryExpression)
        {
            return SyntaxFactory.PrefixUnaryExpression(
                TranspileUnaryExpressionKind(unaryExpression.SyntaxKind),
                Transpile(
                    TranspileUnaryOperatorKind(unaryExpression.OperatorToken.SyntaxKind),
                    unaryExpression.OperatorToken),
                Transpile(unaryExpression.Operand));
        }
    }
}