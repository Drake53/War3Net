namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public abstract class JassSyntaxNode
    {
        internal JassSyntaxNode()
        {
        }

        public abstract JassSyntaxKind SyntaxKind { get; }

        /// <summary>
        /// Determines if two nodes are the same, disregarding trivia differences.
        /// </summary>
        public abstract bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other);

        public abstract void WriteTo(TextWriter writer);

        public abstract IEnumerable<JassSyntaxNode> GetChildNodes();

        public abstract IEnumerable<JassSyntaxToken> GetChildTokens();

        public abstract IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens();

        public abstract IEnumerable<JassSyntaxNode> GetDescendantNodes();

        public abstract IEnumerable<JassSyntaxToken> GetDescendantTokens();

        public abstract IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens();

        public abstract JassSyntaxToken GetFirstToken();

        public abstract JassSyntaxToken GetLastToken();

        /// <summary>
        /// Accepts the visitor by calling the appropriate Visit method.
        /// </summary>
        /// <param name="visitor">The visitor to accept.</param>
        public abstract void Accept(IJassSyntaxVisitor visitor);

        /// <summary>
        /// Accepts the visitor by calling the appropriate Visit method and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="visitor">The visitor to accept.</param>
        /// <returns>The result of visiting this node.</returns>
        public abstract TResult? Accept<TResult>(IJassSyntaxVisitor<TResult> visitor);

        protected internal abstract JassSyntaxNode ReplaceFirstToken(JassSyntaxToken newToken);

        protected internal abstract JassSyntaxNode ReplaceLastToken(JassSyntaxToken newToken);

        public string ToFullString()
        {
            using var writer = new StringWriter();
            WriteTo(writer);
            return writer.ToString();
        }
    }
}