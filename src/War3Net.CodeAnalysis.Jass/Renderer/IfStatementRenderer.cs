namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassIfStatementSyntax ifStatement)
        {
            Render(ifStatement.IfClause);

            foreach (var elseIfClause in ifStatement.ElseIfClauses)
            {
                Render(elseIfClause);
            }

            if (ifStatement.ElseClause is not null)
            {
                Render(ifStatement.ElseClause);
            }

            Render(ifStatement.EndIfToken);
        }
    }
}