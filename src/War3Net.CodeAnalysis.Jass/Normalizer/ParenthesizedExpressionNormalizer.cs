namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteParenthesizedExpression(JassParenthesizedExpressionSyntax parenthesizedExpression, out JassExpressionSyntax result)
        {
            _nodes.Add(parenthesizedExpression);
            var normalized = base.RewriteParenthesizedExpression(parenthesizedExpression, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}