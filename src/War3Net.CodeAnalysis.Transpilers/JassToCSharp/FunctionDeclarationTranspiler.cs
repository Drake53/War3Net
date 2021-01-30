// ------------------------------------------------------------------------------
// <copyright file="FunctionDeclarationTranspiler.cs" company="Drake53">
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
        public MemberDeclarationSyntax Transpile(JassFunctionDeclarationSyntax functionDeclaration)
        {
            return SyntaxFactory.MethodDeclaration(
                default,
                new SyntaxTokenList(
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                    SyntaxFactory.Token(SyntaxKind.StaticKeyword)),
                Transpile(functionDeclaration.FunctionDeclarator.ReturnType),
                null,
                Transpile(functionDeclaration.FunctionDeclarator.IdentifierName),
                null,
                SyntaxFactory.ParameterList(Transpile(functionDeclaration.FunctionDeclarator.ParameterList)),
                default,
                SyntaxFactory.Block(Transpile(functionDeclaration.Body)),
                null);
        }
    }
}