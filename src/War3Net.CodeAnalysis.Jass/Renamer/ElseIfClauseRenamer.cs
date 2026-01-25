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
            if (TryRenameElseIfClauseDeclarator(elseIfClause.ElseIfClauseDeclarator, out var renamedDeclarator) |
                TryRenameStatementList(elseIfClause.Statements, out var renamedStatements))
            {
                renamedElseIfClause = new JassElseIfClauseSyntax(
                    renamedDeclarator ?? elseIfClause.ElseIfClauseDeclarator,
                    renamedStatements ?? elseIfClause.Statements);

                return true;
            }

            renamedElseIfClause = null;
            return false;
        }
    }
}