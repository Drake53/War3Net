namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteFunctionReferenceExpression(JassFunctionReferenceExpressionSyntax functionReferenceExpression, out JassExpressionSyntax result)
        {
            _nodes.Add(functionReferenceExpression);
            var normalized = base.RewriteFunctionReferenceExpression(functionReferenceExpression, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}