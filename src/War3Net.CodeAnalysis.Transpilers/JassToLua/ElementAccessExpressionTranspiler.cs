namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaExpressionSyntax Transpile(JassElementAccessExpressionSyntax elementAccessExpression, out JassTypeSyntax type)
        {
            type = GetVariableType(elementAccessExpression.IdentifierName);

            return new LuaTableIndexAccessExpressionSyntax(
                Transpile(elementAccessExpression.IdentifierName),
                Transpile(elementAccessExpression.ElementAccessClause.Argument, out _));
        }
    }
}