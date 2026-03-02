namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaStatementSyntax Transpile(JassSetStatementSyntax setStatement)
        {
            return new LuaAssignmentExpressionSyntax(
                setStatement.ElementAccessClause is null
                    ? Transpile(setStatement.IdentifierName)
                    : new LuaTableIndexAccessExpressionSyntax(Transpile(setStatement.IdentifierName), Transpile(setStatement.ElementAccessClause.Argument, out _)),
                Transpile(setStatement.EqualsValueClause.Value, out _));
        }
    }
}