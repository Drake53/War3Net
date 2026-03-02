namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteUnaryExpression(JassUnaryExpressionSyntax unaryExpression, out JassExpressionSyntax result)
        {
            _nodes.Add(unaryExpression);
            var normalized = base.RewriteUnaryExpression(unaryExpression, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}