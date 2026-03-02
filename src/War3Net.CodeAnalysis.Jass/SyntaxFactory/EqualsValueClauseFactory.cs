namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassEqualsValueClauseSyntax EqualsValueClause(JassExpressionSyntax expression)
        {
            return new JassEqualsValueClauseSyntax(
                Token(JassSyntaxKind.EqualsToken),
                expression);
        }

        public static JassEqualsValueClauseSyntax EqualsValueClause(JassSyntaxToken equalsToken, JassExpressionSyntax expression)
        {
            ThrowHelper.ThrowIfInvalidToken(equalsToken, JassSyntaxKind.EqualsToken);

            return new JassEqualsValueClauseSyntax(
                equalsToken,
                expression);
        }
    }
}