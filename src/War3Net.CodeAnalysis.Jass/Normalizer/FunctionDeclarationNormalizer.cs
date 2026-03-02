using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteFunctionDeclaration(JassFunctionDeclarationSyntax functionDeclaration, out JassTopLevelDeclarationSyntax result)
        {
            _nodes.Add(functionDeclaration);
            var normalized = base.RewriteFunctionDeclaration(functionDeclaration, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}