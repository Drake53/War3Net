using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaExpressionSyntax Transpile(JassEqualsValueClauseSyntax equalsValueClause)
        {
            return Transpile(equalsValueClause.Expression, out _);
        }
    }
}