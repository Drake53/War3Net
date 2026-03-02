namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        public JassCompilationUnitSyntax NormalizeWhitespace(JassCompilationUnitSyntax compilationUnit)
        {
            RewriteCompilationUnit(compilationUnit, out var result);
            return result;
        }

        /// <inheritdoc/>
        protected override bool RewriteCompilationUnit(JassCompilationUnitSyntax compilationUnit, out JassCompilationUnitSyntax result)
        {
            _nodes.Add(compilationUnit);
            var normalized = base.RewriteCompilationUnit(compilationUnit, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}