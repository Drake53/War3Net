// ------------------------------------------------------------------------------
// <copyright file="GlobalVariableDeclarationFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassGlobalVariableDeclarationSyntax GlobalVariableDeclaration(JassTypeSyntax type, string name)
        {
            return new JassGlobalVariableDeclarationSyntax(
                VariableDeclarator(
                    type,
                    ParseIdentifierName(name)));
        }

        public static JassGlobalVariableDeclarationSyntax GlobalVariableDeclaration(JassTypeSyntax type, string name, JassExpressionSyntax value)
        {
            return new JassGlobalVariableDeclarationSyntax(
                VariableDeclarator(
                    type,
                    ParseIdentifierName(name),
                    EqualsValueClause(value)));
        }

        public static JassGlobalVariableDeclarationSyntax GlobalVariableDeclaration(JassTypeSyntax type, string name, JassEqualsValueClauseSyntax value)
        {
            return new JassGlobalVariableDeclarationSyntax(
                VariableDeclarator(
                    type,
                    ParseIdentifierName(name),
                    value));
        }

        public static JassGlobalVariableDeclarationSyntax GlobalArrayDeclaration(JassTypeSyntax type, string name)
        {
            return new JassGlobalVariableDeclarationSyntax(
                ArrayDeclarator(
                    type,
                    ParseIdentifierName(name)));
        }

        public static JassGlobalVariableDeclarationSyntax GlobalDeclaration(JassVariableOrArrayDeclaratorSyntax declarator)
        {
            return new JassGlobalVariableDeclarationSyntax(declarator);
        }
    }
}