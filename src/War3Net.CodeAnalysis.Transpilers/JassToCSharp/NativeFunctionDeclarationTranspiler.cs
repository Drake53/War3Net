// ------------------------------------------------------------------------------
// <copyright file="NativeFunctionDeclarationTranspiler.cs" company="Drake53">
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
        public static MemberDeclarationSyntax Transpile(this Jass.Syntax.NativeFunctionDeclarationSyntax nativeFunctionDeclarationNode)
        {
            _ = nativeFunctionDeclarationNode ?? throw new ArgumentNullException(nameof(nativeFunctionDeclarationNode));

            return nativeFunctionDeclarationNode.FunctionDeclarationNode.Transpile()
                .WithModifiers(
                    new SyntaxTokenList(
                        SyntaxFactory.Token(TranspileToEnumHandler.IsFunctionEnumConverter(nativeFunctionDeclarationNode.FunctionDeclarationNode.IdentifierNode.ValueText, out _)
                            ? TranspileToEnumHandler.EnumCastFunctionAccessModifier
                            : SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(SyntaxKind.StaticKeyword),
                        SyntaxFactory.Token(SyntaxKind.ExternKeyword)))
                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        }
    }
}