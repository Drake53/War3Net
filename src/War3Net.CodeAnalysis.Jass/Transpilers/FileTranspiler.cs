// ------------------------------------------------------------------------------
// <copyright file="FileTranspiler.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static CompilationUnitSyntax Transpile(this Syntax.FileSyntax fileNode, string className, params (string Directive, bool Static)[] usingDirectives)
        {
            _ = fileNode ?? throw new ArgumentNullException(nameof(fileNode));

            var comment = fileNode.StartFileEmpty is null
                ? fileNode.StartFileLineDelimiter.Transpile()
                : default;

            var @class = SyntaxFactory.ClassDeclaration(
                default,
                new SyntaxTokenList(
                    SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PublicKeyword),
                    SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.StaticKeyword)),
                // SyntaxFactory.Token(
                SyntaxFactory.Identifier(
                    // SyntaxTriviaList.Create(comment),
                    SyntaxTriviaList.Empty,
                    Microsoft.CodeAnalysis.CSharp.SyntaxKind.IdentifierToken,
                    className,
                    string.Empty,
                    SyntaxTriviaList.Empty),
                null,
                null,
                default,
                new SyntaxList<MemberDeclarationSyntax>(
                    fileNode.DeclarationList.Transpile().Concat(
                        fileNode.FunctionList.Select(function => function.Transpile()))));

            var @namespace = SyntaxFactory.NamespaceDeclaration(
                SyntaxFactory.IdentifierName(
                    // SyntaxFactory.Token(
                    SyntaxFactory.Identifier(
                        // SyntaxTriviaList.Create(SyntaxFactory.Comment("//Transpiled from JASS code")),
                        SyntaxTriviaList.Empty,
                        Microsoft.CodeAnalysis.CSharp.SyntaxKind.IdentifierToken,
                        "JassTranspiledCode",
                        string.Empty,
                        SyntaxTriviaList.Empty)),
                default(SyntaxList<ExternAliasDirectiveSyntax>),
                default(SyntaxList<UsingDirectiveSyntax>),
                new SyntaxList<MemberDeclarationSyntax>(@class));

            var compilationUnit = SyntaxFactory.CompilationUnit(
                default,
                new SyntaxList<UsingDirectiveSyntax>(usingDirectives.Select(directive
                => directive.Static
                ? SyntaxFactory.UsingDirective(
                    SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.StaticKeyword),
                    null,
                    SyntaxFactory.ParseName(directive.Directive))
                : SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(directive.Directive)))),
                default,
                new SyntaxList<MemberDeclarationSyntax>(@namespace));

            return compilationUnit;
        }
    }
}