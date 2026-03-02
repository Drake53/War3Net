using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassReturnClauseSyntax returnClause)
        {
            Render(returnClause.ReturnsToken);
            WriteSpace();
            Render(returnClause.ReturnType);
        }
    }
}