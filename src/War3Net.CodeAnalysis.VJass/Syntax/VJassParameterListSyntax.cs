// ------------------------------------------------------------------------------
// <copyright file="VJassParameterListSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Linq;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassParameterListSyntax : IEquatable<VJassParameterListSyntax>
    {
        public static readonly VJassParameterListSyntax Empty = new(ImmutableArray<VJassParameterSyntax>.Empty);

        public VJassParameterListSyntax(ImmutableArray<VJassParameterSyntax> parameters)
        {
            Parameters = parameters;
        }

        public ImmutableArray<VJassParameterSyntax> Parameters { get; }

        public bool Equals(VJassParameterListSyntax? other)
        {
            return other is not null
                && Parameters.SequenceEqual(other.Parameters);
        }

        public override string ToString() => Parameters.Any() ? string.Join($"{VJassSymbol.Comma} ", Parameters) : VJassKeyword.Nothing;
    }
}