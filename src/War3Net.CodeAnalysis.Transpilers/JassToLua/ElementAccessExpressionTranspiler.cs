using CSharpLua.LuaAst;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaExpressionSyntax Transpile(JassElementAccessExpressionSyntax elementAccessExpression, out JassTypeSyntax type)
        {
            type = GetVariableType(elementAccessExpression.IdentifierName);

            return new LuaTableIndexAccessExpressionSyntax(
                Transpile(elementAccessExpression.IdentifierName),
                Transpile(elementAccessExpression.ElementAccessClause.Expression, out _));
        }
    }
}