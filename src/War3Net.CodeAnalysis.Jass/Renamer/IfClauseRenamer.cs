namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameIfClause(JassIfClauseSyntax ifClause, [NotNullWhen(true)] out JassIfClauseSyntax? renamedIfClause)
        {
            if (TryRenameIfClauseDeclarator(ifClause.IfClauseDeclarator, out var renamedDeclarator) |
                TryRenameStatementList(ifClause.Statements, out var renamedStatements))
            {
                renamedIfClause = new JassIfClauseSyntax(
                    renamedDeclarator ?? ifClause.IfClauseDeclarator,
                    renamedStatements ?? ifClause.Statements);

                return true;
            }

            renamedIfClause = null;
            return false;
        }
    }
}