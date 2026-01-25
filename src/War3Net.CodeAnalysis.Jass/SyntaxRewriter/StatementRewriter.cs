// ------------------------------------------------------------------------------
// <copyright file="StatementRewriter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="statement">The <see cref="JassStatementSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassStatementSyntax"/>, or the input <paramref name="statement"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="statement"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteStatement(JassStatementSyntax statement, out JassStatementSyntax result)
        {
            return statement switch
            {
                JassCallStatementSyntax callStatement => RewriteCallStatement(callStatement, out result),
                JassDebugStatementSyntax debugStatement => RewriteDebugStatement(debugStatement, out result),
                JassExitStatementSyntax exitStatement => RewriteExitStatement(exitStatement, out result),
                JassIfStatementSyntax ifStatement => RewriteIfStatement(ifStatement, out result),
                JassLocalVariableDeclarationStatementSyntax localVariableDeclarationStatement => RewriteLocalVariableDeclarationStatement(localVariableDeclarationStatement, out result),
                JassLoopStatementSyntax loopStatement => RewriteLoopStatement(loopStatement, out result),
                JassReturnStatementSyntax returnStatement => RewriteReturnStatement(returnStatement, out result),
                JassSetStatementSyntax setStatement => RewriteSetStatement(setStatement, out result),
            };
        }
    }
}