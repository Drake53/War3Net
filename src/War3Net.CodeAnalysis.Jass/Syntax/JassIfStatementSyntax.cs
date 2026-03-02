namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassIfStatementSyntax : JassStatementSyntax
    {
        internal JassIfStatementSyntax(
            JassIfClauseSyntax ifClause,
            ImmutableArray<JassElseIfClauseSyntax> elseIfClauses,
            JassElseClauseSyntax? elseClause,
            JassSyntaxToken endIfToken)
        {
            IfClause = ifClause;
            ElseIfClauses = elseIfClauses;
            ElseClause = elseClause;
            EndIfToken = endIfToken;
        }

        public JassIfClauseSyntax IfClause { get; }

        public ImmutableArray<JassElseIfClauseSyntax> ElseIfClauses { get; }

        public JassElseClauseSyntax? ElseClause { get; }

        public JassSyntaxToken EndIfToken { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.IfStatement;

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassIfStatementSyntax ifStatement
                && IfClause.IsEquivalentTo(ifStatement.IfClause)
                && ElseIfClauses.IsEquivalentTo(ifStatement.ElseIfClauses)
                && ElseClause.NullableEquivalentTo(ifStatement.ElseClause);
        }

        public override void WriteTo(TextWriter writer)
        {
            IfClause.WriteTo(writer);
            ElseIfClauses.WriteTo(writer);
            ElseClause?.WriteTo(writer);
            EndIfToken.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return IfClause;

            foreach (var child in ElseIfClauses)
            {
                yield return child;
            }

            if (ElseClause is not null)
            {
                yield return ElseClause;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return EndIfToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return IfClause;

            foreach (var child in ElseIfClauses)
            {
                yield return child;
            }

            if (ElseClause is not null)
            {
                yield return ElseClause;
            }

            yield return EndIfToken;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield return IfClause;
            foreach (var descendant in IfClause.GetDescendantNodes())
            {
                yield return descendant;
            }

            foreach (var descendant in ElseIfClauses.GetDescendantNodes())
            {
                yield return descendant;
            }

            if (ElseClause is not null)
            {
                yield return ElseClause;
                foreach (var descendant in ElseClause.GetDescendantNodes())
                {
                    yield return descendant;
                }
            }
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            foreach (var descendant in IfClause.GetDescendantTokens())
            {
                yield return descendant;
            }

            foreach (var descendant in ElseIfClauses.GetDescendantTokens())
            {
                yield return descendant;
            }

            if (ElseClause is not null)
            {
                foreach (var descendant in ElseClause.GetDescendantTokens())
                {
                    yield return descendant;
                }
            }

            yield return EndIfToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return IfClause;
            foreach (var descendant in IfClause.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }

            foreach (var descendant in ElseIfClauses.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }

            if (ElseClause is not null)
            {
                yield return ElseClause;
                foreach (var descendant in ElseClause.GetDescendantNodesAndTokens())
                {
                    yield return descendant;
                }
            }

            yield return EndIfToken;
        }

        public override string ToString() => IfClause.ToString();

        public override JassSyntaxToken GetFirstToken() => IfClause.GetFirstToken();

        public override JassSyntaxToken GetLastToken() => EndIfToken;

        public override void Accept(IJassSyntaxVisitor visitor) => visitor.VisitIfStatement(this);

        public override TResult? Accept<TResult>(IJassSyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitIfStatement(this);

        public JassIfStatementSyntax Update(
            JassIfClauseSyntax ifClause,
            ImmutableArray<JassElseIfClauseSyntax> elseIfClauses,
            JassElseClauseSyntax? elseClause,
            JassSyntaxToken endIfToken)
        {
            if (ReferenceEquals(IfClause, ifClause) &&
                ElseIfClauses.SequenceEqual(elseIfClauses) &&
                ReferenceEquals(ElseClause, elseClause) &&
                ReferenceEquals(EndIfToken, endIfToken))
            {
                return this;
            }

            ThrowHelper.ThrowIfInvalidToken(endIfToken, JassSyntaxKind.EndIfKeyword);

            return new JassIfStatementSyntax(ifClause, elseIfClauses, elseClause, endIfToken);
        }

        public JassIfStatementSyntax WithIfClause(JassIfClauseSyntax ifClause) => Update(ifClause, ElseIfClauses, ElseClause, EndIfToken);

        public JassIfStatementSyntax WithElseIfClauses(ImmutableArray<JassElseIfClauseSyntax> elseIfClauses) => Update(IfClause, elseIfClauses, ElseClause, EndIfToken);

        public JassIfStatementSyntax WithElseClause(JassElseClauseSyntax? elseClause) => Update(IfClause, ElseIfClauses, elseClause, EndIfToken);

        public JassIfStatementSyntax WithEndIfToken(JassSyntaxToken endIfToken) => Update(IfClause, ElseIfClauses, ElseClause, endIfToken);

        protected internal override JassIfStatementSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassIfStatementSyntax(
                IfClause.ReplaceFirstToken(newToken),
                ElseIfClauses,
                ElseClause,
                EndIfToken);
        }

        protected internal override JassIfStatementSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassIfStatementSyntax(
                IfClause,
                ElseIfClauses,
                ElseClause,
                newToken);
        }
    }
}