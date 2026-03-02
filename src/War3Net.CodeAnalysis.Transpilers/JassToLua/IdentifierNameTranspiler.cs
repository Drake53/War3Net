using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaIdentifierNameSyntax Transpile(JassIdentifierNameSyntax identifierName)
        {
            return identifierName.Token.Text;
        }

        public LuaIdentifierNameSyntax Transpile(JassIdentifierNameSyntax identifierName, out JassTypeSyntax type)
        {
            type = GetVariableType(identifierName);
            return identifierName.Token.Text;
        }
    }
}