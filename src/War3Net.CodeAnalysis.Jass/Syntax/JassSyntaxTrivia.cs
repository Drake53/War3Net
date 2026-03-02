using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassSyntaxTrivia
    {
        public static readonly JassSyntaxTrivia SingleSpace = new(JassSyntaxKind.WhitespaceTrivia, JassSymbol.Space);
        public static readonly JassSyntaxTrivia NewLine = new(JassSyntaxKind.NewLineTrivia, JassSymbol.CarriageReturnLineFeed);

        internal JassSyntaxTrivia(
            JassSyntaxKind syntaxKind,
            string text)
        {
            SyntaxKind = syntaxKind;
            Text = text;
        }

        public JassSyntaxKind SyntaxKind { get; }

        public string Text { get; }

        public bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxTrivia? other)
        {
            return other is not null
                && SyntaxKind == other.SyntaxKind
                && string.Equals(Text, other.Text, StringComparison.Ordinal);
        }

        public void WriteTo(TextWriter writer)
        {
            writer.Write(Text);
        }

        public override string ToString() => Text;
    }
}