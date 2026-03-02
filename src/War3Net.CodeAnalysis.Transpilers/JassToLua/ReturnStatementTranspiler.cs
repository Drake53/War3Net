namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaStatementSyntax Transpile(JassReturnStatementSyntax returnStatement)
        {
            return returnStatement.Value is null
                ? new LuaReturnStatementSyntax()
                : new LuaReturnStatementSyntax(Transpile(returnStatement.Value, out _));
        }
    }
}