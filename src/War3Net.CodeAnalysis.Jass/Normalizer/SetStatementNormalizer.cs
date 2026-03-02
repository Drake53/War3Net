namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteSetStatement(JassSetStatementSyntax setStatement, out JassStatementSyntax result)
        {
            _nodes.Add(setStatement);
            var normalized = base.RewriteSetStatement(setStatement, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            _requireNewLineTrivia = _encounteredAnyTextOnCurrentLine;

            return normalized;
        }
    }
}