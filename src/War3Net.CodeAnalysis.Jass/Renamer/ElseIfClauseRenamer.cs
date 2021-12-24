// ------------------------------------------------------------------------------
// <copyright file="ElseIfClauseRenamer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameElseIfClause(JassElseIfClauseSyntax elseIfClause, [NotNullWhen(true)] out JassElseIfClauseSyntax? renamedElseIfClause)
        {
            if (TryRenameExpression(elseIfClause.Condition, out var renamedCondition) |
                TryRenameStatementList(elseIfClause.Body, out var renamedBody))
            {
                renamedElseIfClause = new JassElseIfClauseSyntax(
                    renamedCondition ?? elseIfClause.Condition,
                    renamedBody ?? elseIfClause.Body);

                return true;
            }

            renamedElseIfClause = null;
            return false;
        }
    }
}