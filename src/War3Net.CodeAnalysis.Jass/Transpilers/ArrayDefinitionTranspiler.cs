// ------------------------------------------------------------------------------
// <copyright file="ArrayDefinitionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

using System;
using System.ComponentModel;
using System.Text;

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
                SyntaxFactory.VariableDeclaration(arrayDefinitionNode.TypeNameNode.Transpile(TokenTranspileFlags.ReturnArray))
                .AddVariable(arrayDefinitionNode));

            return arrayDefinition;
        }

        public static LocalDeclarationStatementSyntax TranspileLocal(this Syntax.ArrayDefinitionSyntax arrayDefinitionNode)
        {
            _ = arrayDefinitionNode ?? throw new ArgumentNullException(nameof(arrayDefinitionNode));

            var declaration = SyntaxFactory.VariableDeclaration(
                arrayDefinitionNode.TypeNameNode.Transpile(TokenTranspileFlags.ReturnArray))
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
                        arrayDefinitionNode.IdentifierNameNode.ValueText,
                        SyntaxTriviaList.Empty),
                    null,
                    SyntaxFactory.EqualsValueClause(
                        // use SyntaxFactory.ArrayCreationExpression??
                        SyntaxFactory.ParseExpression($"new {arrayDefinitionNode.TypeNameNode.TypeNameToken.TranspileTypeString()}[{JASS_ARRAY_LIMIT_CONSTANT_NAME}]"))));
        }
    }

    public static partial class JassToLuaTranspiler
    {
        public static void TranspileGlobal(this Syntax.ArrayDefinitionSyntax arrayDefinitionNode, ref StringBuilder sb)
        {
            _ = arrayDefinitionNode ?? throw new ArgumentNullException(nameof(arrayDefinitionNode));

            arrayDefinitionNode.Transpile(ref sb);
        }

        public static void TranspileLocal(this Syntax.ArrayDefinitionSyntax arrayDefinitionNode, ref StringBuilder sb)
        {
            _ = arrayDefinitionNode ?? throw new ArgumentNullException(nameof(arrayDefinitionNode));

            sb.Append("local ");
            arrayDefinitionNode.Transpile(ref sb);
        }

        private static void Transpile(this Syntax.ArrayDefinitionSyntax arrayDefinitionNode, ref StringBuilder sb)
        {
            arrayDefinitionNode.IdentifierNameNode.TranspileIdentifier(ref sb);
            sb.Append(" = ");

            var tokenType = arrayDefinitionNode.TypeNameNode.TypeNameToken.TokenType;
            switch (tokenType)
            {
                case SyntaxTokenType.HandleKeyword:
                case SyntaxTokenType.AlphanumericIdentifier:
                    sb.Append("{}");
                    break;

                case SyntaxTokenType.IntegerKeyword:
                    sb.Append("__jarray(0)");
                    break;

                case SyntaxTokenType.RealKeyword:
                    sb.Append("__jarray(0.0)");
                    break;

                case SyntaxTokenType.StringKeyword:
                    sb.Append("__jarray(\"\")");
                    break;

                case SyntaxTokenType.BooleanKeyword:
                    sb.Append("__jarray(false)");
                    break;

                case SyntaxTokenType.CodeKeyword: throw new NotSupportedException("Code arrays are not supported.");

                default: throw new InvalidEnumArgumentException(nameof(tokenType), (int)tokenType, typeof(SyntaxTokenType));
            }
        }
    }
}