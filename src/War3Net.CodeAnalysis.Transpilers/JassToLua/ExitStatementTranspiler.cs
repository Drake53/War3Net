// ------------------------------------------------------------------------------
// <copyright file="ExitStatementTranspiler.cs" company="Drake53">
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
        public static void Transpile(this ExitStatementSyntax exitStatementNode, ref StringBuilder sb)
        {
            _ = exitStatementNode ?? throw new ArgumentNullException(nameof(exitStatementNode));

            sb.Append("if ");
            exitStatementNode.ConditionExpressionNode.Transpile(ref sb);
            sb.AppendLine(" then");
            sb.AppendLine("break");
            sb.Append("end");
        }

        public static LuaStatementSyntax TranspileToLua(this ExitStatementSyntax exitStatementNode)
        {
            _ = exitStatementNode ?? throw new ArgumentNullException(nameof(exitStatementNode));

            var @if = new LuaIfStatementSyntax(exitStatementNode.ConditionExpressionNode.TranspileToLua());
            @if.Body.Statements.Add(LuaBreakStatementSyntax.Instance);

            return @if;
        }
    }
}