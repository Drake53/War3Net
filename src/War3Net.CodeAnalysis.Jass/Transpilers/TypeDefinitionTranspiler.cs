// ------------------------------------------------------------------------------
// <copyright file="TypeDefinitionTranspiler.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Collections.Generic;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.CSharp.Attributes;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static IEnumerable<MemberDeclarationSyntax> Transpile(this Syntax.TypeDefinitionSyntax typeDefinitionNode)
        {
            _ = typeDefinitionNode ?? throw new ArgumentNullException(nameof(typeDefinitionNode));

            var identifier = SyntaxFactory.Identifier(
                SyntaxTriviaList.Empty,
                SyntaxKind.IdentifierToken,
                typeDefinitionNode.NewTypeNameNode.TranspileIdentifier(),
                typeDefinitionNode.NewTypeNameNode.ValueText,
                SyntaxTriviaList.Empty); // todo: comment?

            if (TranspileToEnumHandler.IsTypeEnum(typeDefinitionNode.NewTypeNameNode.ValueText, out var convertFunction))
            {
                var enumDeclr = SyntaxFactory.EnumDeclaration(
                    new SyntaxList<AttributeListSyntax>(
                        SyntaxFactory.AttributeList(
                            default(SeparatedSyntaxList<AttributeSyntax>).Add(SyntaxFactory.Attribute(
                                SyntaxFactory.ParseName(nameof(ExplicitCastFromIntegerAttribute)),
                                SyntaxFactory.ParseAttributeArgumentList($"(nameof({convertFunction}))"))))),
                    new SyntaxTokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)),
                    identifier,
                    null,
                    default);

                TranspileToEnumHandler.AddEnum(enumDeclr);
                yield break;
            }

            var declr = SyntaxFactory.ClassDeclaration(
                default(SyntaxList<AttributeListSyntax>),
                new SyntaxTokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)),
                identifier,
                null,
                null,
                default(SyntaxList<TypeParameterConstraintClauseSyntax>),
                default(SyntaxList<MemberDeclarationSyntax>))
                .AddMembers(SyntaxFactory.ConstructorDeclaration(
                    default,
                    new SyntaxTokenList(SyntaxFactory.Token(SyntaxKind.InternalKeyword)),
                    identifier,
                    SyntaxFactory.ParameterList(),
                    null,
                    SyntaxFactory.Block()));

            if (typeDefinitionNode.BaseTypeNode.HandleIdentifierNode.TokenType == SyntaxTokenType.AlphanumericIdentifier)
            {
                yield return declr.AddBaseListTypes(SyntaxFactory.SimpleBaseType(typeDefinitionNode.BaseTypeNode.HandleIdentifierNode.TranspileType()));
            }
            else
            {
                yield return declr;
            }
        }
    }
}