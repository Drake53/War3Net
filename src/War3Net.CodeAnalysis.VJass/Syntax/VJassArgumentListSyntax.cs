// ------------------------------------------------------------------------------
// <copyright file="VJassArgumentListSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Linq;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassArgumentListSyntax : IEquatable<VJassArgumentListSyntax>
    {
        public VJassArgumentListSyntax(ImmutableArray<IExpressionSyntax> arguments)
        {
            Arguments = arguments;
        }

        public ImmutableArray<IExpressionSyntax> Arguments { get; init; }

        public bool Equals(VJassArgumentListSyntax? other)
        {
            return other is not null
                && Arguments.SequenceEqual(other.Arguments);
        }

        public override string ToString() => string.Join($"{VJassSymbol.Comma} ", Arguments);
    }
}