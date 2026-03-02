namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteParameter(JassParameterSyntax parameter, out JassParameterSyntax result)
        {
            _nodes.Add(parameter);
            var normalized = base.RewriteParameter(parameter, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}