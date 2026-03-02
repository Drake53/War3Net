namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteBinaryExpression(JassBinaryExpressionSyntax binaryExpression, out JassExpressionSyntax result)
        {
            _nodes.Add(binaryExpression);
            var normalized = base.RewriteBinaryExpression(binaryExpression, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}