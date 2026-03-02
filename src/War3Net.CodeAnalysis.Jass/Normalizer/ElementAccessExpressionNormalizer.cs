using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteElementAccessExpression(JassElementAccessExpressionSyntax elementAccessExpression, out JassExpressionSyntax result)
        {
            _nodes.Add(elementAccessExpression);
            var normalized = base.RewriteElementAccessExpression(elementAccessExpression, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}