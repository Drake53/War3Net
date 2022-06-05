// ------------------------------------------------------------------------------
// <copyright file="VJassWhitespaceTrivia.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassWhitespaceTrivia : ISyntaxTrivia
    {
        internal VJassWhitespaceTrivia(string whitespace)
        {
            Whitespace = whitespace;
        }

        public string Whitespace { get; }

        public bool Equals(ISyntaxTrivia? other)
        {
            return other is VJassWhitespaceTrivia whitespaceTrivia
                && string.Equals(Whitespace, whitespaceTrivia.Whitespace, StringComparison.Ordinal);
        }

        public override string ToString() => Whitespace;
    }
}