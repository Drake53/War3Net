using System.Collections.Generic;
using System.Linq;
using CSharpLua.LuaAst;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public IEnumerable<LuaExpressionSyntax> Transpile(JassArgumentListSyntax argumentList)
        {
            return argumentList.ArgumentList.Items.Select(argument => Transpile(argument, out _));
        }
    }
}