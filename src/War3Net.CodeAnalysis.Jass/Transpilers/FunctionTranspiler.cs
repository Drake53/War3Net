// ------------------------------------------------------------------------------
// <copyright file="FunctionTranspiler.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Linq;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static MethodDeclarationSyntax Transpile(this Syntax.FunctionSyntax functionNode)
        {
            _ = functionNode ?? throw new ArgumentNullException(nameof(functionNode));

            // TODO: do smth with constant keyword?

            var functionDeclr = functionNode.FunctionDeclarationNode.Transpile();

            return functionDeclr.WithBody(
                SyntaxFactory.Block(
                    functionNode.LocalVariableListNode.Transpile().Concat(
                    functionNode.StatementListNode.Transpile())))
                .WithModifiers(
                new Microsoft.CodeAnalysis.SyntaxTokenList(
                    SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PublicKeyword),
                    SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.StaticKeyword)));
        }
    }
}