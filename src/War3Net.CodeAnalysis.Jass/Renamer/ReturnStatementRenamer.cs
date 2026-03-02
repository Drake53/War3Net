using System.Diagnostics.CodeAnalysis;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameReturnStatement(JassReturnStatementSyntax returnStatement, [NotNullWhen(true)] out JassStatementSyntax? renamedReturnStatement)
        {
            if (TryRenameExpression(returnStatement.Value, out var renamedValue))
            {
                renamedReturnStatement = new JassReturnStatementSyntax(
                    returnStatement.ReturnToken,
                    renamedValue);

                return true;
            }

            renamedReturnStatement = null;
            return false;
        }
    }
}