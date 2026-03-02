namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteLocalVariableDeclarationStatement(JassLocalVariableDeclarationStatementSyntax localVariableDeclarationStatement, out JassStatementSyntax result)
        {
            _nodes.Add(localVariableDeclarationStatement);
            var normalized = base.RewriteLocalVariableDeclarationStatement(localVariableDeclarationStatement, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            _requireNewLineTrivia = _encounteredAnyTextOnCurrentLine;

            return normalized;
        }
    }
}