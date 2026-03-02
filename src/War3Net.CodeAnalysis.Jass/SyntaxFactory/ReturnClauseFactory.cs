namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassReturnClauseSyntax ReturnClause(JassTypeSyntax returnType)
        {
            return new JassReturnClauseSyntax(
                Token(JassSyntaxKind.ReturnsKeyword),
                returnType);
        }

        public static JassReturnClauseSyntax ReturnClause(JassSyntaxToken returnsToken, JassTypeSyntax returnType)
        {
            ThrowHelper.ThrowIfInvalidToken(returnsToken, JassSyntaxKind.ReturnsKeyword);

            return new JassReturnClauseSyntax(
                returnsToken,
                returnType);
        }
    }
}