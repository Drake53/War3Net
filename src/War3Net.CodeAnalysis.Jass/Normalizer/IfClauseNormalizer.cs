namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteIfClause(JassIfClauseSyntax ifClause, out JassIfClauseSyntax result)
        {
            _nodes.Add(ifClause);
            var normalized = base.RewriteIfClause(ifClause, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}