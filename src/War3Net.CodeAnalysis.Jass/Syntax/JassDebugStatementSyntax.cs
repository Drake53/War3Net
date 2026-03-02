namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassDebugStatementSyntax : JassStatementSyntax
    {
        internal JassDebugStatementSyntax(
            JassSyntaxToken debugToken,
            JassStatementSyntax statement)
        {
            DebugToken = debugToken;
            Statement = statement;
        }

        public JassSyntaxToken DebugToken { get; }

        public JassStatementSyntax Statement { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxFacts.GetDebugStatementKind(Statement.SyntaxKind);

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassDebugStatementSyntax debugStatement
                && Statement.IsEquivalentTo(debugStatement.Statement);
        }

        public override void WriteTo(TextWriter writer)
        {
            DebugToken.WriteTo(writer);
            Statement.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return Statement;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return DebugToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return DebugToken;
            yield return Statement;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield return Statement;
            foreach (var descendant in Statement.GetDescendantNodes())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return DebugToken;

            foreach (var descendant in Statement.GetDescendantTokens())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return DebugToken;

            yield return Statement;
            foreach (var descendant in Statement.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }
        }

        public override string ToString() => $"{DebugToken} {Statement}";

        public override JassSyntaxToken GetFirstToken() => DebugToken;

        public override JassSyntaxToken GetLastToken() => Statement.GetLastToken();

        public override void Accept(IJassSyntaxVisitor visitor) => visitor.VisitDebugStatement(this);

        public override TResult? Accept<TResult>(IJassSyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitDebugStatement(this);

        public JassDebugStatementSyntax Update(
            JassSyntaxToken debugToken,
            JassStatementSyntax statement)
        {
            if (ReferenceEquals(DebugToken, debugToken) &&
                ReferenceEquals(Statement, statement))
            {
                return this;
            }

            ThrowHelper.ThrowIfInvalidToken(debugToken, JassSyntaxKind.DebugKeyword);
            ThrowHelper.ThrowIfInvalidDebugStatement(statement);

            return new JassDebugStatementSyntax(debugToken, statement);
        }

        public JassDebugStatementSyntax WithDebugToken(JassSyntaxToken debugToken) => Update(debugToken, Statement);

        public JassDebugStatementSyntax WithStatement(JassStatementSyntax statement) => Update(DebugToken, statement);

        protected internal override JassDebugStatementSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassDebugStatementSyntax(
                newToken,
                Statement);
        }

        protected internal override JassDebugStatementSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassDebugStatementSyntax(
                DebugToken,
                Statement.ReplaceLastToken(newToken));
        }
    }
}