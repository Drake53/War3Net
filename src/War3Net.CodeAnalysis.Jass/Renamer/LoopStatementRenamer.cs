// ------------------------------------------------------------------------------
// <copyright file="LoopStatementRenamer.cs" company="Drake53">
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
        private bool TryRenameLoopStatement(JassLoopStatementSyntax loopStatement, [NotNullWhen(true)] out IStatementSyntax? renamedLoopStatement)
        {
            if (TryRenameStatementList(loopStatement.Body, out var renamedBody))
            {
                renamedLoopStatement = new JassLoopStatementSyntax(renamedBody);
                return true;
            }

            renamedLoopStatement = null;
            return false;
        }
    }
}