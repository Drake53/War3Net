// ------------------------------------------------------------------------------
// <copyright file="JassIdentifierNameSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassIdentifierNameSyntax : IEquatable<JassIdentifierNameSyntax>
    {
        public JassIdentifierNameSyntax(string name)
        {
            Name = name;
        }

        public string Name { get; init; }

        public virtual bool Equals(JassIdentifierNameSyntax? other)
        {
            return other is not null
                && string.Equals(Name, other.Name, StringComparison.Ordinal);
        }

        public override string ToString() => Name;
    }
}