namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassIdentifierNameSyntax : JassTypeSyntax
    {
        internal JassIdentifierNameSyntax(
            JassSyntaxToken token)
        {
            Token = token;
        }

        public override JassSyntaxToken Token { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.IdentifierName;

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassIdentifierNameSyntax identifierName
                && Token.IsEquivalentTo(identifierName.Token);
        }

        public override void WriteTo(TextWriter writer)
        {
            Token.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield break;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return Token;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return Token;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield break;
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return Token;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return Token;
        }

        public override string ToString() => Token.ToString();

        public override JassSyntaxToken GetFirstToken() => Token;

        public override JassSyntaxToken GetLastToken() => Token;

        public override void Accept(IJassSyntaxVisitor visitor) => visitor.VisitIdentifierName(this);

        public override TResult? Accept<TResult>(IJassSyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitIdentifierName(this);

        public JassIdentifierNameSyntax WithToken(JassSyntaxToken token)
        {
            if (ReferenceEquals(Token, token))
            {
                return this;
            }

            ThrowHelper.ThrowIfInvalidToken(token, JassSyntaxKind.IdentifierToken);

            return new JassIdentifierNameSyntax(token);
        }

        protected internal override JassIdentifierNameSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassIdentifierNameSyntax(newToken);
        }

        protected internal override JassIdentifierNameSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassIdentifierNameSyntax(newToken);
        }
    }
}