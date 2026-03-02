using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteLoopStatement(JassLoopStatementSyntax loopStatement, out JassStatementSyntax result)
        {
            _nodes.Add(loopStatement);
            var normalized = base.RewriteLoopStatement(loopStatement, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}