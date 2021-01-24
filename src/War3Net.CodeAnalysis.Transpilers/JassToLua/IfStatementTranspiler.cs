// ------------------------------------------------------------------------------
// <copyright file="IfStatementTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaStatementSyntax Transpile(JassIfStatementSyntax ifStatement)
        {
            var luaIfStatement = new LuaIfStatementSyntax(Transpile(ifStatement.Condition, out _));

            luaIfStatement.Body.Statements.AddRange(Transpile(ifStatement.Body));
            luaIfStatement.ElseIfStatements.AddRange(ifStatement.ElseIfClauses.Select(Transpile));
            luaIfStatement.Else = ifStatement.ElseClause is null ? null : Transpile(ifStatement.ElseClause);

            return luaIfStatement;
        }
    }
}