using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteEmptyParameterList(JassEmptyParameterListSyntax emptyParameterList, out JassParameterListOrEmptyParameterListSyntax result)
        {
            _nodes.Add(emptyParameterList);
            var normalized = base.RewriteEmptyParameterList(emptyParameterList, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}