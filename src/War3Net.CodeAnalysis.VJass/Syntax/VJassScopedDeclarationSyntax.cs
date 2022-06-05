// ------------------------------------------------------------------------------
// <copyright file="VJassScopedDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassScopedDeclarationSyntax : IScopedDeclarationSyntax
    {
        public VJassScopedDeclarationSyntax(
            VJassModifierListSyntax modifiers,
            IScopedDeclarationSyntax declaration)
        {
            Modifiers = modifiers;
            Declaration = declaration;
        }

        public VJassModifierListSyntax Modifiers { get; }

        public IScopedDeclarationSyntax Declaration { get; }

        public bool Equals(IScopedDeclarationSyntax? other)
        {
            return other is VJassScopedDeclarationSyntax scopedDeclaration
                && Modifiers.Equals(scopedDeclaration.Modifiers)
                && Declaration.Equals(scopedDeclaration.Declaration);
        }

        public override string ToString() => $"{Modifiers} {Declaration}";
    }
}