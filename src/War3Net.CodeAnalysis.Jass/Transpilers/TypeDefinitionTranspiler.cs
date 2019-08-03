// ------------------------------------------------------------------------------
// <copyright file="TypeDefinitionTranspiler.cs" company="Drake53">
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
        public static MemberDeclarationSyntax Transpile(this Syntax.TypeDefinitionSyntax typeDefinitionNode)
        {
            _ = typeDefinitionNode ?? throw new ArgumentNullException(nameof(typeDefinitionNode));

            return SyntaxFactory.ClassDeclaration(
                default(SyntaxList<AttributeListSyntax>),
                new SyntaxTokenList(SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PublicKeyword)),
                SyntaxFactory.Identifier(
                    SyntaxTriviaList.Empty,
                    Microsoft.CodeAnalysis.CSharp.SyntaxKind.IdentifierToken,
                    typeDefinitionNode.NewTypeNameNode.TranspileIdentifier(),
                    string.Empty,
                    SyntaxTriviaList.Empty), // todo: comment?
                null,
                null,
                default(SyntaxList<TypeParameterConstraintClauseSyntax>),
                default(SyntaxList<MemberDeclarationSyntax>))
                .AddBaseListTypes(SyntaxFactory.SimpleBaseType(typeDefinitionNode.BaseTypeNode.HandleIdentifierNode.TranspileType(false)));
        }
    }
}