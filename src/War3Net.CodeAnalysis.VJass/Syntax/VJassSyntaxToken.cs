// ------------------------------------------------------------------------------
// <copyright file="VJassSyntaxToken.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassSyntaxToken
    {
        internal VJassSyntaxToken(
            VJassSyntaxKind syntaxKind,
            string text,
            VJassSyntaxTriviaList trailingTrivia)
        {
            LeadingTrivia = VJassSyntaxTriviaList.Empty;
            SyntaxKind = syntaxKind;
            Text = text;
            TrailingTrivia = trailingTrivia;
        }

        internal VJassSyntaxToken(
            VJassSyntaxTriviaList leadingTrivia,
            VJassSyntaxKind syntaxKind,
            string text,
            VJassSyntaxTriviaList trailingTrivia)
        {
            LeadingTrivia = leadingTrivia;
            SyntaxKind = syntaxKind;
            Text = text;
            TrailingTrivia = trailingTrivia;
        }

        public VJassSyntaxTriviaList LeadingTrivia { get; }

        public VJassSyntaxKind SyntaxKind { get; }

        public string Text { get; }

        public VJassSyntaxTriviaList TrailingTrivia { get; }

        public bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxToken? other)
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