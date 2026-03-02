using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteParameterList(JassParameterListSyntax parameterList, out JassParameterListOrEmptyParameterListSyntax result)
        {
            _nodes.Add(parameterList);
            var normalized = base.RewriteParameterList(parameterList, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}