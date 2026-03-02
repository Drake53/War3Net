namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteElseIfClauseDeclarator(JassElseIfClauseDeclaratorSyntax elseIfClauseDeclarator, out JassElseIfClauseDeclaratorSyntax result)
        {
            _nodes.Add(elseIfClauseDeclarator);
            var normalized = base.RewriteElseIfClauseDeclarator(elseIfClauseDeclarator, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}