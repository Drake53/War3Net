using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteArrayDeclarator(JassArrayDeclaratorSyntax arrayDeclarator, out JassVariableOrArrayDeclaratorSyntax result)
        {
            _nodes.Add(arrayDeclarator);
            var normalized = base.RewriteArrayDeclarator(arrayDeclarator, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}