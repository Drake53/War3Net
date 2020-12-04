// ------------------------------------------------------------------------------
// <copyright file="FunctionReferenceTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static ExpressionSyntax Transpile(this Jass.Syntax.FunctionReferenceSyntax functionReferenceNode)
        {
            _ = functionReferenceNode ?? throw new ArgumentNullException(nameof(functionReferenceNode));

            return functionReferenceNode.IdentifierNameNode.TranspileExpression();
            // return SyntaxFactory.ParseExpression(functionReferenceNode.IdentifierNameNode.TranspileType());
        }

        public static ExpressionSyntax Transpile(this Jass.Syntax.FunctionReferenceSyntax functionReferenceNode, out bool @const)
        {
            _ = functionReferenceNode ?? throw new ArgumentNullException(nameof(functionReferenceNode));

            @const = false;

            return functionReferenceNode.IdentifierNameNode.TranspileExpression();
            // return SyntaxFactory.ParseExpression(functionReferenceNode.IdentifierNameNode.TranspileType());
        }
    }
}