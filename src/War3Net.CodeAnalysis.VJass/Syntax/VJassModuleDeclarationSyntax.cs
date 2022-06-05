// ------------------------------------------------------------------------------
// <copyright file="VJassModuleDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassModuleDeclarationSyntax : ITopLevelDeclarationSyntax, IScopedDeclarationSyntax
    {
        public VJassModuleDeclarationSyntax(
            VJassIdentifierNameSyntax identifierName,
            VJassIdentifierNameSyntax? initializer,
            VJassMemberDeclarationListSyntax memberDeclarations)
        {
            IdentifierName = identifierName;
            Initializer = initializer;
            MemberDeclarations = memberDeclarations;
        }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public VJassIdentifierNameSyntax? Initializer { get; }

        public VJassMemberDeclarationListSyntax MemberDeclarations { get; }

        public bool Equals(ITopLevelDeclarationSyntax? other)
        {
            return other is VJassModuleDeclarationSyntax moduleDeclaration
                && IdentifierName.Equals(moduleDeclaration.IdentifierName)
                && Initializer.NullableEquals(moduleDeclaration.Initializer)
                && MemberDeclarations.Equals(moduleDeclaration.MemberDeclarations);
        }

        public bool Equals(IScopedDeclarationSyntax? other)
        {
            return other is VJassModuleDeclarationSyntax moduleDeclaration
                && IdentifierName.Equals(moduleDeclaration.IdentifierName)
                && Initializer.NullableEquals(moduleDeclaration.Initializer)
                && MemberDeclarations.Equals(moduleDeclaration.MemberDeclarations);
        }

        public override string ToString() => Initializer is null
            ? $"{VJassKeyword.Module} {IdentifierName} [{MemberDeclarations.Declarations.Length}]"
            : $"{VJassKeyword.Module} {IdentifierName} {VJassKeyword.Initializer} {Initializer} [{MemberDeclarations.Declarations.Length}]";
    }
}