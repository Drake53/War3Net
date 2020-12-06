// ------------------------------------------------------------------------------
// <copyright file="ReturnStatementTranspiler.cs" company="Drake53">
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
        public static void Transpile(this ReturnStatementSyntax returnStatementNode, ref StringBuilder sb)
        {
            _ = returnStatementNode ?? throw new ArgumentNullException(nameof(returnStatementNode));

            if (returnStatementNode.EmptyExpressionNode is null)
            {
                sb.Append("return ");
                returnStatementNode.ExpressionNode?.Transpile(ref sb);
            }
            else
            {
                sb.Append("return");
            }
        }

        public static LuaReturnStatementSyntax TranspileToLua(this ReturnStatementSyntax returnStatementNode)
        {
            _ = returnStatementNode ?? throw new ArgumentNullException(nameof(returnStatementNode));

            return returnStatementNode.ExpressionNode is null
                ? new LuaReturnStatementSyntax()
                : new LuaReturnStatementSyntax(returnStatementNode.ExpressionNode.TranspileToLua());
        }
    }
}