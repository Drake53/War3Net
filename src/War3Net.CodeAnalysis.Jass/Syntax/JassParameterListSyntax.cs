// ------------------------------------------------------------------------------
// <copyright file="JassParameterListSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Linq;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassParameterListSyntax : IEquatable<JassParameterListSyntax>
    {
        public static readonly JassParameterListSyntax Empty = new JassParameterListSyntax(ImmutableArray<JassParameterSyntax>.Empty);

        internal JassParameterListSyntax(ImmutableArray<JassParameterSyntax> parameters)
        {
            Parameters = parameters;
        }

        public ImmutableArray<JassParameterSyntax> Parameters { get; init; }

        public bool Equals(JassParameterListSyntax? other)
        {
            return other is not null
                && Parameters.SequenceEqual(other.Parameters);
        }

        public override string ToString() => Parameters.Any() ? string.Join($"{JassSymbol.Comma} ", Parameters) : JassKeyword.Nothing;
    }
}