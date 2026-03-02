using System.Diagnostics.CodeAnalysis;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameEqualsValueClause(JassEqualsValueClauseSyntax? equalsValueClause, [NotNullWhen(true)] out JassEqualsValueClauseSyntax? renamedEqualsValueClause)
        {
            if (equalsValueClause is not null &&
                TryRenameExpression(equalsValueClause.Expression, out var renamedExpression))
            {
                renamedEqualsValueClause = new JassEqualsValueClauseSyntax(
                    equalsValueClause.EqualsToken,
                    renamedExpression);

                return true;
            }

            renamedEqualsValueClause = null;
            return false;
        }
    }
}