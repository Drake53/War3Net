using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaExpressionSyntax Transpile(JassInvocationExpressionSyntax invocationExpression, out JassTypeSyntax type)
        {
            type = GetFunctionReturnType(invocationExpression.IdentifierName);

            var luaInvocationExpression = new LuaInvocationExpressionSyntax(Transpile(invocationExpression.IdentifierName));

            luaInvocationExpression.AddArguments(Transpile(invocationExpression.ArgumentList));

            return luaInvocationExpression;
        }
    }
}