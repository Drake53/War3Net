namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassSetStatementSyntax setStatement)
        {
            Render(setStatement.SetToken);
            WriteSpace();
            Render(setStatement.IdentifierName);

            if (setStatement.ElementAccessClause is not null)
            {
                Render(setStatement.ElementAccessClause);
            }

            WriteSpace();
            Render(setStatement.Value);
        }
    }
}