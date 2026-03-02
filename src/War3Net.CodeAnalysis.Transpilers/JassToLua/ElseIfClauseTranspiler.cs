namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaElseIfStatementSyntax Transpile(JassElseIfClauseSyntax elseIfClause)
        {
            var elseifStatement = new LuaElseIfStatementSyntax(Transpile(elseIfClause.ElseIfClauseDeclarator.Condition, out _));

            elseifStatement.Body.Statements.AddRange(elseIfClause.Statements.Select(Transpile));

            return elseifStatement;
        }
    }
}