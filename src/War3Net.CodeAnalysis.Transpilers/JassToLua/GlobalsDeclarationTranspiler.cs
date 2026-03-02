namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public IEnumerable<LuaStatementSyntax> Transpile(JassGlobalsDeclarationSyntax globalsDeclaration)
        {
            return globalsDeclaration.GlobalDeclarations.Select(Transpile);
        }
    }
}