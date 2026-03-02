namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassInvocationExpressionSyntax invocationExpression)
        {
            Render(invocationExpression.IdentifierName);
            Render(invocationExpression.ArgumentList);
        }
    }
}