// ------------------------------------------------------------------------------
// <copyright file="LocalVariableDeclarationStatementFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassLocalVariableDeclarationStatementSyntax LocalVariableDeclarationStatement(JassVariableDeclaratorSyntax declarator)
        {
            return new JassLocalVariableDeclarationStatementSyntax(
                Token(JassSyntaxKind.LocalKeyword),
                declarator);
        }

        public static JassLocalVariableDeclarationStatementSyntax LocalVariableDeclarationStatement(JassTypeSyntax type, JassIdentifierNameSyntax identifierName)
        {
            return new JassLocalVariableDeclarationStatementSyntax(
                Token(JassSyntaxKind.LocalKeyword),
                VariableDeclarator(
                    type,
                    identifierName));
        }

        public static JassLocalVariableDeclarationStatementSyntax LocalVariableDeclarationStatement(JassTypeSyntax type, string name)
        {
            return new JassLocalVariableDeclarationStatementSyntax(
                Token(JassSyntaxKind.LocalKeyword),
                VariableDeclarator(
                    type,
                    ParseIdentifierName(name)));
        }

        public static JassLocalVariableDeclarationStatementSyntax LocalVariableDeclarationStatement(string type, JassIdentifierNameSyntax identifierName)
        {
            return new JassLocalVariableDeclarationStatementSyntax(
                Token(JassSyntaxKind.LocalKeyword),
                VariableDeclarator(
                    ParseTypeName(type),
                    identifierName));
        }

        public static JassLocalVariableDeclarationStatementSyntax LocalVariableDeclarationStatement(string type, string name)
        {
            return new JassLocalVariableDeclarationStatementSyntax(
                Token(JassSyntaxKind.LocalKeyword),
                VariableDeclarator(
                    ParseTypeName(type),
                    ParseIdentifierName(name)));
        }

        public static JassLocalVariableDeclarationStatementSyntax LocalVariableDeclarationStatement(JassTypeSyntax type, JassIdentifierNameSyntax identifierName, JassEqualsValueClauseSyntax value)
        {
            return new JassLocalVariableDeclarationStatementSyntax(
                Token(JassSyntaxKind.LocalKeyword),
                VariableDeclarator(
                    type,
                    identifierName,
                    value));
        }

        public static JassLocalVariableDeclarationStatementSyntax LocalVariableDeclarationStatement(JassTypeSyntax type, JassIdentifierNameSyntax identifierName, JassExpressionSyntax value)
        {
            return new JassLocalVariableDeclarationStatementSyntax(
                Token(JassSyntaxKind.LocalKeyword),
                VariableDeclarator(
                    type,
                    identifierName,
                    EqualsValueClause(value)));
        }

        public static JassLocalVariableDeclarationStatementSyntax LocalVariableDeclarationStatement(JassTypeSyntax type, string name, JassEqualsValueClauseSyntax value)
        {
            return new JassLocalVariableDeclarationStatementSyntax(
                Token(JassSyntaxKind.LocalKeyword),
                VariableDeclarator(
                    type,
                    ParseIdentifierName(name),
                    value));
        }

        public static JassLocalVariableDeclarationStatementSyntax LocalVariableDeclarationStatement(JassTypeSyntax type, string name, JassExpressionSyntax value)
        {
            return new JassLocalVariableDeclarationStatementSyntax(
                Token(JassSyntaxKind.LocalKeyword),
                VariableDeclarator(
                    type,
                    ParseIdentifierName(name),
                    EqualsValueClause(value)));
        }

        public static JassLocalVariableDeclarationStatementSyntax LocalVariableDeclarationStatement(string type, JassIdentifierNameSyntax identifierName, JassEqualsValueClauseSyntax value)
        {
            return new JassLocalVariableDeclarationStatementSyntax(
                Token(JassSyntaxKind.LocalKeyword),
                VariableDeclarator(
                    ParseTypeName(type),
                    identifierName,
                    value));
        }

        public static JassLocalVariableDeclarationStatementSyntax LocalVariableDeclarationStatement(string type, JassIdentifierNameSyntax identifierName, JassExpressionSyntax value)
        {
            return new JassLocalVariableDeclarationStatementSyntax(
                Token(JassSyntaxKind.LocalKeyword),
                VariableDeclarator(
                    ParseTypeName(type),
                    identifierName,
                    EqualsValueClause(value)));
        }

        public static JassLocalVariableDeclarationStatementSyntax LocalVariableDeclarationStatement(string type, string name, JassEqualsValueClauseSyntax value)
        {
            return new JassLocalVariableDeclarationStatementSyntax(
                Token(JassSyntaxKind.LocalKeyword),
                VariableDeclarator(
                    ParseTypeName(type),
                    ParseIdentifierName(name),
                    value));
        }

        public static JassLocalVariableDeclarationStatementSyntax LocalVariableDeclarationStatement(string type, string name, JassExpressionSyntax value)
        {
            return new JassLocalVariableDeclarationStatementSyntax(
                Token(JassSyntaxKind.LocalKeyword),
                VariableDeclarator(
                    ParseTypeName(type),
                    ParseIdentifierName(name),
                    EqualsValueClause(value)));
        }

        public static JassLocalVariableDeclarationStatementSyntax LocalArrayDeclarationStatement(JassArrayDeclaratorSyntax declarator)
        {
            return new JassLocalVariableDeclarationStatementSyntax(
                Token(JassSyntaxKind.LocalKeyword),
                declarator);
        }

        public static JassLocalVariableDeclarationStatementSyntax LocalArrayDeclarationStatement(JassTypeSyntax type, JassIdentifierNameSyntax identifierName)
        {
            return new JassLocalVariableDeclarationStatementSyntax(
                Token(JassSyntaxKind.LocalKeyword),
                ArrayDeclarator(
                    type,
                    identifierName));
        }

        public static JassLocalVariableDeclarationStatementSyntax LocalArrayDeclarationStatement(JassTypeSyntax type, string name)
        {
            return new JassLocalVariableDeclarationStatementSyntax(
                Token(JassSyntaxKind.LocalKeyword),
                ArrayDeclarator(
                    type,
                    ParseIdentifierName(name)));
        }

        public static JassLocalVariableDeclarationStatementSyntax LocalArrayDeclarationStatement(string type, JassIdentifierNameSyntax identifierName)
        {
            return new JassLocalVariableDeclarationStatementSyntax(
                Token(JassSyntaxKind.LocalKeyword),
                ArrayDeclarator(
                    ParseTypeName(type),
                    identifierName));
        }

        public static JassLocalVariableDeclarationStatementSyntax LocalArrayDeclarationStatement(string type, string name)
        {
            return new JassLocalVariableDeclarationStatementSyntax(
                Token(JassSyntaxKind.LocalKeyword),
                ArrayDeclarator(
                    ParseTypeName(type),
                    ParseIdentifierName(name)));
        }
    }
}