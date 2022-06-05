// ------------------------------------------------------------------------------
// <copyright file="VJassTypeSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassTypeSyntax : IEquatable<VJassTypeSyntax>
    {
        public VJassTypeSyntax(VJassIdentifierNameSyntax typeName)
        {
            TypeName = typeName;
        }

        public VJassIdentifierNameSyntax TypeName { get; }

        public bool Equals(VJassTypeSyntax? other)
        {
            return other is not null
                && TypeName.Equals(other.TypeName);
        }

        public override string ToString() => TypeName.ToString();
    }
}