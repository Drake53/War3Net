// ------------------------------------------------------------------------------
// <copyright file="VJassScopedGlobalVariableDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.VJass.Extensions;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassScopedGlobalVariableDeclarationSyntax : VJassScopedGlobalDeclarationSyntax
    {
        internal VJassScopedGlobalVariableDeclarationSyntax(
            ImmutableArray<VJassModifierSyntax> modifiers,
            VJassVariableOrArrayDeclaratorSyntax declarator)
        {
            Modifiers = modifiers;
            Declarator = declarator;
        }

        public ImmutableArray<VJassModifierSyntax> Modifiers { get; }

        public VJassVariableOrArrayDeclaratorSyntax Declarator { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassScopedGlobalVariableDeclarationSyntax scopedGlobalVariableDeclaration
                && Modifiers.IsEquivalentTo(scopedGlobalVariableDeclaration.Modifiers)
                && Declarator.IsEquivalentTo(scopedGlobalVariableDeclaration.Declarator);
        }

        public override void WriteTo(TextWriter writer)
        {
            Modifiers.WriteTo(writer);
            Declarator.WriteTo(writer);
        }

        public override string ToString() => $"{Modifiers.Join()}{Declarator}";

        public override VJassSyntaxToken GetFirstToken() => (Modifiers.IsEmpty ? (VJassSyntaxNode)Declarator : Modifiers[0]).GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => Declarator.GetLastToken();

        protected internal override VJassScopedGlobalVariableDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            if (!Modifiers.IsEmpty)
            {
                return new VJassScopedGlobalVariableDeclarationSyntax(
                    Modifiers.ReplaceFirstItem(Modifiers[0].ReplaceFirstToken(newToken)),
                    Declarator);
            }

            return new VJassScopedGlobalVariableDeclarationSyntax(
                Modifiers,
                Declarator.ReplaceFirstToken(newToken));
        }

        protected internal override VJassScopedGlobalVariableDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassScopedGlobalVariableDeclarationSyntax(
                Modifiers,
                Declarator.ReplaceLastToken(newToken));
        }
    }
}