// ------------------------------------------------------------------------------
// <copyright file="ConstantExpressionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this ConstantExpressionSyntax constantExpressionNode, ref StringBuilder sb, out bool isString)
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