﻿// ------------------------------------------------------------------------------
// <copyright file="GlobalConstantDeclarationFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassGlobalConstantDeclarationSyntax GlobalConstantDeclaration(JassTypeSyntax type, JassIdentifierNameSyntax identifierName, JassEqualsValueClauseSyntax value)
        {
            return new JassGlobalConstantDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                type,
                identifierName,
                value);
        }

        public static JassGlobalConstantDeclarationSyntax GlobalConstantDeclaration(JassTypeSyntax type, JassIdentifierNameSyntax identifierName, JassExpressionSyntax value)
        {
            return new JassGlobalConstantDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                type,
                identifierName,
                EqualsValueClause(value));
        }

        public static JassGlobalConstantDeclarationSyntax GlobalConstantDeclaration(JassTypeSyntax type, string name, JassEqualsValueClauseSyntax value)
        {
            return new JassGlobalConstantDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                type,
                ParseIdentifierName(name),
                value);
        }

        public static JassGlobalConstantDeclarationSyntax GlobalConstantDeclaration(JassTypeSyntax type, string name, JassExpressionSyntax value)
        {
            return new JassGlobalConstantDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                type,
                ParseIdentifierName(name),
                EqualsValueClause(value));
        }

        public static JassGlobalConstantDeclarationSyntax GlobalConstantDeclaration(string type, JassIdentifierNameSyntax identifierName, JassEqualsValueClauseSyntax value)
        {
            return new JassGlobalConstantDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                ParseTypeName(type),
                identifierName,
                value);
        }

        public static JassGlobalConstantDeclarationSyntax GlobalConstantDeclaration(string type, JassIdentifierNameSyntax identifierName, JassExpressionSyntax value)
        {
            return new JassGlobalConstantDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                ParseTypeName(type),
                identifierName,
                EqualsValueClause(value));
        }

        public static JassGlobalConstantDeclarationSyntax GlobalConstantDeclaration(string type, string name, JassEqualsValueClauseSyntax value)
        {
            return new JassGlobalConstantDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                ParseTypeName(type),
                ParseIdentifierName(name),
                value);
        }

        public static JassGlobalConstantDeclarationSyntax GlobalConstantDeclaration(string type, string name, JassExpressionSyntax value)
        {
            return new JassGlobalConstantDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                ParseTypeName(type),
                ParseIdentifierName(name),
                EqualsValueClause(value));
        }

        public static JassGlobalConstantDeclarationSyntax GlobalConstantDeclaration(JassSyntaxToken constantToken, JassTypeSyntax type, JassIdentifierNameSyntax identifierName, JassEqualsValueClauseSyntax value)
        {
            ThrowHelper.ThrowIfInvalidToken(constantToken, JassSyntaxKind.ConstantKeyword);

            return new JassGlobalConstantDeclarationSyntax(
                constantToken,
                type,
                identifierName,
                value);
        }
    }
}