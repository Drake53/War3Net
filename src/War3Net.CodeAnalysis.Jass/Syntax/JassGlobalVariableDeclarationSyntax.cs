namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassGlobalVariableDeclarationSyntax : JassGlobalDeclarationSyntax
    {
        internal JassGlobalVariableDeclarationSyntax(
            JassVariableOrArrayDeclaratorSyntax declarator)
        {
            Declarator = declarator;
        }

        public JassVariableOrArrayDeclaratorSyntax Declarator { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxFacts.GetGlobalDeclarationKind(Declarator.SyntaxKind);

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassGlobalVariableDeclarationSyntax globalVariableDeclaration
                && Declarator.IsEquivalentTo(globalVariableDeclaration.Declarator);
        }

        public override void WriteTo(TextWriter writer)
        {
            Declarator.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return Declarator;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield break;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return Declarator;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield return Declarator;
            foreach (var descendant in Declarator.GetDescendantNodes())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            return Declarator.GetDescendantTokens();
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return Declarator;
            foreach (var descendant in Declarator.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }
        }

        public override string ToString() => Declarator.ToString();

        public override JassSyntaxToken GetFirstToken() => Declarator.GetFirstToken();

        public override JassSyntaxToken GetLastToken() => Declarator.GetLastToken();

        public override void Accept(IJassSyntaxVisitor visitor) => visitor.VisitGlobalVariableDeclaration(this);

        public override TResult? Accept<TResult>(IJassSyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitGlobalVariableDeclaration(this);

        public JassGlobalVariableDeclarationSyntax WithDeclarator(JassVariableOrArrayDeclaratorSyntax declarator)
        {
            if (ReferenceEquals(Declarator, declarator))
            {
                return this;
            }

            return new JassGlobalVariableDeclarationSyntax(declarator);
        }

        protected internal override JassGlobalVariableDeclarationSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassGlobalVariableDeclarationSyntax(Declarator.ReplaceFirstToken(newToken));
        }

        protected internal override JassGlobalVariableDeclarationSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassGlobalVariableDeclarationSyntax(Declarator.ReplaceLastToken(newToken));
        }
    }
}