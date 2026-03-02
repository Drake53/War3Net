using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassElementAccessClauseSyntax elementAccessClause)
        {
            Render(elementAccessClause.OpenBracketToken);
            Render(elementAccessClause.Expression);
            Render(elementAccessClause.CloseBracketToken);
        }
    }
}