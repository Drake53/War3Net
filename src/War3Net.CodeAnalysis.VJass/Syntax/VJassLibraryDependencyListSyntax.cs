// ------------------------------------------------------------------------------
// <copyright file="VJassLibraryDependencyListSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Linq;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassLibraryDependencyListSyntax : IEquatable<VJassLibraryDependencyListSyntax>
    {
        public VJassLibraryDependencyListSyntax(
            ImmutableArray<VJassLibraryDependencySyntax> dependencies)
        {
            Dependencies = dependencies;
        }

        public ImmutableArray<VJassLibraryDependencySyntax> Dependencies { get; }

        public bool Equals(VJassLibraryDependencyListSyntax? other)
        {
            return other is not null
                && Dependencies.SequenceEqual(other.Dependencies);
        }

        public override string ToString() => $"{VJassKeyword.Requires} {string.Join($"{VJassSymbol.Comma} ", Dependencies)}";
    }
}