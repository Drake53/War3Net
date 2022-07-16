// ------------------------------------------------------------------------------
// <copyright file="VJassModifierSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassModifierSyntax : VJassSyntaxNode
    {
        public static readonly VJassModifierSyntax Private = new(new(new VJassSyntaxToken(VJassSyntaxKind.PrivateKeyword, VJassKeyword.Private, VJassSyntaxTriviaList.SingleSpace)));
        public static readonly VJassModifierSyntax Public = new(new(new VJassSyntaxToken(VJassSyntaxKind.PublicKeyword, VJassKeyword.Public, VJassSyntaxTriviaList.SingleSpace)));
        public static readonly VJassModifierSyntax Static = new(new(new VJassSyntaxToken(VJassSyntaxKind.StaticKeyword, VJassKeyword.Static, VJassSyntaxTriviaList.SingleSpace)));
        public static readonly VJassModifierSyntax ReadOnly = new(new(new VJassSyntaxToken(VJassSyntaxKind.ReadOnlyKeyword, VJassKeyword.ReadOnly, VJassSyntaxTriviaList.SingleSpace)));
        public static readonly VJassModifierSyntax Constant = new(new(new VJassSyntaxToken(VJassSyntaxKind.ConstantKeyword, VJassKeyword.Constant, VJassSyntaxTriviaList.SingleSpace)));

        internal VJassModifierSyntax(
            VJassIdentifierNameSyntax modifierName)
        {
            ModifierName = modifierName;
        }

        public VJassIdentifierNameSyntax ModifierName { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassModifierSyntax modifier
                && ModifierName.IsEquivalentTo(modifier.ModifierName);
        }

        public override void WriteTo(TextWriter writer)
        {
            ModifierName.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            ModifierName.ProcessTo(writer, context);
        }

        public override string ToString() => ModifierName.ToString();

        public override VJassSyntaxToken GetFirstToken() => ModifierName.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => ModifierName.GetLastToken();

        protected internal override VJassModifierSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassModifierSyntax(ModifierName.ReplaceFirstToken(newToken));
        }

        protected internal override VJassModifierSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassModifierSyntax(ModifierName.ReplaceLastToken(newToken));
        }
    }
}