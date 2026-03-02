namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaStatementSyntax Transpile(JassIfStatementSyntax ifStatement)
        {
            var luaIfStatement = new LuaIfStatementSyntax(Transpile(ifStatement.IfClause.IfClauseDeclarator.Condition, out _));

            luaIfStatement.Body.Statements.AddRange(ifStatement.IfClause.Statements.Select(Transpile));
            luaIfStatement.ElseIfStatements.AddRange(ifStatement.ElseIfClauses.Select(Transpile));
            luaIfStatement.Else = ifStatement.ElseClause is null ? null : Transpile(ifStatement.ElseClause);

            return luaIfStatement;
        }
    }
}