namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaStatementSyntax Transpile(JassCallStatementSyntax callStatement)
        {
            return new LuaInvocationExpressionSyntax(
                Transpile(callStatement.IdentifierName),
                Transpile(callStatement.ArgumentList));
        }
    }
}