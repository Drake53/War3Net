using CSharpLua.LuaAst;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaExpressionSyntax Transpile(JassFunctionReferenceExpressionSyntax functionReferenceExpression, out JassTypeSyntax type)
        {
            type = JassPredefinedTypeSyntax.Code;

            return Transpile(functionReferenceExpression.IdentifierName);
        }
    }
}