namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassUnaryExpressionSyntax unaryExpression)
        {
            Render(unaryExpression.OperatorToken);
            if (unaryExpression.SyntaxKind == JassSyntaxKind.LogicalNotExpression)
            {
                WriteSpace();
            }

            Render(unaryExpression.Expression);
        }
    }
}