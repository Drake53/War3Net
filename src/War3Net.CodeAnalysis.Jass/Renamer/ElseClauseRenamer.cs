// ------------------------------------------------------------------------------
// <copyright file="ElseClauseRenamer.cs" company="Drake53">
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
        private bool TryRenameElseClause(JassElseClauseSyntax? elseClause, [NotNullWhen(true)] out JassElseClauseSyntax? renamedElseClause)
        {
            if (elseClause is not null &&
                TryRenameStatementList(elseClause.Body, out var renamedBody))
            {
                renamedElseClause = new JassElseClauseSyntax(renamedBody);
                return true;
            }

            renamedElseClause = null;
            return false;
        }
    }
}