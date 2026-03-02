namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassDebugStatementSyntax DebugStatement(JassStatementSyntax statement)
        {
            var statementKind = JassSyntaxFacts.GetDebugStatementKind(statement.SyntaxKind);
            if (statementKind == JassSyntaxKind.None)
            {
                throw new ArgumentException($"'{statement.SyntaxKind}' is not a valid statement kind for debug statements.", nameof(statement));
            }

            return new JassDebugStatementSyntax(
                Token(JassSyntaxKind.DebugKeyword),
                statement);
        }

        public static JassDebugStatementSyntax DebugStatement(JassSyntaxToken debugToken, JassStatementSyntax statement)
        {
            ThrowHelper.ThrowIfInvalidToken(debugToken, JassSyntaxKind.DebugKeyword);

            var statementKind = JassSyntaxFacts.GetDebugStatementKind(statement.SyntaxKind);
            if (statementKind == JassSyntaxKind.None)
            {
                throw new ArgumentException($"'{statement.SyntaxKind}' is not a valid statement kind for debug statements.", nameof(statement));
            }

            return new JassDebugStatementSyntax(
                debugToken,
                statement);
        }
    }
}