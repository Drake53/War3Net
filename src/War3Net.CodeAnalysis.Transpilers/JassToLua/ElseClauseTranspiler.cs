using System.Linq;
using CSharpLua.LuaAst;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaElseClauseSyntax Transpile(JassElseClauseSyntax elseClause)
        {
            var luaElseClause = new LuaElseClauseSyntax();

            luaElseClause.Body.Statements.AddRange(elseClause.Statements.Select(Transpile));

            return luaElseClause;
        }
    }
}