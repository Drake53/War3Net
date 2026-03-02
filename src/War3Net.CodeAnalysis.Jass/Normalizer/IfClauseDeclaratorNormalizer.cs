namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteIfClauseDeclarator(JassIfClauseDeclaratorSyntax ifClauseDeclarator, out JassIfClauseDeclaratorSyntax result)
        {
            _nodes.Add(ifClauseDeclarator);
            var normalized = base.RewriteIfClauseDeclarator(ifClauseDeclarator, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}