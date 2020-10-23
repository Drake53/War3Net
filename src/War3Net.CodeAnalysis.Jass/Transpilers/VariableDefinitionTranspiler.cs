// ------------------------------------------------------------------------------
// <copyright file="VariableDefinitionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static MemberDeclarationSyntax TranspileMember(this Syntax.VariableDefinitionSyntax variableDefinitionNode)
        {
            _ = variableDefinitionNode ?? throw new ArgumentNullException(nameof(variableDefinitionNode));

            var variableDefinition = SyntaxFactory.FieldDeclaration(
                default,
                new SyntaxTokenList(
                    SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PublicKeyword),
                    SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.StaticKeyword)),
                SyntaxFactory.VariableDeclaration(variableDefinitionNode.TypeNameNode.Transpile())
                .AddVariable(variableDefinitionNode));

            return variableDefinition;
        }

        public static LocalDeclarationStatementSyntax TranspileLocal(this Syntax.VariableDefinitionSyntax variableDefinitionNode)
        {
            _ = variableDefinitionNode ?? throw new ArgumentNullException(nameof(variableDefinitionNode));

            var declaration = SyntaxFactory.VariableDeclaration(variableDefinitionNode.TypeNameNode.Transpile())
                .AddVariable(variableDefinitionNode);

            return SyntaxFactory.LocalDeclarationStatement(declaration);
        }

        private static VariableDeclarationSyntax AddVariable(this VariableDeclarationSyntax declaration, Syntax.VariableDefinitionSyntax variableDefinitionNode)
        {
            return declaration.AddVariables(
                SyntaxFactory.VariableDeclarator(
                    SyntaxFactory.Identifier(
                        SyntaxTriviaList.Empty,
                        Microsoft.CodeAnalysis.CSharp.SyntaxKind.IdentifierToken,
                        variableDefinitionNode.IdentifierNameNode.TranspileIdentifier(),
                        variableDefinitionNode.IdentifierNameNode.ValueText,
                        SyntaxTriviaList.Empty),
                    null,
                    variableDefinitionNode.EqualsValueClause?.Transpile()
                    ?? SyntaxFactory.EqualsValueClause(
                        SyntaxFactory.LiteralExpression(
                            Microsoft.CodeAnalysis.CSharp.SyntaxKind.DefaultLiteralExpression))));
        }
    }

    public static partial class JassToLuaTranspiler
    {
        public static void TranspileGlobal(this Syntax.VariableDefinitionSyntax variableDefinitionNode, ref StringBuilder sb)
        {
            _ = variableDefinitionNode ?? throw new ArgumentNullException(nameof(variableDefinitionNode));

            variableDefinitionNode.Transpile(ref sb);
        }

        public static void TranspileLocal(this Syntax.VariableDefinitionSyntax variableDefinitionNode, ref StringBuilder sb)
        {
            _ = variableDefinitionNode ?? throw new ArgumentNullException(nameof(variableDefinitionNode));

            sb.Append("local ");
            variableDefinitionNode.Transpile(ref sb);
        }

        private static void Transpile(this Syntax.VariableDefinitionSyntax variableDefinitionNode, ref StringBuilder sb)
        {
            variableDefinitionNode.IdentifierNameNode.TranspileIdentifier(ref sb);
            if (variableDefinitionNode.EmptyEqualsValueClause is null)
            {
                variableDefinitionNode.EqualsValueClause.Transpile(ref sb);
            }
            else
            {
                sb.Append(" = nil");
            }
        }
    }
}