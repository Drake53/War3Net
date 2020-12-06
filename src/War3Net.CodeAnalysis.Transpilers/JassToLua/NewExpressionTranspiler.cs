// ------------------------------------------------------------------------------
// <copyright file="NewExpressionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        [Obsolete]
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

        [Obsolete]
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

        public static LuaExpressionSyntax TranspileToLua(this NewExpressionSyntax newExpressionNode)
        {
            _ = newExpressionNode ?? throw new ArgumentNullException(nameof(newExpressionNode));

            var left = newExpressionNode.Expression.TranspileToLua(out var isString);
            if (newExpressionNode.ExpressionTail is null)
            {
                return left;
            }

            var right = newExpressionNode.ExpressionTail.ExpressionNode.TranspileToLua(out var rightIsString);
            isString = isString || rightIsString;

            return new LuaBinaryExpressionSyntax(left, newExpressionNode.ExpressionTail.BinaryOperatorNode.TranspileToLua(isString), right);
        }

        public static LuaExpressionSyntax TranspileToLua(this NewExpressionSyntax newExpressionNode, out bool isString)
        {
            _ = newExpressionNode ?? throw new ArgumentNullException(nameof(newExpressionNode));

            var left = newExpressionNode.Expression.TranspileToLua(out isString);
            if (newExpressionNode.ExpressionTail is null)
            {
                return left;
            }

            var right = newExpressionNode.ExpressionTail.ExpressionNode.TranspileToLua(out var rightIsString);
            isString = isString || rightIsString;

            return new LuaBinaryExpressionSyntax(left, newExpressionNode.ExpressionTail.BinaryOperatorNode.TranspileToLua(isString), right);
        }
    }
}