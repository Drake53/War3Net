// ------------------------------------------------------------------------------
// <copyright file="ArrayDefinitionTranspiler.cs" company="Drake53">
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
        // Assume that this variable, defined in common.j, can be referenced directly.
        private const string JASS_ARRAY_LIMIT_CONSTANT_NAME = "JASS_MAX_ARRAY_SIZE";

        public static MemberDeclarationSyntax TranspileMember(this Syntax.ArrayDefinitionSyntax arrayDefinitionNode)
        {
            _ = arrayDefinitionNode ?? throw new ArgumentNullException(nameof(arrayDefinitionNode));

            var arrayDefinition = SyntaxFactory.FieldDeclaration(
                default,
                new SyntaxTokenList(
                    SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PublicKeyword),
                    SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.StaticKeyword)),
                SyntaxFactory.VariableDeclaration(arrayDefinitionNode.TypeNameNode.Transpile(true))
                .AddVariable(arrayDefinitionNode));

            return arrayDefinition;
        }

        public static LocalDeclarationStatementSyntax TranspileLocal(this Syntax.ArrayDefinitionSyntax arrayDefinitionNode)
        {
            _ = arrayDefinitionNode ?? throw new ArgumentNullException(nameof(arrayDefinitionNode));

            var declaration = SyntaxFactory.VariableDeclaration(
                arrayDefinitionNode.TypeNameNode.Transpile(true))
                .AddVariable(arrayDefinitionNode);

            return SyntaxFactory.LocalDeclarationStatement(declaration);
        }

        private static VariableDeclarationSyntax AddVariable(this VariableDeclarationSyntax declaration, Syntax.ArrayDefinitionSyntax arrayDefinitionNode)
        {
            return declaration.AddVariables(
                SyntaxFactory.VariableDeclarator(
                    SyntaxFactory.Identifier(
                        SyntaxTriviaList.Empty,
                        Microsoft.CodeAnalysis.CSharp.SyntaxKind.IdentifierToken,
                        arrayDefinitionNode.IdentifierNameNode.TranspileIdentifier(),
                        string.Empty,
                        SyntaxTriviaList.Empty),
                    null,
                    SyntaxFactory.EqualsValueClause(
                        // use SyntaxFactory.ArrayCreationExpression??
                        SyntaxFactory.ParseExpression($"new {arrayDefinitionNode.TypeNameNode.TypeNameToken.TranspileTypeString()}[{JASS_ARRAY_LIMIT_CONSTANT_NAME}]"))));
        }
    }
}