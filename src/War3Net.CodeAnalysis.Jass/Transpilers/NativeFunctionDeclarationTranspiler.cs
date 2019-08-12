// ------------------------------------------------------------------------------
// <copyright file="NativeFunctionDeclarationTranspiler.cs" company="Drake53">
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
        public static MemberDeclarationSyntax Transpile(this Syntax.NativeFunctionDeclarationSyntax nativeFunctionDeclarationNode)
        {
            _ = nativeFunctionDeclarationNode ?? throw new ArgumentNullException(nameof(nativeFunctionDeclarationNode));

            return nativeFunctionDeclarationNode.FunctionDeclarationNode.Transpile()
                .WithModifiers(
                    new SyntaxTokenList(
                        SyntaxFactory.Token(TranspileToEnumHandler.IsFunctionEnumConverter(nativeFunctionDeclarationNode.FunctionDeclarationNode.IdentifierNode.ValueText, out _)
                            ? TranspileToEnumHandler.EnumCastFunctionAccessModifier
                            : SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(SyntaxKind.StaticKeyword),
                        SyntaxFactory.Token(SyntaxKind.ExternKeyword)))
                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        }
    }
}