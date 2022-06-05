// ------------------------------------------------------------------------------
// <copyright file="VJassNewlineTrivia.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassNewlineTrivia : ISyntaxTrivia
    {
        internal VJassNewlineTrivia(string newline)
        {
            Newline = newline;
        }

        public string Newline { get; }

        public bool Equals(ISyntaxTrivia? other)
        {
            return other is VJassNewlineTrivia newlineTrivia
                && string.Equals(Newline, newlineTrivia.Newline, StringComparison.Ordinal);
        }

        public override string ToString() => Newline;
    }
}