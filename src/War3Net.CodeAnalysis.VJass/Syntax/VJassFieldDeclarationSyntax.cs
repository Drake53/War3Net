// ------------------------------------------------------------------------------
// <copyright file="VJassFieldDeclarationSyntax.cs" company="Drake53">
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
    public class VJassFieldDeclarationSyntax : VJassMemberDeclarationSyntax
    {
        internal VJassFieldDeclarationSyntax(
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
            return other is VJassFieldDeclarationSyntax fieldDeclaration
                && Modifiers.IsEquivalentTo(fieldDeclaration.Modifiers)
                && Declarator.IsEquivalentTo(fieldDeclaration.Declarator);
        }

        public override string ToString() => $"{Modifiers.Join()}{Declarator}";

        public override VJassSyntaxToken GetFirstToken() => (Modifiers.IsEmpty ? (VJassSyntaxNode)Declarator : Modifiers[0]).GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => Declarator.GetLastToken();

        public override void WriteTo(TextWriter writer)
        {
            Modifiers.WriteTo(writer);
            Declarator.WriteTo(writer);
        }

        protected internal override VJassFieldDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            if (!Modifiers.IsEmpty)
            {
                return new VJassFieldDeclarationSyntax(
                    Modifiers.ReplaceFirstItem(Modifiers[0].ReplaceFirstToken(newToken)),
                    Declarator);
            }

            return new VJassFieldDeclarationSyntax(
                Modifiers,
                Declarator.ReplaceFirstToken(newToken));
        }

        protected internal override VJassFieldDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassFieldDeclarationSyntax(
                Modifiers,
                Declarator.ReplaceLastToken(newToken));
        }
    }
}