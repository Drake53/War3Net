namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteElseClause(JassElseClauseSyntax? elseClause, [NotNullIfNotNull("elseClause")] out JassElseClauseSyntax? result)
        {
            if (elseClause is null)
            {
                result = null;
                return false;
            }

            _nodes.Add(elseClause);
            var normalized = base.RewriteElseClause(elseClause, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}