// ------------------------------------------------------------------------------
// <copyright file="ElseClauseTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaElseClauseSyntax Transpile(JassElseClauseSyntax elseClause)
        {
            var luaElseClause = new LuaElseClauseSyntax();

            luaElseClause.Body.Statements.AddRange(Transpile(elseClause.Statements));

            return luaElseClause;
        }
    }
}