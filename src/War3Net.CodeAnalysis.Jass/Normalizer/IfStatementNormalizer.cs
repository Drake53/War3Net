namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteIfStatement(JassIfStatementSyntax ifStatement, out JassStatementSyntax result)
        {
            _nodes.Add(ifStatement);
            var normalized = base.RewriteIfStatement(ifStatement, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}