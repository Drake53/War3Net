using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteElseIfClause(JassElseIfClauseSyntax elseIfClause, out JassElseIfClauseSyntax result)
        {
            _nodes.Add(elseIfClause);
            var normalized = base.RewriteElseIfClause(elseIfClause, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}