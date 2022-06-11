// ------------------------------------------------------------------------------
// <copyright file="VJassModuleImplementationDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.VJass.Extensions;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassModuleImplementationDeclarationSyntax : VJassMemberDeclarationSyntax
    {
        internal VJassModuleImplementationDeclarationSyntax(
            VJassSyntaxToken implementToken,
            VJassSyntaxToken? optionalToken,
            VJassIdentifierNameSyntax identifierName)
        {
            ImplementToken = implementToken;
            OptionalToken = optionalToken;
            IdentifierName = identifierName;
        }

        public VJassSyntaxToken ImplementToken { get; }

        public VJassSyntaxToken? OptionalToken { get; }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassModuleImplementationDeclarationSyntax moduleImplementationDeclaration
                && OptionalToken.NullableEquals(moduleImplementationDeclaration.OptionalToken)
                && IdentifierName.IsEquivalentTo(moduleImplementationDeclaration.IdentifierName);
        }

        public override void WriteTo(TextWriter writer)
        {
            ImplementToken.WriteTo(writer);
            OptionalToken?.WriteTo(writer);
            IdentifierName.WriteTo(writer);
        }

        public override string ToString() => $"{ImplementToken} {OptionalToken.OptionalSuffixed()}{IdentifierName}";

        public override VJassSyntaxToken GetFirstToken() => ImplementToken;

        public override VJassSyntaxToken GetLastToken() => IdentifierName.GetLastToken();

        protected internal override VJassModuleImplementationDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassModuleImplementationDeclarationSyntax(
                newToken,
                OptionalToken,
                IdentifierName);
        }

        protected internal override VJassModuleImplementationDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassModuleImplementationDeclarationSyntax(
                ImplementToken,
                OptionalToken,
                IdentifierName.ReplaceLastToken(newToken));
        }
    }
}