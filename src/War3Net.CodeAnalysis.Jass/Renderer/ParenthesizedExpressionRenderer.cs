namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassParenthesizedExpressionSyntax parenthesizedExpression)
        {
            Render(parenthesizedExpression.OpenParenToken);
            Render(parenthesizedExpression.Expression);
            Render(parenthesizedExpression.CloseParenToken);
        }
    }
}