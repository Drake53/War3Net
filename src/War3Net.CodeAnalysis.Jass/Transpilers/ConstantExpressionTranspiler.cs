// ------------------------------------------------------------------------------
// <copyright file="ConstantExpressionTranspiler.cs" company="Drake53">
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
        public static ExpressionSyntax Transpile(this Syntax.ConstantExpressionSyntax constantExpressionNode)
        {
            _ = constantExpressionNode ?? throw new ArgumentNullException(nameof(constantExpressionNode));

            return constantExpressionNode.IntegerExpressionNode?.Transpile()
                ?? constantExpressionNode.RealExpressionNode?.TranspileExpression()
                ?? constantExpressionNode.BooleanExpressionNode?.Transpile()
                ?? constantExpressionNode.StringExpressionNode?.Transpile()
                ?? constantExpressionNode.NullExpressionNode.TranspileExpression();
        }

        public static ExpressionSyntax Transpile(this Syntax.ConstantExpressionSyntax constantExpressionNode, out bool @const)
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

    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this Syntax.ConstantExpressionSyntax constantExpressionNode, ref StringBuilder sb, out bool isString)
        {
            _ = constantExpressionNode ?? throw new ArgumentNullException(nameof(constantExpressionNode));

            isString = constantExpressionNode.StringExpressionNode != null;

            constantExpressionNode.IntegerExpressionNode?.Transpile(ref sb);
            constantExpressionNode.RealExpressionNode?.TranspileExpression(ref sb);
            constantExpressionNode.BooleanExpressionNode?.Transpile(ref sb);
            constantExpressionNode.StringExpressionNode?.Transpile(ref sb);
            constantExpressionNode.NullExpressionNode?.TranspileExpression(ref sb);
        }
    }
}