using System.Linq;
using CSharpLua.LuaAst;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaStatementSyntax Transpile(JassLoopStatementSyntax loopStatement)
        {
            var whileStatement = new LuaWhileStatementSyntax(LuaIdentifierLiteralExpressionSyntax.True);

            whileStatement.Body.Statements.AddRange(loopStatement.Statements.Select(Transpile));

            return whileStatement;
        }
    }
}