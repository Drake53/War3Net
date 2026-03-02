namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaStatementSyntax Transpile(JassExitStatementSyntax exitStatement)
        {
            var ifStatement = new LuaIfStatementSyntax(Transpile(exitStatement.Condition, out _));

            ifStatement.Body.Statements.Add(LuaBreakStatementSyntax.Instance);

            return ifStatement;
        }
    }
}