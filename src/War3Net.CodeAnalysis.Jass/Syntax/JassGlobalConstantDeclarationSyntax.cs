namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassGlobalConstantDeclarationSyntax : JassGlobalDeclarationSyntax
    {
        internal JassGlobalConstantDeclarationSyntax(
            JassSyntaxToken constantToken,
            JassTypeSyntax type,
            JassIdentifierNameSyntax identifierName,
            JassEqualsValueClauseSyntax equalsValueClause)
        {
            ConstantToken = constantToken;
            Type = type;
            IdentifierName = identifierName;
            EqualsValueClause = equalsValueClause;
        }

        public JassSyntaxToken ConstantToken { get; }

        public JassTypeSyntax Type { get; }

        public JassIdentifierNameSyntax IdentifierName { get; }

        public JassEqualsValueClauseSyntax EqualsValueClause { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.GlobalConstantDeclaration;

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassGlobalConstantDeclarationSyntax globalConstantDeclaration
                && Type.IsEquivalentTo(globalConstantDeclaration.Type)
                && IdentifierName.IsEquivalentTo(globalConstantDeclaration.IdentifierName)
                && EqualsValueClause.IsEquivalentTo(globalConstantDeclaration.EqualsValueClause);
        }

        public override void WriteTo(TextWriter writer)
        {
            ConstantToken.WriteTo(writer);
            Type.WriteTo(writer);
            IdentifierName.WriteTo(writer);
            EqualsValueClause.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return Type;
            yield return IdentifierName;
            yield return EqualsValueClause;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return ConstantToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return ConstantToken;
            yield return Type;
            yield return IdentifierName;
            yield return EqualsValueClause;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield return Type;
            foreach (var descendant in Type.GetDescendantNodes())
            {
                yield return descendant;
            }

            yield return IdentifierName;
            foreach (var descendant in IdentifierName.GetDescendantNodes())
            {
                yield return descendant;
            }

            yield return EqualsValueClause;
            foreach (var descendant in EqualsValueClause.GetDescendantNodes())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return ConstantToken;

            foreach (var descendant in Type.GetDescendantTokens())
            {
                yield return descendant;
            }

            foreach (var descendant in IdentifierName.GetDescendantTokens())
            {
                yield return descendant;
            }

            foreach (var descendant in EqualsValueClause.GetDescendantTokens())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return ConstantToken;

            yield return Type;
            foreach (var descendant in Type.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }

            yield return IdentifierName;
            foreach (var descendant in IdentifierName.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }

            yield return EqualsValueClause;
            foreach (var descendant in EqualsValueClause.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }
        }

        public override string ToString() => $"{ConstantToken} {Type} {IdentifierName} {EqualsValueClause}";

        public override JassSyntaxToken GetFirstToken() => ConstantToken;

        public override JassSyntaxToken GetLastToken() => EqualsValueClause.GetLastToken();

        public override void Accept(IJassSyntaxVisitor visitor) => visitor.VisitGlobalConstantDeclaration(this);

        public override TResult? Accept<TResult>(IJassSyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitGlobalConstantDeclaration(this);

        public JassGlobalConstantDeclarationSyntax Update(
            JassSyntaxToken constantToken,
            JassTypeSyntax type,
            JassIdentifierNameSyntax identifierName,
            JassEqualsValueClauseSyntax equalsValueClause)
        {
            if (ReferenceEquals(ConstantToken, constantToken) &&
                ReferenceEquals(Type, type) &&
                ReferenceEquals(IdentifierName, identifierName) &&
                ReferenceEquals(EqualsValueClause, equalsValueClause))
            {
                return this;
            }

            ThrowHelper.ThrowIfInvalidToken(constantToken, JassSyntaxKind.ConstantKeyword);

            return new JassGlobalConstantDeclarationSyntax(constantToken, type, identifierName, equalsValueClause);
        }

        public JassGlobalConstantDeclarationSyntax WithConstantToken(JassSyntaxToken constantToken) => Update(constantToken, Type, IdentifierName, EqualsValueClause);

        public JassGlobalConstantDeclarationSyntax WithType(JassTypeSyntax type) => Update(ConstantToken, type, IdentifierName, EqualsValueClause);

        public JassGlobalConstantDeclarationSyntax WithIdentifierName(JassIdentifierNameSyntax identifierName) => Update(ConstantToken, Type, identifierName, EqualsValueClause);

        public JassGlobalConstantDeclarationSyntax WithEqualsValueClause(JassEqualsValueClauseSyntax equalsValueClause) => Update(ConstantToken, Type, IdentifierName, equalsValueClause);

        protected internal override JassGlobalConstantDeclarationSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassGlobalConstantDeclarationSyntax(
                newToken,
                Type,
                IdentifierName,
                EqualsValueClause);
        }

        protected internal override JassGlobalConstantDeclarationSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassGlobalConstantDeclarationSyntax(
                ConstantToken,
                Type,
                IdentifierName,
                EqualsValueClause.ReplaceLastToken(newToken));
        }
    }
}