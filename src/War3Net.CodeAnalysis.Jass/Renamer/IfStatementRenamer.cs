namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameIfStatement(JassIfStatementSyntax ifStatement, [NotNullWhen(true)] out JassStatementSyntax? renamedIfStatement)
        {
            if (TryRenameIfClause(ifStatement.IfClause, out var renamedIfClause) |
                TryRenameElseIfClauseList(ifStatement.ElseIfClauses, out var renamedElseIfClauses) |
                TryRenameElseClause(ifStatement.ElseClause, out var renamedElseClause))
            {
                renamedIfStatement = new JassIfStatementSyntax(
                    renamedIfClause ?? ifStatement.IfClause,
                    renamedElseIfClauses ?? ifStatement.ElseIfClauses,
                    renamedElseClause ?? ifStatement.ElseClause,
                    ifStatement.EndIfToken);

                return true;
            }

            renamedIfStatement = null;
            return false;
        }
    }
}