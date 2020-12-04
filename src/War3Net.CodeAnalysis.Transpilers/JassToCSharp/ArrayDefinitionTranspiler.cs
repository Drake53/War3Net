// ------------------------------------------------------------------------------
// <copyright file="ArrayDefinitionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        // Assume that this variable, defined in common.j, can be referenced directly.
        private const string JASS_ARRAY_LIMIT_CONSTANT_NAME = "JASS_MAX_ARRAY_SIZE";

        public static MemberDeclarationSyntax TranspileMember(this Jass.Syntax.ArrayDefinitionSyntax arrayDefinitionNode)
        {
            _ = arrayDefinitionNode ?? throw new ArgumentNullException(nameof(arrayDefinitionNode));

            var arrayDefinition = SyntaxFactory.FieldDeclaration(
                default,
                new SyntaxTokenList(
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                    SyntaxFactory.Token(SyntaxKind.StaticKeyword)),
                SyntaxFactory.VariableDeclaration(arrayDefinitionNode.TypeNameNode.Transpile(TokenTranspileFlags.ReturnArray))
                .AddVariable(arrayDefinitionNode));

            return arrayDefinition;
        }

        public static LocalDeclarationStatementSyntax TranspileLocal(this Jass.Syntax.ArrayDefinitionSyntax arrayDefinitionNode)
        {
            _ = arrayDefinitionNode ?? throw new ArgumentNullException(nameof(arrayDefinitionNode));

            var declaration = SyntaxFactory.VariableDeclaration(
                arrayDefinitionNode.TypeNameNode.Transpile(TokenTranspileFlags.ReturnArray))
                .AddVariable(arrayDefinitionNode);

            return SyntaxFactory.LocalDeclarationStatement(declaration);
        }

        private static VariableDeclarationSyntax AddVariable(this VariableDeclarationSyntax declaration, Jass.Syntax.ArrayDefinitionSyntax arrayDefinitionNode)
        {
            return declaration.AddVariables(
                SyntaxFactory.VariableDeclarator(
                    SyntaxFactory.Identifier(
                        SyntaxTriviaList.Empty,
                        SyntaxKind.IdentifierToken,
                        arrayDefinitionNode.IdentifierNameNode.TranspileIdentifier(),
                        arrayDefinitionNode.IdentifierNameNode.ValueText,
                        SyntaxTriviaList.Empty),
                    null,
                    SyntaxFactory.EqualsValueClause(
                        // use SyntaxFactory.ArrayCreationExpression??
                        SyntaxFactory.ParseExpression($"new {arrayDefinitionNode.TypeNameNode.TypeNameToken.TranspileTypeString()}[{JASS_ARRAY_LIMIT_CONSTANT_NAME}]"))));
        }
    }
}