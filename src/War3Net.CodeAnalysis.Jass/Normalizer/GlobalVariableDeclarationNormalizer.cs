namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteGlobalVariableDeclaration(JassGlobalVariableDeclarationSyntax globalVariableDeclaration, out JassGlobalDeclarationSyntax result)
        {
            _nodes.Add(globalVariableDeclaration);
            var normalized = base.RewriteGlobalVariableDeclaration(globalVariableDeclaration, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            _requireNewLineTrivia = _encounteredAnyTextOnCurrentLine;

            return normalized;
        }
    }
}