// ------------------------------------------------------------------------------
// <copyright file="JassTranspilerHelper.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.CodeAnalysis.Jass.Transpilers;

namespace War3Net.CodeAnalysis.Jass
{
    public static class JassTranspilerHelper
    {
        public static CompilationUnitSyntax GetCompilationUnit(SyntaxList<UsingDirectiveSyntax> usingDirectives, MemberDeclarationSyntax namespaceOrClassDeclaration)
        {
            return SyntaxFactory.CompilationUnit(
                default,
                usingDirectives,
                default,
                new SyntaxList<MemberDeclarationSyntax>(namespaceOrClassDeclaration));
        }

        public static MemberDeclarationSyntax GetNamespaceDeclaration(string identifier, ClassDeclarationSyntax classDeclaration)
        {
            return identifier is null
                ? (MemberDeclarationSyntax)classDeclaration
                : SyntaxFactory.NamespaceDeclaration(
                    SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(identifier)),
                    default,
                    default,
                    new SyntaxList<MemberDeclarationSyntax>(classDeclaration));
        }

        public static ClassDeclarationSyntax GetClassDeclaration(string identifier, FileSyntax fileNode)
        {
            return SyntaxFactory.ClassDeclaration(
                default,
                new SyntaxTokenList(
                    SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PublicKeyword),
                    SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.StaticKeyword)),
                SyntaxFactory.Identifier(identifier),
                null,
                null,
                default,
                new SyntaxList<MemberDeclarationSyntax>(fileNode.Transpile()));
        }
    }
}