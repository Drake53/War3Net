// ------------------------------------------------------------------------------
// <copyright file="StatementTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public StatementSyntax Transpile(JassStatementSyntax statement)
        {
            return statement switch
            {
                JassLocalVariableDeclarationStatementSyntax localVariableDeclarationStatement => Transpile(localVariableDeclarationStatement),
                JassSetStatementSyntax setStatement => Transpile(setStatement),
                JassCallStatementSyntax callStatement => Transpile(callStatement),
                JassIfStatementSyntax ifStatement => Transpile(ifStatement),
                JassLoopStatementSyntax loopStatement => Transpile(loopStatement),
                JassExitStatementSyntax exitStatement => Transpile(exitStatement),
                JassReturnStatementSyntax returnStatement => Transpile(returnStatement),
                JassDebugStatementSyntax debugStatement => Transpile(debugStatement),
            };
        }
    }
}