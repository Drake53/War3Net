namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public StatementSyntax Transpile(JassReturnStatementSyntax returnStatement)
        {
            var returnKeyword = Transpile(SyntaxKind.ReturnKeyword, returnStatement.ReturnToken);

            ExpressionSyntax? expression;
            SyntaxTriviaList trailingTrivia;

            if (returnStatement.Value is null)
            {
                expression = null;
                trailingTrivia = returnKeyword.TrailingTrivia;
                returnKeyword = returnKeyword.WithoutTrailingTrivia();
            }
            else
            {
                expression = Transpile(returnStatement.Value);
                trailingTrivia = expression.GetTrailingTrivia();
                expression = expression.WithoutTrailingTrivia();
            }

            return SyntaxFactory.ReturnStatement(
                returnKeyword,
                expression,
                SyntaxFactory.Token(
                    SyntaxTriviaList.Empty,
                    SyntaxKind.SemicolonToken,
                    trailingTrivia));
        }
    }
}