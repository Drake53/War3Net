namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameLoopStatement(JassLoopStatementSyntax loopStatement, [NotNullWhen(true)] out JassStatementSyntax? renamedLoopStatement)
        {
            if (TryRenameStatementList(loopStatement.Statements, out var renamedStatements))
            {
                renamedLoopStatement = new JassLoopStatementSyntax(
                    loopStatement.LoopToken,
                    renamedStatements.Value,
                    loopStatement.EndLoopToken);

                return true;
            }

            renamedLoopStatement = null;
            return false;
        }
    }
}