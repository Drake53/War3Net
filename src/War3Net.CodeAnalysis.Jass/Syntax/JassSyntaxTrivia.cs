// ------------------------------------------------------------------------------
// <copyright file="JassSyntaxTrivia.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassSyntaxTrivia : ISyntaxTrivia
    {
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