// ------------------------------------------------------------------------------
// <copyright file="VJassTypeDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassTypeDeclarationSyntax : ITopLevelDeclarationSyntax, IScopedDeclarationSyntax
    {
        public VJassTypeDeclarationSyntax(VJassIdentifierNameSyntax identifierName, VJassTypeSyntax baseType)
        {
            IdentifierName = identifierName;
            BaseType = baseType;
        }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public VJassTypeSyntax BaseType { get; }

        public bool Equals(ITopLevelDeclarationSyntax? other)
        {
            return other is VJassTypeDeclarationSyntax typeDeclaration
                && IdentifierName.Equals(typeDeclaration.IdentifierName)
                && BaseType.Equals(typeDeclaration.BaseType);
        }

        public bool Equals(IScopedDeclarationSyntax? other)
        {
            return other is VJassTypeDeclarationSyntax typeDeclaration
                && IdentifierName.Equals(typeDeclaration.IdentifierName)
                && BaseType.Equals(typeDeclaration.BaseType);
        }

        public override string ToString() => $"{VJassKeyword.Type} {IdentifierName} {VJassKeyword.Extends} {BaseType}";
    }
}