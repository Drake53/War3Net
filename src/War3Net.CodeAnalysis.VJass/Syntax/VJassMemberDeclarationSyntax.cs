// ------------------------------------------------------------------------------
// <copyright file="VJassMemberDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassMemberDeclarationSyntax : IMemberDeclarationSyntax
    {
        public VJassMemberDeclarationSyntax(
            VJassModifierListSyntax modifiers,
            IMemberDeclarationSyntax declaration)
        {
            Modifiers = modifiers;
            Declaration = declaration;
        }

        public VJassModifierListSyntax Modifiers { get; }

        public IMemberDeclarationSyntax Declaration { get; }

        public bool Equals(IMemberDeclarationSyntax? other)
        {
            return other is VJassMemberDeclarationSyntax memberDeclaration
                && Modifiers.Equals(memberDeclaration.Modifiers)
                && Declaration.Equals(memberDeclaration.Declaration);
        }

        public override string ToString() => $"{Modifiers} {Declaration}";
    }
}