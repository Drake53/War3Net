namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteExitStatement(JassExitStatementSyntax exitStatement, out JassStatementSyntax result)
        {
            _nodes.Add(exitStatement);
            var normalized = base.RewriteExitStatement(exitStatement, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            _requireNewLineTrivia = _encounteredAnyTextOnCurrentLine;

            return normalized;
        }
    }
}