namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaStatementSyntax Transpile(JassSetStatementSyntax setStatement)
        {
            return new LuaAssignmentExpressionSyntax(
                setStatement.ElementAccessClause is null
                    ? Transpile(setStatement.IdentifierName)
                    : new LuaTableIndexAccessExpressionSyntax(Transpile(setStatement.IdentifierName), Transpile(setStatement.ElementAccessClause.Expression, out _)),
                Transpile(setStatement.Value.Expression, out _));
        }
    }
}