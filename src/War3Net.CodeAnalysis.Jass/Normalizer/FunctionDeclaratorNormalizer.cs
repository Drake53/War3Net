using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteFunctionDeclarator(JassFunctionDeclaratorSyntax functionDeclarator, out JassFunctionDeclaratorSyntax result)
        {
            _nodes.Add(functionDeclarator);
            var normalized = base.RewriteFunctionDeclarator(functionDeclarator, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            _currentLevelOfIndentation++;
            _requireNewLineTrivia = _encounteredAnyTextOnCurrentLine;

            return normalized;
        }
    }
}