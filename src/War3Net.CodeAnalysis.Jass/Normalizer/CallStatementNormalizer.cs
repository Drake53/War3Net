namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteCallStatement(JassCallStatementSyntax callStatement, out JassStatementSyntax result)
        {
            _nodes.Add(callStatement);
            var normalized = base.RewriteCallStatement(callStatement, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            _requireNewLineTrivia = _encounteredAnyTextOnCurrentLine;

            return normalized;
        }
    }
}