namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassIfClauseSyntax : JassSyntaxNode
    {
        internal JassIfClauseSyntax(
            JassIfClauseDeclaratorSyntax ifClauseDeclarator,
            ImmutableArray<JassStatementSyntax> statements)
        {
            IfClauseDeclarator = ifClauseDeclarator;
            Statements = statements;
        }

        public JassIfClauseDeclaratorSyntax IfClauseDeclarator { get; }

        public ImmutableArray<JassStatementSyntax> Statements { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.IfClause;

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassIfClauseSyntax ifClause
                && IfClauseDeclarator.IsEquivalentTo(ifClause.IfClauseDeclarator)
                && Statements.IsEquivalentTo(ifClause.Statements);
        }

        public override void WriteTo(TextWriter writer)
        {
            IfClauseDeclarator.WriteTo(writer);
            Statements.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return IfClauseDeclarator;
            foreach (var child in Statements)
            {
                yield return child;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield break;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return IfClauseDeclarator;
            foreach (var child in Statements)
            {
                yield return child;
            }
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield return IfClauseDeclarator;
            foreach (var descendant in IfClauseDeclarator.GetDescendantNodes())
            {
                yield return descendant;
            }

            foreach (var descendant in Statements.GetDescendantNodes())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            foreach (var descendant in IfClauseDeclarator.GetDescendantTokens())
            {
                yield return descendant;
            }

            foreach (var descendant in Statements.GetDescendantTokens())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return IfClauseDeclarator;
            foreach (var descendant in IfClauseDeclarator.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }

            foreach (var descendant in Statements.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }
        }

        public override string ToString() => $"{IfClauseDeclarator} [...]";

        public override JassSyntaxToken GetFirstToken() => IfClauseDeclarator.GetFirstToken();

        public override JassSyntaxToken GetLastToken() => Statements.IsEmpty ? IfClauseDeclarator.GetLastToken() : Statements[^1].GetLastToken();

        public override void Accept(IJassSyntaxVisitor visitor) => visitor.VisitIfClause(this);

        public override TResult? Accept<TResult>(IJassSyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitIfClause(this);

        public JassIfClauseSyntax Update(
            JassIfClauseDeclaratorSyntax ifClauseDeclarator,
            ImmutableArray<JassStatementSyntax> statements)
        {
            if (ReferenceEquals(IfClauseDeclarator, ifClauseDeclarator) &&
                Statements.SequenceEqual(statements))
            {
                return this;
            }

            return new JassIfClauseSyntax(ifClauseDeclarator, statements);
        }

        public JassIfClauseSyntax WithIfClauseDeclarator(JassIfClauseDeclaratorSyntax ifClauseDeclarator) => Update(ifClauseDeclarator, Statements);

        public JassIfClauseSyntax WithStatements(ImmutableArray<JassStatementSyntax> statements) => Update(IfClauseDeclarator, statements);

        protected internal override JassIfClauseSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassIfClauseSyntax(
                IfClauseDeclarator.ReplaceFirstToken(newToken),
                Statements);
        }

        protected internal override JassIfClauseSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            if (!Statements.IsEmpty)
            {
                return new JassIfClauseSyntax(
                    IfClauseDeclarator,
                    Statements.ReplaceLastItem(Statements[^1].ReplaceLastToken(newToken)));
            }

            return new JassIfClauseSyntax(
                IfClauseDeclarator.ReplaceLastToken(newToken),
                Statements);
        }
    }
}