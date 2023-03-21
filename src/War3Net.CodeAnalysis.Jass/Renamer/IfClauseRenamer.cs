// ------------------------------------------------------------------------------
// <copyright file="IfClauseRenamer.cs" company="Drake53">
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