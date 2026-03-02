using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaExpressionSyntax Transpile(JassParenthesizedExpressionSyntax parenthesizedExpression, out JassTypeSyntax type)
        {
            return new LuaParenthesizedExpressionSyntax(Transpile(parenthesizedExpression.Expression, out type));
        }
    }
}