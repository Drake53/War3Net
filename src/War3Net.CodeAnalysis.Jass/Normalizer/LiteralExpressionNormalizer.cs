namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteLiteralExpression(JassLiteralExpressionSyntax literalExpression, out JassExpressionSyntax result)
        {
            _nodes.Add(literalExpression);
            var normalized = base.RewriteLiteralExpression(literalExpression, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}