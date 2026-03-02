namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaExpressionSyntax Transpile(JassEqualsValueClauseSyntax equalsValueClause)
        {
            return Transpile(equalsValueClause.Value, out _);
        }
    }
}