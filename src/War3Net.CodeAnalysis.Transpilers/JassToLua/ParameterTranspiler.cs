using CSharpLua.LuaAst;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaIdentifierNameSyntax Transpile(JassParameterSyntax parameter)
        {
            RegisterLocalVariableType(parameter);

            return Transpile(parameter.IdentifierName);
        }
    }
}