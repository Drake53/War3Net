using System.Diagnostics.CodeAnalysis;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameSetStatement(JassSetStatementSyntax setStatement, [NotNullWhen(true)] out JassStatementSyntax? renamedSetStatement)
        {
            if (TryRenameVariableIdentifierName(setStatement.IdentifierName, out var renamedIdentifierName) |
                TryRenameElementAccessClause(setStatement.ElementAccessClause, out var renamedElementAccessClause) |
                TryRenameEqualsValueClause(setStatement.Value, out var renamedValue))
            {
                renamedSetStatement = new JassSetStatementSyntax(
                    setStatement.SetToken,
                    renamedIdentifierName ?? setStatement.IdentifierName,
                    renamedElementAccessClause ?? setStatement.ElementAccessClause,
                    renamedValue ?? setStatement.Value);

                return true;
            }

            renamedSetStatement = null;
            return false;
        }
    }
}