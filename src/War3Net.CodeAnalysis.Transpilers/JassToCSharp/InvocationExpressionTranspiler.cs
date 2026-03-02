namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public ExpressionSyntax Transpile(JassInvocationExpressionSyntax invocationExpression)
        {
            return SyntaxFactory.InvocationExpression(
                Transpile(invocationExpression.IdentifierName),
                Transpile(invocationExpression.ArgumentList));
        }
    }
}