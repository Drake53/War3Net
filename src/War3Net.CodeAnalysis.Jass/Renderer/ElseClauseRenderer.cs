namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassElseClauseSyntax elseClause)
        {
            Render(elseClause.ElseToken);
            Indent();
            Render(elseClause.Statements);
            Outdent();
        }
    }
}