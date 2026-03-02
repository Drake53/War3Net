namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewritePredefinedType(JassPredefinedTypeSyntax predefinedType, out JassTypeSyntax result)
        {
            _nodes.Add(predefinedType);
            var normalized = base.RewritePredefinedType(predefinedType, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}