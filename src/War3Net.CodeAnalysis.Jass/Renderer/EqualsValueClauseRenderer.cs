namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassEqualsValueClauseSyntax equalsValueClause)
        {
            Render(equalsValueClause.EqualsToken);
            WriteSpace();
            Render(equalsValueClause.Expression);
        }
    }
}