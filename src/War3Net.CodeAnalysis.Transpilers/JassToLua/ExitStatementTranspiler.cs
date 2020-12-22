// ------------------------------------------------------------------------------
// <copyright file="ExitStatementTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        [return: NotNullIfNotNull("exitStatement")]
        public LuaStatementSyntax? Transpile(ExitStatementSyntax? exitStatement)
        {
            if (exitStatement is null)
            {
                return null;
            }

            var @if = new LuaIfStatementSyntax(Transpile(exitStatement.ConditionExpressionNode));
            @if.Body.Statements.Add(LuaBreakStatementSyntax.Instance);

            return @if;
        }
    }
}