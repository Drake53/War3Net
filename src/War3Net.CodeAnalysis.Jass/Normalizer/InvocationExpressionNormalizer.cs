namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteInvocationExpression(JassInvocationExpressionSyntax invocationExpression, out JassExpressionSyntax result)
        {
            _nodes.Add(invocationExpression);
            var normalized = base.RewriteInvocationExpression(invocationExpression, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}