namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteElementAccessClause(JassElementAccessClauseSyntax? elementAccessClause, [NotNullIfNotNull("elementAccessClause")] out JassElementAccessClauseSyntax? result)
        {
            if (elementAccessClause is null)
            {
                result = null;
                return false;
            }

            _nodes.Add(elementAccessClause);
            var normalized = base.RewriteElementAccessClause(elementAccessClause, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}