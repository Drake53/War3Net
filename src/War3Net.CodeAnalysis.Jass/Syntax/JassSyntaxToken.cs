// ------------------------------------------------------------------------------
// <copyright file="JassSyntaxToken.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

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

        public JassSyntaxTriviaList LeadingTrivia { get; }

        public JassSyntaxKind SyntaxKind { get; }

        public string Text { get; }

        public JassSyntaxTriviaList TrailingTrivia { get; }

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
    }
}