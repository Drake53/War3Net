﻿// ------------------------------------------------------------------------------
// <copyright file="VJassIdentifierNameSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassIdentifierNameSyntax : IEquatable<VJassIdentifierNameSyntax>
    {
        public VJassIdentifierNameSyntax(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public bool Equals(VJassIdentifierNameSyntax? other)
        {
            return other is not null
                && string.Equals(Name, other.Name, StringComparison.Ordinal);
        }

        public override string ToString() => Name;
    }
}