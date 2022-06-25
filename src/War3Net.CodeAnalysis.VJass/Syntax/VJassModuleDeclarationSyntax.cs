// ------------------------------------------------------------------------------
// <copyright file="VJassModuleDeclarationSyntax.cs" company="Drake53">
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
    public class VJassModuleDeclarationSyntax : VJassScopedDeclarationSyntax
    {
        internal VJassModuleDeclarationSyntax(
            ImmutableArray<VJassModifierSyntax> modifiers,
            VJassModuleDeclaratorSyntax declarator,
            ImmutableArray<VJassMemberDeclarationSyntax> memberDeclarations,
            VJassSyntaxToken endModuleToken)
        {
            Modifiers = modifiers;
            Declarator = declarator;
            MemberDeclarations = memberDeclarations;
            EndModuleToken = endModuleToken;
        }

        public ImmutableArray<VJassModifierSyntax> Modifiers { get; }

        public VJassModuleDeclaratorSyntax Declarator { get; }

        public ImmutableArray<VJassMemberDeclarationSyntax> MemberDeclarations { get; }

        public VJassSyntaxToken EndModuleToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassModuleDeclarationSyntax moduleDeclaration
                && Modifiers.IsEquivalentTo(moduleDeclaration.Modifiers)
                && Declarator.IsEquivalentTo(moduleDeclaration.Declarator)
                && MemberDeclarations.IsEquivalentTo(moduleDeclaration.MemberDeclarations);
        }

        public override void WriteTo(TextWriter writer)
        {
            Modifiers.WriteTo(writer);
            Declarator.WriteTo(writer);
            MemberDeclarations.WriteTo(writer);
            EndModuleToken.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            Modifiers.ProcessTo(writer, context);
            Declarator.ProcessTo(writer, context);
            MemberDeclarations.ProcessTo(writer, context);
            EndModuleToken.ProcessTo(writer, context);
        }

        public override string ToString() => $"{Modifiers.Join()}{Declarator} [...]";

        public override VJassSyntaxToken GetFirstToken() => (Modifiers.IsEmpty ? (VJassSyntaxNode)Declarator : Modifiers[0]).GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => EndModuleToken;

        protected internal override VJassModuleDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            if (!Modifiers.IsEmpty)
            {
                return new VJassModuleDeclarationSyntax(
                    Modifiers.ReplaceFirstItem(Modifiers[0].ReplaceFirstToken(newToken)),
                    Declarator,
                    MemberDeclarations,
                    EndModuleToken);
            }

            return new VJassModuleDeclarationSyntax(
                Modifiers,
                Declarator.ReplaceFirstToken(newToken),
                MemberDeclarations,
                EndModuleToken);
        }

        protected internal override VJassModuleDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassModuleDeclarationSyntax(
                Modifiers,
                Declarator,
                MemberDeclarations,
                newToken);
        }
    }
}