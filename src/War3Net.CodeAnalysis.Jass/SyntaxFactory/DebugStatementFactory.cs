namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassDebugStatementSyntax DebugStatement(JassStatementSyntax statement)
        {
            ThrowHelper.ThrowIfInvalidDebugStatement(statement);

            return new JassDebugStatementSyntax(
                Token(JassSyntaxKind.DebugKeyword),
                statement);
        }

        public static JassDebugStatementSyntax DebugStatement(JassSyntaxToken debugToken, JassStatementSyntax statement)
        {
            ThrowHelper.ThrowIfInvalidToken(debugToken, JassSyntaxKind.DebugKeyword);
            ThrowHelper.ThrowIfInvalidDebugStatement(statement);

            return new JassDebugStatementSyntax(
                debugToken,
                statement);
        }
    }
}