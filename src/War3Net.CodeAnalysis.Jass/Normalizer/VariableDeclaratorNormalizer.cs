namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteVariableDeclarator(JassVariableDeclaratorSyntax variableDeclarator, out JassVariableOrArrayDeclaratorSyntax result)
        {
            _nodes.Add(variableDeclarator);
            var normalized = base.RewriteVariableDeclarator(variableDeclarator, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}