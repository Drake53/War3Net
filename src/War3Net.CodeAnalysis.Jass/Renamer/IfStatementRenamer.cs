// ------------------------------------------------------------------------------
// <copyright file="IfStatementRenamer.cs" company="Drake53">
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