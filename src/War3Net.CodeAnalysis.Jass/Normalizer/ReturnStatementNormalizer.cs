namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteReturnStatement(JassReturnStatementSyntax returnStatement, out JassStatementSyntax result)
        {
            _nodes.Add(returnStatement);
            var normalized = base.RewriteReturnStatement(returnStatement, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            _requireNewLineTrivia = _encounteredAnyTextOnCurrentLine;

            return normalized;
        }
    }
}