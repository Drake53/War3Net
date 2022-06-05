// ------------------------------------------------------------------------------
// <copyright file="VJassLibraryDependencySyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassLibraryDependencySyntax : IEquatable<VJassLibraryDependencySyntax>
    {
        public VJassLibraryDependencySyntax(VJassIdentifierNameSyntax identifierName)
        {
            IdentifierName = identifierName;
        }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public bool Equals(VJassLibraryDependencySyntax? other)
        {
            return other is not null
                && IdentifierName.Equals(other.IdentifierName);
        }

        public override string ToString() => IdentifierName.ToString();
    }
}