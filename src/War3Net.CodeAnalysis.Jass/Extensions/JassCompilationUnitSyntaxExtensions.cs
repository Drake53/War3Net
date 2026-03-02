namespace War3Net.CodeAnalysis.Jass.Extensions
{
    public static class JassCompilationUnitSyntaxExtensions
    {
        public static JassCompilationUnitSyntax NormalizeWhitespace(this JassCompilationUnitSyntax compilationUnit)
        {
            return new JassSyntaxNormalizingVisitor().NormalizeWhitespace(compilationUnit);
        }
    }
}