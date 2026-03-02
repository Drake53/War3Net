using System.Diagnostics.CodeAnalysis;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameElseClause(JassElseClauseSyntax? elseClause, [NotNullWhen(true)] out JassElseClauseSyntax? renamedElseClause)
        {
            if (elseClause is not null &&
                TryRenameStatementList(elseClause.Statements, out var renamedStatements))
            {
                renamedElseClause = new JassElseClauseSyntax(
                    elseClause.ElseToken,
                    renamedStatements.Value);

                return true;
            }

            renamedElseClause = null;
            return false;
        }
    }
}