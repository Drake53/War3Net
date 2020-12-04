// ------------------------------------------------------------------------------
// <copyright file="NewExpressionTranspiler.cs" company="Drake53">
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
        public static void Transpile(this NewExpressionSyntax newExpressionNode, ref StringBuilder sb)
        {
            _ = newExpressionNode ?? throw new ArgumentNullException(nameof(newExpressionNode));

            newExpressionNode.Expression.Transpile(ref sb, out var isString);
            if (newExpressionNode.EmptyExpressionTail is null)
            {
                newExpressionNode.ExpressionTail.BinaryOperatorNode.Transpile(isString, ref sb);
                newExpressionNode.ExpressionTail.ExpressionNode.Transpile(ref sb);
            }
        }

        public static void Transpile(this NewExpressionSyntax newExpressionNode, ref StringBuilder sb, out bool isString)
        {
            _ = newExpressionNode ?? throw new ArgumentNullException(nameof(newExpressionNode));

            newExpressionNode.Expression.Transpile(ref sb, out isString);
            if (newExpressionNode.EmptyExpressionTail is null)
            {
                newExpressionNode.ExpressionTail.BinaryOperatorNode.Transpile(isString, ref sb);
                newExpressionNode.ExpressionTail.ExpressionNode.Transpile(ref sb);
            }
        }
    }
}