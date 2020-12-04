// ------------------------------------------------------------------------------
// <copyright file="ConstantExpressionTranspiler.cs" company="Drake53">
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
        public static ExpressionSyntax Transpile(this Jass.Syntax.ConstantExpressionSyntax constantExpressionNode)
        {
            _ = constantExpressionNode ?? throw new ArgumentNullException(nameof(constantExpressionNode));

            return constantExpressionNode.IntegerExpressionNode?.Transpile()
                ?? constantExpressionNode.RealExpressionNode?.TranspileExpression()
                ?? constantExpressionNode.BooleanExpressionNode?.Transpile()
                ?? constantExpressionNode.StringExpressionNode?.Transpile()
                ?? constantExpressionNode.NullExpressionNode.TranspileExpression();
        }

        public static ExpressionSyntax Transpile(this Jass.Syntax.ConstantExpressionSyntax constantExpressionNode, out bool @const)
        {
            _ = constantExpressionNode ?? throw new ArgumentNullException(nameof(constantExpressionNode));

            @const = true;

            return constantExpressionNode.IntegerExpressionNode?.Transpile()
                ?? constantExpressionNode.RealExpressionNode?.TranspileExpression()
                ?? constantExpressionNode.BooleanExpressionNode?.Transpile()
                ?? constantExpressionNode.StringExpressionNode?.Transpile()
                ?? constantExpressionNode.NullExpressionNode.TranspileExpression();
        }
    }
}