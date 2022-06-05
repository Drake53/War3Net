// ------------------------------------------------------------------------------
// <copyright file="VJassScopedGlobalDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassScopedGlobalDeclarationSyntax : IScopedGlobalDeclarationSyntax
    {
        public VJassScopedGlobalDeclarationSyntax(
            VJassModifierListSyntax modifiers,
            IVariableDeclaratorSyntax declarator)
        {
            Modifiers = modifiers;
            Declarator = declarator;
        }

        public VJassModifierListSyntax Modifiers { get; }

        public IVariableDeclaratorSyntax Declarator { get; }

        public bool Equals(IScopedGlobalDeclarationSyntax? other)
        {
            return other is VJassScopedGlobalDeclarationSyntax globalDeclaration
                && Modifiers.Equals(globalDeclaration.Modifiers)
                && Declarator.Equals(globalDeclaration.Declarator);
        }

        public override string ToString() => $"{Modifiers} {Declarator}";
    }
}