// ------------------------------------------------------------------------------
// <copyright file="VJassSyntaxToken.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassSyntaxToken
    {
        internal VJassSyntaxToken(
            SyntaxKind syntaxKind,
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
            SyntaxKind syntaxKind,
            string text,
            VJassSyntaxTriviaList trailingTrivia)
        {
            LeadingTrivia = leadingTrivia;
            SyntaxKind = syntaxKind;
            Text = text;
            TrailingTrivia = trailingTrivia;
        }

        public VJassSyntaxTriviaList LeadingTrivia { get; }

        public SyntaxKind SyntaxKind { get; }

        public string Text { get; }

        public VJassSyntaxTriviaList TrailingTrivia { get; }

        public void WriteTo(TextWriter writer)
        {
            LeadingTrivia.WriteTo(writer);
            writer.Write(Text);
            TrailingTrivia.WriteTo(writer);
        }

        public override string ToString() => Text;
    }
}