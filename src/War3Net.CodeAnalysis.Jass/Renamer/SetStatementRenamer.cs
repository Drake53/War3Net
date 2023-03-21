// ------------------------------------------------------------------------------
// <copyright file="SetStatementRenamer.cs" company="Drake53">
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
        private bool TryRenameSetStatement(JassSetStatementSyntax setStatement, [NotNullWhen(true)] out JassStatementSyntax? renamedSetStatement)
        {
            if (TryRenameVariableIdentifierName(setStatement.IdentifierName, out var renamedIdentifierName) |
                TryRenameElementAccessClause(setStatement.ElementAccessClause, out var renamedElementAccessClause) |
                TryRenameEqualsValueClause(setStatement.Value, out var renamedValue))
            {
                renamedSetStatement = new JassSetStatementSyntax(
                    setStatement.SetToken,
                    renamedIdentifierName ?? setStatement.IdentifierName,
                    renamedElementAccessClause ?? setStatement.ElementAccessClause,
                    renamedValue ?? setStatement.Value);

                return true;
            }

            renamedSetStatement = null;
            return false;
        }
    }
}