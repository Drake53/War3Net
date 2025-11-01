// ------------------------------------------------------------------------------
// <copyright file="ReturnStatementRenamer.cs" company="Drake53">
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
        private bool TryRenameReturnStatement(JassReturnStatementSyntax returnStatement, [NotNullWhen(true)] out JassStatementSyntax? renamedReturnStatement)
        {
            if (TryRenameExpression(returnStatement.Value, out var renamedValue))
            {
                renamedReturnStatement = new JassReturnStatementSyntax(
                    returnStatement.ReturnToken,
                    renamedValue);

                return true;
            }

            renamedReturnStatement = null;
            return false;
        }
    }
}