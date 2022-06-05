// ------------------------------------------------------------------------------
// <copyright file="VJassParameterSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassParameterSyntax : IEquatable<VJassParameterSyntax>
    {
        public VJassParameterSyntax(
            VJassTypeSyntax type,
            VJassIdentifierNameSyntax identifierName)
        {
            Type = type;
            IdentifierName = identifierName;
        }

        public VJassTypeSyntax Type { get; }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public bool Equals(VJassParameterSyntax? other)
        {
            return other is not null
                && Type.Equals(other.Type)
                && IdentifierName.Equals(other.IdentifierName);
        }

        public override string ToString() => $"{Type} {IdentifierName}";
    }
}