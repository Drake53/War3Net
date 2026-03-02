using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public EqualsValueClauseSyntax Transpile(JassEqualsValueClauseSyntax equalsValueClause)
        {
            return SyntaxFactory.EqualsValueClause(
                Transpile(SyntaxKind.EqualsToken, equalsValueClause.EqualsToken),
                Transpile(equalsValueClause.Expression));
        }
    }
}