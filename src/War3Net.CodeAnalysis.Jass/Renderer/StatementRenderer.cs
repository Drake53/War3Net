// ------------------------------------------------------------------------------
// <copyright file="StatementRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassStatementSyntax statement)
        {
            switch (statement)
            {
                case JassLocalVariableDeclarationStatementSyntax localVariableDeclarationStatement: Render(localVariableDeclarationStatement); break;
                case JassSetStatementSyntax setStatement: Render(setStatement); break;
                case JassCallStatementSyntax callStatement: Render(callStatement); break;
                case JassIfStatementSyntax ifStatement: Render(ifStatement); break;
                case JassLoopStatementSyntax loopStatement: Render(loopStatement); break;
                case JassExitStatementSyntax exitStatement: Render(exitStatement); break;
                case JassReturnStatementSyntax returnStatement: Render(returnStatement); break;
                case JassDebugStatementSyntax debugStatement: Render(debugStatement); break;

                default: throw new NotSupportedException();
            }
        }
    }
}