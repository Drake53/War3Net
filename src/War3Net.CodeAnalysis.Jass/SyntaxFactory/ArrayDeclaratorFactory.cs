// ------------------------------------------------------------------------------
// <copyright file="ArrayDeclaratorFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassArrayDeclaratorSyntax ArrayDeclarator(JassTypeSyntax type, JassIdentifierNameSyntax identifierName)
        {
            return new JassArrayDeclaratorSyntax(
                type,
                Token(JassSyntaxKind.ArrayKeyword),
                identifierName);
        }

        public static JassArrayDeclaratorSyntax ArrayDeclarator(JassTypeSyntax type, string name)
        {
            return new JassArrayDeclaratorSyntax(
                type,
                Token(JassSyntaxKind.ArrayKeyword),
                ParseIdentifierName(name));
        }

        public static JassArrayDeclaratorSyntax ArrayDeclarator(string type, JassIdentifierNameSyntax identifierName)
        {
            return new JassArrayDeclaratorSyntax(
                ParseTypeName(type),
                Token(JassSyntaxKind.ArrayKeyword),
                identifierName);
        }

        public static JassArrayDeclaratorSyntax ArrayDeclarator(string type, string name)
        {
            return new JassArrayDeclaratorSyntax(
                ParseTypeName(type),
                Token(JassSyntaxKind.ArrayKeyword),
                ParseIdentifierName(name));
        }
    }
}