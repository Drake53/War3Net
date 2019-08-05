// ------------------------------------------------------------------------------
// <copyright file="TypeReferenceTranspiler.cs" company="Drake53">
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
        public static ParameterSyntax Transpile(this Syntax.TypeReferenceSyntax typeReferenceNode)
        {
            _ = typeReferenceNode ?? throw new ArgumentNullException(nameof(typeReferenceNode));

            return SyntaxFactory.Parameter(
                SyntaxFactory.Identifier(
                    SyntaxTriviaList.Empty,
                    Microsoft.CodeAnalysis.CSharp.SyntaxKind.IdentifierToken,
                    typeReferenceNode.TypeReferenceNameToken.TranspileIdentifier(),
                    typeReferenceNode.TypeReferenceNameToken.ValueText,
                    SyntaxTriviaList.Empty))
            .WithType(
                typeReferenceNode.TypeNameNode.Transpile(
                    // TODO: better solution for this hacky piece of shit
                    typeReferenceNode.TypeReferenceNameToken.ValueText == "func"
                    ? TokenTranspileFlags.ReturnBoolFunc
                    : (TokenTranspileFlags)0));
        }
    }
}