namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteEqualsValueClause(JassEqualsValueClauseSyntax? equalsValueClause, [NotNullIfNotNull("equalsValueClause")] out JassEqualsValueClauseSyntax? result)
        {
            if (equalsValueClause is null)
            {
                result = null;
                return false;
            }

            _nodes.Add(equalsValueClause);
            var normalized = base.RewriteEqualsValueClause(equalsValueClause, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}