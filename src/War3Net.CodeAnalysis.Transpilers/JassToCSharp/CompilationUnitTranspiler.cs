namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public IEnumerable<MemberDeclarationSyntax> Transpile(JassCompilationUnitSyntax compilationUnit)
        {
            return compilationUnit.Declarations.SelectMany(Transpile);
        }
    }
}