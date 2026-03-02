namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteReturnClause(JassReturnClauseSyntax returnClause, out JassReturnClauseSyntax result)
        {
            _nodes.Add(returnClause);
            var normalized = base.RewriteReturnClause(returnClause, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}