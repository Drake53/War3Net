// ------------------------------------------------------------------------------
// <copyright file="VJassStructDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassStructDeclarationSyntax : ITopLevelDeclarationSyntax, IScopedDeclarationSyntax
    {
        public VJassStructDeclarationSyntax(
            VJassIdentifierNameSyntax identifierName,
            VJassIdentifierNameSyntax? extends,
            VJassMemberDeclarationListSyntax memberDeclarations)
        {
            IdentifierName = identifierName;
            Extends = extends;
            MemberDeclarations = memberDeclarations;
        }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public VJassIdentifierNameSyntax? Extends { get; }

        public VJassMemberDeclarationListSyntax MemberDeclarations { get; }

        public bool Equals(ITopLevelDeclarationSyntax? other)
        {
            return other is VJassStructDeclarationSyntax structDeclaration
                && IdentifierName.Equals(structDeclaration.IdentifierName)
                && Extends.NullableEquals(structDeclaration.Extends)
                && MemberDeclarations.Equals(structDeclaration.MemberDeclarations);
        }

        public bool Equals(IScopedDeclarationSyntax? other)
        {
            return other is VJassStructDeclarationSyntax structDeclaration
                && IdentifierName.Equals(structDeclaration.IdentifierName)
                && Extends.NullableEquals(structDeclaration.Extends)
                && MemberDeclarations.Equals(structDeclaration.MemberDeclarations);
        }

        public override string ToString() => Extends is null
            ? $"{VJassKeyword.Struct} {IdentifierName} [{MemberDeclarations.Declarations.Length}]"
            : $"{VJassKeyword.Struct} {IdentifierName} {VJassKeyword.Extends} {Extends} [{MemberDeclarations.Declarations.Length}]";
    }
}