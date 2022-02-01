// ------------------------------------------------------------------------------
// <copyright file="StatementTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaStatementSyntax Transpile(IStatementSyntax statement)
        {
            return statement switch
            {
                JassEmptySyntax empty => Transpile(empty),
                JassCommentSyntax comment => Transpile(comment),
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