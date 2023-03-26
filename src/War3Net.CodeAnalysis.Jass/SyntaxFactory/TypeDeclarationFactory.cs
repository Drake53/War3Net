// ------------------------------------------------------------------------------
// <copyright file="TypeDeclarationFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassTypeDeclarationSyntax TypeDeclaration(JassIdentifierNameSyntax identifierName, JassTypeSyntax baseType)
        {
            return new JassTypeDeclarationSyntax(
                Token(JassSyntaxKind.TypeKeyword),
                identifierName,
                Token(JassSyntaxKind.ExtendsKeyword),
                baseType);
        }

        public static JassTypeDeclarationSyntax TypeDeclaration(JassIdentifierNameSyntax identifierName, string baseType)
        {
            return new JassTypeDeclarationSyntax(
                Token(JassSyntaxKind.TypeKeyword),
                identifierName,
                Token(JassSyntaxKind.ExtendsKeyword),
                ParseTypeName(baseType));
        }

        public static JassTypeDeclarationSyntax TypeDeclaration(string name, JassTypeSyntax baseType)
        {
            return new JassTypeDeclarationSyntax(
                Token(JassSyntaxKind.TypeKeyword),
                ParseIdentifierName(name),
                Token(JassSyntaxKind.ExtendsKeyword),
                baseType);
        }

        public static JassTypeDeclarationSyntax TypeDeclaration(string name, string baseType)
        {
            return new JassTypeDeclarationSyntax(
                Token(JassSyntaxKind.TypeKeyword),
                ParseIdentifierName(name),
                Token(JassSyntaxKind.ExtendsKeyword),
                ParseTypeName(baseType));
        }
    }
}