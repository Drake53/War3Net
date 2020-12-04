// ------------------------------------------------------------------------------
// <copyright file="ArrayReferenceTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static ExpressionSyntax Transpile(this Jass.Syntax.ArrayReferenceSyntax arrayReferenceNode)
        {
            _ = arrayReferenceNode ?? throw new ArgumentNullException(nameof(arrayReferenceNode));

            return SyntaxFactory.ElementAccessExpression(
                arrayReferenceNode.IdentifierNameNode.TranspileExpression())
                .AddArgumentListArguments(
                SyntaxFactory.Argument(
                    arrayReferenceNode.IndexExpressionNode.Transpile()));
        }

        public static ExpressionSyntax Transpile(this Jass.Syntax.ArrayReferenceSyntax arrayReferenceNode, out bool @const)
        {
            _ = arrayReferenceNode ?? throw new ArgumentNullException(nameof(arrayReferenceNode));

            @const = false;

            return SyntaxFactory.ElementAccessExpression(
                arrayReferenceNode.IdentifierNameNode.TranspileExpression())
                .AddArgumentListArguments(
                SyntaxFactory.Argument(
                    arrayReferenceNode.IndexExpressionNode.Transpile()));
        }
    }
}