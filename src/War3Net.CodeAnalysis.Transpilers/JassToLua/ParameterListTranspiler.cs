namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public IEnumerable<LuaIdentifierNameSyntax> Transpile(JassParameterListOrEmptyParameterListSyntax parameterListOrEmptyParameterList)
        {
            return parameterListOrEmptyParameterList switch
            {
                JassParameterListSyntax parameterList => Transpile(parameterList),
                JassEmptyParameterListSyntax => Enumerable.Empty<LuaIdentifierNameSyntax>(),
            };
        }

        public IEnumerable<LuaIdentifierNameSyntax> Transpile(JassParameterListSyntax parameterList)
        {
            return parameterList.ParameterList.Items.Select(Transpile);
        }
    }
}