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
        public static JassLocalVariableDeclarationStatementSyntax LocalVariableDeclarationStatement(JassTypeSyntax type, string name)
        {
            return new JassLocalVariableDeclarationStatementSyntax(
                Token(JassSyntaxKind.LocalKeyword),
                VariableDeclarator(
                    type,
                    ParseIdentifierName(name)));
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

        public static JassLocalVariableDeclarationStatementSyntax LocalVariableDeclarationStatement(JassTypeSyntax type, string name, JassEqualsValueClauseSyntax value)
        {
            return new JassLocalVariableDeclarationStatementSyntax(
                Token(JassSyntaxKind.LocalKeyword),
                VariableDeclarator(
                    type,
                    ParseIdentifierName(name),
                    value));
        }

        public static JassLocalVariableDeclarationStatementSyntax LocalArrayDeclarationStatement(JassTypeSyntax type, string name)
        {
            return new JassLocalVariableDeclarationStatementSyntax(
                Token(JassSyntaxKind.LocalKeyword),
                ArrayDeclarator(
                    type,
                    ParseIdentifierName(name)));
        }
    }
}