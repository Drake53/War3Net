// ------------------------------------------------------------------------------
// <copyright file="FunctionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static MethodDeclarationSyntax Transpile(this Syntax.FunctionSyntax functionNode)
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
                    SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PublicKeyword),
                    SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.StaticKeyword)));
        }
    }

    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this Syntax.FunctionSyntax functionNode, ref StringBuilder sb)
        {
            _ = functionNode ?? throw new ArgumentNullException(nameof(functionNode));

            functionNode.FunctionDeclarationNode.Transpile(ref sb);
            functionNode.DeclarationLineDelimiterNode.Transpile(ref sb);
            functionNode.LocalVariableListNode.Transpile(ref sb);
            functionNode.StatementListNode.Transpile(ref sb);
            sb.Append("end");
            functionNode.LastLineDelimiterNode.Transpile(ref sb);

            TranspileStringConcatenationHandler.ResetLocalVariables();
        }
    }
}