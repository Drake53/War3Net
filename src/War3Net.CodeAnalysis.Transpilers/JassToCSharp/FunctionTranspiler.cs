// ------------------------------------------------------------------------------
// <copyright file="FunctionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static MethodDeclarationSyntax Transpile(this Jass.Syntax.FunctionSyntax functionNode)
        {
            _ = functionNode ?? throw new ArgumentNullException(nameof(functionNode));

            // TODO: do smth with constant keyword?

            var functionDeclr = functionNode.FunctionDeclarationNode.Transpile();

            return functionDeclr.WithBody(
                SyntaxFactory.Block(
                    functionNode.LocalVariableListNode.Transpile().Concat(
                    functionNode.StatementListNode.Transpile())))
                .WithModifiers(
                new Microsoft.CodeAnalysis.SyntaxTokenList(
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                    SyntaxFactory.Token(SyntaxKind.StaticKeyword)));
        }
    }
}