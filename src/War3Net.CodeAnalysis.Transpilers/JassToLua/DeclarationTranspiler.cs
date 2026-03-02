namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public IEnumerable<LuaStatementSyntax> Transpile(JassTopLevelDeclarationSyntax declaration)
        {
            if (declaration is JassNativeFunctionDeclarationSyntax nativeFunctionDeclaration)
            {
                RegisterFunctionReturnType(nativeFunctionDeclaration);
            }

            return declaration switch
            {
                JassGlobalsDeclarationSyntax globalsDeclaration => Transpile(globalsDeclaration),
                JassFunctionDeclarationSyntax functionDeclaration => IgnoreEmptyDeclarations && KeepFunctionsSeparated ? new[] { Transpile(functionDeclaration), LuaBlankLinesStatement.One } : new[] { Transpile(functionDeclaration) },
                _ => Array.Empty<LuaStatementSyntax>(),
            };
        }
    }
}