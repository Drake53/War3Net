// ------------------------------------------------------------------------------
// <copyright file="TypeDefinitionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

            if (TranspileToEnumHandler.IsTypeEnum(typeDefinitionNode.NewTypeNameNode.ValueText, out _))
            {
                var enumDeclr = SyntaxFactory.EnumDeclaration(
                    new SyntaxList<AttributeListSyntax>(SyntaxFactory.AttributeList()),
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

    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this Syntax.TypeDefinitionSyntax typeDefinitionNode, ref StringBuilder sb)
        {
            // _ = typeDefinitionNode ?? throw new ArgumentNullException(nameof(typeDefinitionNode));

            throw new NotSupportedException();
        }
    }
}