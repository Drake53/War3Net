// ------------------------------------------------------------------------------
// <copyright file="ParenthesizedExpressionTranspiler.cs" company="Drake53">
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
        public static void Transpile(this ParenthesizedExpressionSyntax parenthesizedExpressionNode, ref StringBuilder sb, out bool isString)
        {
            _ = parenthesizedExpressionNode ?? throw new ArgumentNullException(nameof(parenthesizedExpressionNode));

            sb.Append('(');
            parenthesizedExpressionNode.ExpressionNode.Transpile(ref sb, out isString);
            sb.Append(')');
        }

        public static LuaExpressionSyntax TranspileToLua(this ParenthesizedExpressionSyntax parenthesizedExpressionNode, out bool isString)
        {
            _ = parenthesizedExpressionNode ?? throw new ArgumentNullException(nameof(parenthesizedExpressionNode));

            return new LuaParenthesizedExpressionSyntax(parenthesizedExpressionNode.ExpressionNode.TranspileToLua(out isString));
        }
    }
}