namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassSyntaxToken
    {
        internal JassSyntaxToken(
            JassSyntaxKind syntaxKind,
            string text,
            JassSyntaxTriviaList trailingTrivia)
        {
            LeadingTrivia = JassSyntaxTriviaList.Empty;
            SyntaxKind = syntaxKind;
            Text = text;
            TrailingTrivia = trailingTrivia;
        }

        internal JassSyntaxToken(
            JassSyntaxTriviaList leadingTrivia,
            JassSyntaxKind syntaxKind,
            string text,
            JassSyntaxTriviaList trailingTrivia)
        {
            LeadingTrivia = leadingTrivia;
            SyntaxKind = syntaxKind;
            Text = text;
            TrailingTrivia = trailingTrivia;
        }

        internal JassSyntaxToken(
            JassSyntaxTriviaList leadingTrivia,
            JassSyntaxKind syntaxKind,
            string text,
            JassSyntaxTriviaList trailingTrivia,
            bool isMissing)
        {
            LeadingTrivia = leadingTrivia;
            SyntaxKind = syntaxKind;
            Text = text;
            TrailingTrivia = trailingTrivia;
            IsMissing = isMissing;
        }

        public JassSyntaxTriviaList LeadingTrivia { get; }

        public JassSyntaxKind SyntaxKind { get; }

        public string Text { get; }

        public JassSyntaxTriviaList TrailingTrivia { get; }

        /// <summary>
        /// Gets a value indicating whether this token was synthesized by the parser
        /// as a placeholder for a missing token. Missing tokens have empty <see cref="Text"/>.
        /// </summary>
        public bool IsMissing { get; }

        public bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxToken? other)
        {
            return other is not null
                && SyntaxKind == other.SyntaxKind
                && string.Equals(Text, other.Text, StringComparison.Ordinal);
        }

        public void WriteTo(TextWriter writer)
        {
            LeadingTrivia.WriteTo(writer);
            writer.Write(Text);
            TrailingTrivia.WriteTo(writer);
        }

        public override string ToString() => Text;

        public string ToFullString() => $"{LeadingTrivia}{Text}{TrailingTrivia}";
    }
}