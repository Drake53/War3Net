namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassElseIfClauseDeclaratorSyntax ElseIfClauseDeclarator(JassExpressionSyntax condition)
        {
            return new JassElseIfClauseDeclaratorSyntax(
                Token(JassSyntaxKind.ElseIfKeyword),
                condition,
                Token(JassSyntaxKind.ThenKeyword));
        }

        public static JassElseIfClauseDeclaratorSyntax ElseIfClauseDeclarator(JassSyntaxToken elseIfToken, JassExpressionSyntax condition, JassSyntaxToken thenToken)
        {
            ThrowHelper.ThrowIfInvalidToken(elseIfToken, JassSyntaxKind.ElseIfKeyword);
            ThrowHelper.ThrowIfInvalidToken(thenToken, JassSyntaxKind.ThenKeyword);

            return new JassElseIfClauseDeclaratorSyntax(
                elseIfToken,
                condition,
                thenToken);
        }
    }
}