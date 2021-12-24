// ------------------------------------------------------------------------------
// <copyright file="StatementRenamer.cs" company="Drake53">
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
        private bool TryRenameStatement(IStatementSyntax statement, [NotNullWhen(true)] out IStatementSyntax? renamedStatement)
        {
            return statement switch
            {
                JassLocalVariableDeclarationStatementSyntax localVariableDeclarationStatement => TryRenameLocalVariableDeclarationStatement(localVariableDeclarationStatement, out renamedStatement),
                JassSetStatementSyntax setStatement => TryRenameSetStatement(setStatement, out renamedStatement),
                JassCallStatementSyntax callStatement => TryRenameCallStatement(callStatement, out renamedStatement),
                JassIfStatementSyntax ifStatement => TryRenameIfStatement(ifStatement, out renamedStatement),
                JassLoopStatementSyntax loopStatement => TryRenameLoopStatement(loopStatement, out renamedStatement),
                JassExitStatementSyntax exitStatement => TryRenameExitStatement(exitStatement, out renamedStatement),
                JassReturnStatementSyntax returnStatement => TryRenameReturnStatement(returnStatement, out renamedStatement),
                JassDebugStatementSyntax debugStatement => TryRenameDebugStatement(debugStatement, out renamedStatement),

                _ => TryRenameDummy(statement, out renamedStatement),
            };
        }
    }
}