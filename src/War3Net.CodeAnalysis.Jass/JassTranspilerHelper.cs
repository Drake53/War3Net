// ------------------------------------------------------------------------------
// <copyright file="JassTranspilerHelper.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.CSharp.Attributes;

namespace War3Net.CodeAnalysis.Jass
{
    public static class JassTranspilerHelper
    {
        public static CompilationUnitSyntax GetCompilationUnit(SyntaxList<UsingDirectiveSyntax> usingDirectives, params MemberDeclarationSyntax[] namespaceOrClassDeclarations)
        {
            return SyntaxFactory.CompilationUnit(
                default,
                usingDirectives,
                default,
                new SyntaxList<MemberDeclarationSyntax>(namespaceOrClassDeclarations));
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

        public static ClassDeclarationSyntax GetClassDeclaration(string identifier, IEnumerable<MemberDeclarationSyntax> members, bool applyNativeMemberAttributes)
        {
            var declarations = applyNativeMemberAttributes
                ? members.Select(declr => declr.AddAttributeByName(nameof(NativeLuaMemberAttribute)))
                : members;

            var classDeclaration = SyntaxFactory.ClassDeclaration(
                default,
                new SyntaxTokenList(
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                    SyntaxFactory.Token(SyntaxKind.StaticKeyword)),
                SyntaxFactory.Identifier(identifier),
                null,
                null,
                default,
                new SyntaxList<MemberDeclarationSyntax>(declarations));

            return applyNativeMemberAttributes
                ? classDeclaration.AddAttributeByName(nameof(NativeLuaMemberContainerAttribute))
                : classDeclaration;
        }

        private static T AddAttributeByName<T>(this T memberDeclaration, string attributeName)
            where T : MemberDeclarationSyntax
        {
            return (T)memberDeclaration.AddAttributeLists(
                SyntaxFactory.AttributeList(
                    default(SeparatedSyntaxList<AttributeSyntax>).Add(
                        SyntaxFactory.Attribute(
                            SyntaxFactory.ParseName(attributeName)))));
        }
    }
}