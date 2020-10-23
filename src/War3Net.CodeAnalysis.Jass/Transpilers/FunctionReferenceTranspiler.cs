// ------------------------------------------------------------------------------
// <copyright file="FunctionReferenceTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Text;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static ExpressionSyntax Transpile(this Syntax.FunctionReferenceSyntax functionReferenceNode)
        {
            _ = functionReferenceNode ?? throw new ArgumentNullException(nameof(functionReferenceNode));

            return functionReferenceNode.IdentifierNameNode.TranspileExpression();
            // return SyntaxFactory.ParseExpression(functionReferenceNode.IdentifierNameNode.TranspileType());
        }

        public static ExpressionSyntax Transpile(this Syntax.FunctionReferenceSyntax functionReferenceNode, out bool @const)
        {
            _ = functionReferenceNode ?? throw new ArgumentNullException(nameof(functionReferenceNode));

            @const = false;

            return functionReferenceNode.IdentifierNameNode.TranspileExpression();
            // return SyntaxFactory.ParseExpression(functionReferenceNode.IdentifierNameNode.TranspileType());
        }
    }

    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this Syntax.FunctionReferenceSyntax functionReferenceNode, ref StringBuilder sb)
        {
            _ = functionReferenceNode ?? throw new ArgumentNullException(nameof(functionReferenceNode));

            functionReferenceNode.IdentifierNameNode.TranspileExpression(ref sb);
        }
    }
}