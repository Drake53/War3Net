using System.Diagnostics.CodeAnalysis;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameExitStatement(JassExitStatementSyntax exitStatement, [NotNullWhen(true)] out JassStatementSyntax? renamedExitStatement)
        {
            if (TryRenameExpression(exitStatement.Condition, out var renamedCondition))
            {
                renamedExitStatement = new JassExitStatementSyntax(
                    exitStatement.ExitWhenToken,
                    renamedCondition);

                return true;
            }

            renamedExitStatement = null;
            return false;
        }
    }
}