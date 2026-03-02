namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public StatementSyntax Transpile(JassSetStatementSyntax setStatement)
        {
            var leadingTrivia = MergeTrivia(setStatement.SetToken, setStatement.IdentifierName.GetLeadingTrivia());

            ExpressionSyntax left = setStatement.ElementAccessClause is null
                ? Transpile(leadingTrivia, setStatement.IdentifierName)
                : SyntaxFactory.ElementAccessExpression(
                    Transpile(leadingTrivia, setStatement.IdentifierName),
                    Transpile(setStatement.ElementAccessClause));

            var assignmentExpression = SyntaxFactory.AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                left,
                Transpile(SyntaxKind.EqualsToken, setStatement.EqualsValueClause.EqualsToken),
                Transpile(setStatement.EqualsValueClause.Value));

            var trailingTrivia = assignmentExpression.GetTrailingTrivia();

            return SyntaxFactory.ExpressionStatement(
                assignmentExpression.WithoutTrailingTrivia(),
                SyntaxFactory.Token(
                    SyntaxTriviaList.Empty,
                    SyntaxKind.SemicolonToken,
                    trailingTrivia));
        }
    }
}