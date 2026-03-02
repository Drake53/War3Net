namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassIdentifierNameSyntax IdentifierName(JassSyntaxToken identifier)
        {
            ThrowHelper.ThrowIfInvalidToken(identifier, JassSyntaxKind.IdentifierToken);

            return new JassIdentifierNameSyntax(identifier);
        }

        public static JassIdentifierNameSyntax IdentifierName(string identifierName)
        {
            return new JassIdentifierNameSyntax(Identifier(identifierName));
        }

        public static JassSyntaxToken Identifier(string text)
        {
            return Identifier(JassSyntaxTriviaList.Empty, text, JassSyntaxTriviaList.Empty);
        }

        public static JassSyntaxToken Identifier(JassSyntaxTriviaList leadingTrivia, string text, JassSyntaxTriviaList trailingTrivia)
        {
            ThrowHelper.ThrowIfInvalidIdentifier(text);

            return new JassSyntaxToken(leadingTrivia, JassSyntaxKind.IdentifierToken, text, trailingTrivia);
        }
    }
}