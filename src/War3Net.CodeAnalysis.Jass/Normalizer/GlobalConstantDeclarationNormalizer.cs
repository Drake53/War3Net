namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteGlobalConstantDeclaration(JassGlobalConstantDeclarationSyntax globalConstantDeclaration, out JassGlobalDeclarationSyntax result)
        {
            _nodes.Add(globalConstantDeclaration);
            var normalized = base.RewriteGlobalConstantDeclaration(globalConstantDeclaration, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            _requireNewLineTrivia = _encounteredAnyTextOnCurrentLine;

            return normalized;
        }
    }
}