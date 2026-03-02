namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassIfClauseSyntax ifClause)
        {
            Render(ifClause.IfClauseDeclarator);
            Indent();
            Render(ifClause.Statements);
            Outdent();
        }
    }
}