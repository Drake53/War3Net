// ------------------------------------------------------------------------------
// <copyright file="JassTypeDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassTypeDeclarationSyntax : ITopLevelDeclarationSyntax, IDeclarationLineSyntax
    {
        public JassTypeDeclarationSyntax(JassIdentifierNameSyntax identifierName, JassTypeSyntax baseType)
        {
            IdentifierName = identifierName;
            BaseType = baseType;
        }

        public JassIdentifierNameSyntax IdentifierName { get; init; }

        public JassTypeSyntax BaseType { get; init; }

        public bool Equals(ITopLevelDeclarationSyntax? other)
        {
            return other is JassTypeDeclarationSyntax typeDeclaration
                && IdentifierName.Equals(typeDeclaration.IdentifierName)
                && BaseType.Equals(typeDeclaration.BaseType);
        }

        public bool Equals(IDeclarationLineSyntax? other)
        {
            return other is JassTypeDeclarationSyntax typeDeclaration
                && IdentifierName.Equals(typeDeclaration.IdentifierName)
                && BaseType.Equals(typeDeclaration.BaseType);
        }

        public override string ToString() => $"{JassKeyword.Type} {IdentifierName} {JassKeyword.Extends} {BaseType}";
    }
}