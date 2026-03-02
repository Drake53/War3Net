namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public StatementSyntax Transpile(JassLoopStatementSyntax loopStatement)
        {
            return SyntaxFactory.WhileStatement(
                Transpile(
                    loopStatement.LoopToken.LeadingTrivia,
                    SyntaxKind.WhileKeyword,
                    JassSyntaxTriviaList.SingleSpace),
                SyntaxFactory.Token(SyntaxKind.OpenParenToken),
                SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression),
                SyntaxFactory.Token(
                    SyntaxTriviaList.Empty,
                    SyntaxKind.CloseParenToken,
                    SyntaxTriviaList.Create(SyntaxFactory.ElasticSpace)),
                SyntaxFactory.Block(
                    Transpile(SyntaxKind.OpenBraceToken, loopStatement.LoopToken.TrailingTrivia),
                    SyntaxFactory.List(loopStatement.Statements.Select(Transpile)),
                    Transpile(SyntaxKind.CloseBraceToken, loopStatement.EndLoopToken)));
        }
    }
}