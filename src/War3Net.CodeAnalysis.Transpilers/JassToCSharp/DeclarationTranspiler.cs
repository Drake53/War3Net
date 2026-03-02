namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public IEnumerable<MemberDeclarationSyntax> Transpile(JassTopLevelDeclarationSyntax declaration)
        {
            return declaration switch
            {
                JassTypeDeclarationSyntax typeDeclaration => new[] { Transpile(typeDeclaration) },
                JassGlobalsDeclarationSyntax globalsDeclaration => Transpile(globalsDeclaration),
                JassNativeFunctionDeclarationSyntax nativeFunctionDeclaration => new[] { Transpile(nativeFunctionDeclaration) },
                JassFunctionDeclarationSyntax functionDeclaration => new[] { Transpile(functionDeclaration) },
                _ => Array.Empty<MemberDeclarationSyntax>(),
            };
        }
    }
}