namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaStatementSyntax Transpile(JassDebugStatementSyntax debugStatement)
        {
            return Transpile(debugStatement.Statement);
        }
    }
}