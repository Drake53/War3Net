// ------------------------------------------------------------------------------
// <copyright file="GlobalConstantDeclarationTranspiler.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

using System;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static MemberDeclarationSyntax Transpile(this Syntax.GlobalConstantDeclarationSyntax globalConstantDeclarationNode)
        {
            _ = globalConstantDeclarationNode ?? throw new ArgumentNullException(nameof(globalConstantDeclarationNode));

            var globalConstantDeclaration = SyntaxFactory.FieldDeclaration(
                // default,
                // default,
                SyntaxFactory.VariableDeclaration(globalConstantDeclarationNode.TypeNameNode.Transpile(false))
                .AddVariables(
                    SyntaxFactory.VariableDeclarator(
                        SyntaxFactory.Identifier(
                            SyntaxTriviaList.Empty,
                            Microsoft.CodeAnalysis.CSharp.SyntaxKind.IdentifierToken,
                            globalConstantDeclarationNode.IdentifierNameNode.TranspileIdentifier(),
                            globalConstantDeclarationNode.IdentifierNameNode.ValueText,
                            SyntaxTriviaList.Empty),
                        null,
                        globalConstantDeclarationNode.EqualsValueClause.Transpile(out var isConstantExpression))));

            return isConstantExpression
                ? globalConstantDeclaration.WithModifiers(
                    new SyntaxTokenList(
                        SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.ConstKeyword)))
                : globalConstantDeclaration.WithModifiers(
                    new SyntaxTokenList(
                        SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.StaticKeyword),
                        SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.ReadOnlyKeyword)));
        }
    }
}