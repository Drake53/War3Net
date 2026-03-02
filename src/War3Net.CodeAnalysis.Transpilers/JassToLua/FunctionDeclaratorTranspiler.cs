using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaVariableDeclaratorSyntax Transpile(JassFunctionDeclaratorSyntax functionDeclarator)
        {
            RegisterFunctionReturnType(functionDeclarator);

            var functionExpression = new LuaFunctionExpressionSyntax();
            functionExpression.AddParameters(Transpile(functionDeclarator.ParameterList));

            return new LuaVariableDeclaratorSyntax(Transpile(functionDeclarator.IdentifierName), functionExpression);
        }
    }
}