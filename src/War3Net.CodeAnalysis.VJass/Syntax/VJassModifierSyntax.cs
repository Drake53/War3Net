// ------------------------------------------------------------------------------
// <copyright file="VJassModifierSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassModifierSyntax
    {
        public VJassModifierSyntax(VJassIdentifierNameSyntax modifierName)
        {
            ModifierName = modifierName;
        }

        public VJassIdentifierNameSyntax ModifierName { get; }

        public bool Equals(VJassModifierSyntax? other)
        {
            return other is not null
                && ModifierName.Equals(other.ModifierName);
        }

        public override string ToString() => ModifierName.ToString();
    }
}