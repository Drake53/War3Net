// ------------------------------------------------------------------------------
// <copyright file="VJassSyntaxTriviaList.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Linq;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassSyntaxTriviaList
    {
        public static readonly VJassSyntaxTriviaList Empty = new(ImmutableArray<ISyntaxTrivia>.Empty);

        public VJassSyntaxTriviaList(ImmutableArray<ISyntaxTrivia> trivia)
        {
            Trivia = trivia;
        }

        public ImmutableArray<ISyntaxTrivia> Trivia { get; }

        public bool Equals(VJassSyntaxTriviaList? other)
        {
            return other is not null
                && Trivia.SequenceEqual(other.Trivia);
        }

        public override string ToString() => string.Concat(Trivia);
    }
}