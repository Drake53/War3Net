using System.Collections.Generic;
using System.Linq;
using CSharpLua.LuaAst;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public IEnumerable<LuaStatementSyntax> Transpile(JassGlobalsDeclarationSyntax globalsDeclaration)
        {
            return globalsDeclaration.GlobalDeclarations.Select(Transpile);
        }
    }
}