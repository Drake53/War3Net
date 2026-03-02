namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassArgumentListSyntax : JassSyntaxNode
    {
        public static readonly JassArgumentListSyntax Empty = new(
            new JassSyntaxToken(JassSyntaxKind.OpenParenToken, JassSymbol.OpenParen, JassSyntaxTriviaList.Empty),
            SeparatedSyntaxList<JassExpressionSyntax, JassSyntaxToken>.Empty,
            new JassSyntaxToken(JassSyntaxKind.CloseParenToken, JassSymbol.CloseParen, JassSyntaxTriviaList.Empty));

        internal JassArgumentListSyntax(
            JassSyntaxToken openParenToken,
            SeparatedSyntaxList<JassExpressionSyntax, JassSyntaxToken> arguments,
            JassSyntaxToken closeParenToken)
        {
            OpenParenToken = openParenToken;
            Arguments = arguments;
            CloseParenToken = closeParenToken;
        }

        public JassSyntaxToken OpenParenToken { get; }

        public SeparatedSyntaxList<JassExpressionSyntax, JassSyntaxToken> Arguments { get; }

        public JassSyntaxToken CloseParenToken { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.ArgumentList;

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassArgumentListSyntax argumentList
                && Arguments.IsEquivalentTo(argumentList.Arguments);
        }

        public override void WriteTo(TextWriter writer)
        {
            OpenParenToken.WriteTo(writer);
            Arguments.WriteTo(writer);
            CloseParenToken.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            return Arguments.Items;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return OpenParenToken;
            foreach (var child in Arguments.Separators)
            {
                yield return child;
            }

            yield return CloseParenToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return OpenParenToken;
            foreach (var child in Arguments.GetChildNodesAndTokens())
            {
                yield return child;
            }

            yield return CloseParenToken;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            return Arguments.GetDescendantNodes();
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return OpenParenToken;
            foreach (var descendant in Arguments.GetDescendantTokens())
            {
                yield return descendant;
            }

            yield return CloseParenToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return OpenParenToken;
            foreach (var descendant in Arguments.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }

            yield return CloseParenToken;
        }

        public override string ToString() => $"{OpenParenToken}{Arguments}{CloseParenToken}";

        public override JassSyntaxToken GetFirstToken() => OpenParenToken;

        public override JassSyntaxToken GetLastToken() => CloseParenToken;

        public override void Accept(IJassSyntaxVisitor visitor) => visitor.VisitArgumentList(this);

        public override TResult? Accept<TResult>(IJassSyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitArgumentList(this);

        public JassArgumentListSyntax Update(
            JassSyntaxToken openParenToken,
            SeparatedSyntaxList<JassExpressionSyntax, JassSyntaxToken> arguments,
            JassSyntaxToken closeParenToken)
        {
            if (ReferenceEquals(OpenParenToken, openParenToken) &&
                ReferenceEquals(Arguments, arguments) &&
                ReferenceEquals(CloseParenToken, closeParenToken))
            {
                return this;
            }

            ThrowHelper.ThrowIfInvalidToken(openParenToken, JassSyntaxKind.OpenParenToken);
            ThrowHelper.ThrowIfInvalidToken(closeParenToken, JassSyntaxKind.CloseParenToken);

            return new JassArgumentListSyntax(openParenToken, arguments, closeParenToken);
        }

        public JassArgumentListSyntax WithOpenParenToken(JassSyntaxToken openParenToken) => Update(openParenToken, Arguments, CloseParenToken);

        public JassArgumentListSyntax WithArguments(SeparatedSyntaxList<JassExpressionSyntax, JassSyntaxToken> arguments) => Update(OpenParenToken, arguments, CloseParenToken);

        public JassArgumentListSyntax WithCloseParenToken(JassSyntaxToken closeParenToken) => Update(OpenParenToken, Arguments, closeParenToken);

        protected internal override JassArgumentListSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassArgumentListSyntax(
                newToken,
                Arguments,
                CloseParenToken);
        }

        protected internal override JassArgumentListSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassArgumentListSyntax(
                OpenParenToken,
                Arguments,
                newToken);
        }
    }
}