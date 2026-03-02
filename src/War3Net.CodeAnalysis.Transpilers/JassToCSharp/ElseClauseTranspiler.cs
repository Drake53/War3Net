namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public ElseClauseSyntax Transpile(JassElseClauseSyntax elseClause, JassSyntaxToken closingToken)
        {
            var elseBlock = SyntaxFactory.Block(
                Transpile(SyntaxKind.OpenBraceToken, elseClause.ElseToken.TrailingTrivia),
                SyntaxFactory.List(elseClause.Statements.Select(Transpile)),
                Transpile(SyntaxKind.CloseBraceToken, closingToken));

            return SyntaxFactory.ElseClause(
                Token(SyntaxKind.ElseKeyword, SyntaxTriviaList.Create(SyntaxFactory.ElasticSpace)),
                elseBlock);
        }
    }
}