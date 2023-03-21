// ------------------------------------------------------------------------------
// <copyright file="DebugStatementRenamer.cs" company="Drake53">
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
        private bool TryRenameDebugStatement(JassDebugStatementSyntax debugStatement, [NotNullWhen(true)] out JassStatementSyntax? renamedDebugStatement)
        {
            if (TryRenameStatement(debugStatement.Statement, out var renamedStatement))
            {
                renamedDebugStatement = new JassDebugStatementSyntax(
                    debugStatement.DebugToken,
                    renamedStatement);

                return true;
            }

            renamedDebugStatement = null;
            return false;
        }
    }
}