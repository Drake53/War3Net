// ------------------------------------------------------------------------------
// <copyright file="UnaryExpressionTranspiler.cs" company="Drake53">
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
        public static void Transpile(this UnaryExpressionSyntax unaryExpressionNode, ref StringBuilder sb)
        {
            _ = unaryExpressionNode ?? throw new ArgumentNullException(nameof(unaryExpressionNode));

            unaryExpressionNode.UnaryOperatorNode.Transpile(ref sb);
            unaryExpressionNode.ExpressionNode.Transpile(ref sb);
        }

        public static LuaExpressionSyntax TranspileToLua(this UnaryExpressionSyntax unaryExpressionNode)
        {
            _ = unaryExpressionNode ?? throw new ArgumentNullException(nameof(unaryExpressionNode));

            return new LuaPrefixUnaryExpressionSyntax(
                unaryExpressionNode.ExpressionNode.TranspileToLua(),
                unaryExpressionNode.UnaryOperatorNode.TranspileToLua());
        }
    }
}