using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteGlobalsDeclaration(JassGlobalsDeclarationSyntax globalsDeclaration, out JassTopLevelDeclarationSyntax result)
        {
            _nodes.Add(globalsDeclaration);
            var normalized = base.RewriteGlobalsDeclaration(globalsDeclaration, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}