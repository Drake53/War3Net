// ------------------------------------------------------------------------------
// <copyright file="VJassModifierListSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Linq;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassModifierListSyntax : IEquatable<VJassModifierListSyntax>
    {
        public static readonly VJassModifierListSyntax Empty = new(ImmutableArray<VJassModifierSyntax>.Empty);

        public VJassModifierListSyntax(
            ImmutableArray<VJassModifierSyntax> modifiers)
        {
            Modifiers = modifiers;
        }

        public ImmutableArray<VJassModifierSyntax> Modifiers { get; }

        public bool Equals(VJassModifierListSyntax? other)
        {
            return other is not null
                && Modifiers.SequenceEqual(other.Modifiers);
        }

        public override string ToString() => string.Join(' ', Modifiers);
    }
}