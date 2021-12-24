// ------------------------------------------------------------------------------
// <copyright file="ExitStatementRenamer.cs" company="Drake53">
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
        private bool TryRenameExitStatement(JassExitStatementSyntax exitStatement, [NotNullWhen(true)] out IStatementSyntax? renamedExitStatement)
        {
            if (TryRenameExpression(exitStatement.Condition, out var renamedCondition))
            {
                renamedExitStatement = new JassExitStatementSyntax(renamedCondition);
                return true;
            }

            renamedExitStatement = null;
            return false;
        }
    }
}