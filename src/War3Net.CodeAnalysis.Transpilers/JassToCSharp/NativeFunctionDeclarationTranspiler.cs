// ------------------------------------------------------------------------------
// <copyright file="NativeFunctionDeclarationTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public MemberDeclarationSyntax Transpile(JassNativeFunctionDeclarationSyntax nativeFunctionDeclaration)
        {
            return SyntaxFactory.MethodDeclaration(
                default,
                new SyntaxTokenList(
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                    SyntaxFactory.Token(SyntaxKind.StaticKeyword),
                    SyntaxFactory.Token(SyntaxKind.ExternKeyword)),
                Transpile(nativeFunctionDeclaration.FunctionDeclarator.ReturnType),
                null,
                Transpile(nativeFunctionDeclaration.FunctionDeclarator.IdentifierName),
                null,
                SyntaxFactory.ParameterList(Transpile(nativeFunctionDeclaration.FunctionDeclarator.ParameterList)),
                default,
                null,
                null,
                SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        }
    }
}