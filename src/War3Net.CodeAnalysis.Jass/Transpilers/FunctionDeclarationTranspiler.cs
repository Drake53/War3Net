// ------------------------------------------------------------------------------
// <copyright file="FunctionDeclarationTranspiler.cs" company="Drake53">
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
        private const string JassEntryPointName = "main";
        private const string CSharpEntryPointName = "Main";

        public static MethodDeclarationSyntax Transpile(this Syntax.FunctionDeclarationSyntax functionDeclarationNode)
        {
            _ = functionDeclarationNode ?? throw new ArgumentNullException(nameof(functionDeclarationNode));

            var functionDeclaration = SyntaxFactory.MethodDeclaration(
                default,
                default,
                functionDeclarationNode.ReturnTypeNode.Transpile(),
                null,
                functionDeclarationNode.IdentifierNode.ValueText == JassEntryPointName
                ? SyntaxFactory.Identifier(CSharpEntryPointName)
                : SyntaxFactory.Identifier(
                    SyntaxTriviaList.Empty,
                    Microsoft.CodeAnalysis.CSharp.SyntaxKind.IdentifierToken,
                    functionDeclarationNode.IdentifierNode.TranspileIdentifier(),
                    functionDeclarationNode.IdentifierNode.ValueText,
                    SyntaxTriviaList.Empty),
                null,
                SyntaxFactory.ParameterList(default),
                default,
                null,
                null);

            return functionDeclaration.AddParameterListParameters(functionDeclarationNode.ParameterListReferenceNode.Transpile().ToArray());
        }
    }
}