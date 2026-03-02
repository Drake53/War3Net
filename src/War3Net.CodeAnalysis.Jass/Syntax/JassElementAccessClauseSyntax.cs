namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassElementAccessClauseSyntax : JassSyntaxNode
    {
        internal JassElementAccessClauseSyntax(
            JassSyntaxToken openBracketToken,
            JassExpressionSyntax argument,
            JassSyntaxToken closeBracketToken)
        {
            OpenBracketToken = openBracketToken;
            Argument = argument;
            CloseBracketToken = closeBracketToken;
        }

        public JassSyntaxToken OpenBracketToken { get; }

        public JassExpressionSyntax Argument { get; }

        public JassSyntaxToken CloseBracketToken { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.ElementAccessClause;

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassElementAccessClauseSyntax elementAccessClause
                && Argument.IsEquivalentTo(elementAccessClause.Argument);
        }

        public override void WriteTo(TextWriter writer)
        {
            OpenBracketToken.WriteTo(writer);
            Argument.WriteTo(writer);
            CloseBracketToken.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return Argument;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return OpenBracketToken;
            yield return CloseBracketToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return OpenBracketToken;
            yield return Argument;
            yield return CloseBracketToken;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield return Argument;
            foreach (var descendant in Argument.GetDescendantNodes())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return OpenBracketToken;

            foreach (var descendant in Argument.GetDescendantTokens())
            {
                yield return descendant;
            }

            yield return CloseBracketToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return OpenBracketToken;

            yield return Argument;
            foreach (var descendant in Argument.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }

            yield return CloseBracketToken;
        }

        public override string ToString() => $"{OpenBracketToken}{Argument}{CloseBracketToken}";

        public override JassSyntaxToken GetFirstToken() => OpenBracketToken;

        public override JassSyntaxToken GetLastToken() => CloseBracketToken;

        public override void Accept(IJassSyntaxVisitor visitor) => visitor.VisitElementAccessClause(this);

        public override TResult? Accept<TResult>(IJassSyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitElementAccessClause(this);

        public JassElementAccessClauseSyntax Update(
            JassSyntaxToken openBracketToken,
            JassExpressionSyntax argument,
            JassSyntaxToken closeBracketToken)
        {
            if (ReferenceEquals(OpenBracketToken, openBracketToken) &&
                ReferenceEquals(Argument, argument) &&
                ReferenceEquals(CloseBracketToken, closeBracketToken))
            {
                return this;
            }

            ThrowHelper.ThrowIfInvalidToken(openBracketToken, JassSyntaxKind.OpenBracketToken);
            ThrowHelper.ThrowIfInvalidToken(closeBracketToken, JassSyntaxKind.CloseBracketToken);

            return new JassElementAccessClauseSyntax(openBracketToken, argument, closeBracketToken);
        }

        public JassElementAccessClauseSyntax WithOpenBracketToken(JassSyntaxToken openBracketToken) => Update(openBracketToken, Argument, CloseBracketToken);

        public JassElementAccessClauseSyntax WithArgument(JassExpressionSyntax argument) => Update(OpenBracketToken, argument, CloseBracketToken);

        public JassElementAccessClauseSyntax WithCloseBracketToken(JassSyntaxToken closeBracketToken) => Update(OpenBracketToken, Argument, closeBracketToken);

        protected internal override JassElementAccessClauseSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassElementAccessClauseSyntax(
                newToken,
                Argument,
                CloseBracketToken);
        }

        protected internal override JassElementAccessClauseSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassElementAccessClauseSyntax(
                OpenBracketToken,
                Argument,
                newToken);
        }
    }
}