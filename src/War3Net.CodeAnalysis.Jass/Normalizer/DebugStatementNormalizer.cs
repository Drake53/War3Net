using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteDebugStatement(JassDebugStatementSyntax debugStatement, out JassStatementSyntax result)
        {
            _nodes.Add(debugStatement);
            var normalized = base.RewriteDebugStatement(debugStatement, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}