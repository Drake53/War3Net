// ------------------------------------------------------------------------------
// <copyright file="CustomScriptActionRenderer.cs" company="Drake53">
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
        public void Render(ICustomScriptAction customScriptAction)
        {
            switch (customScriptAction)
            {
                case JassEmptyStatementSyntax emptyStatement: Render(emptyStatement); break;
                case JassCommentStatementSyntax commentStatement: Render(commentStatement); break;
                case JassLocalVariableDeclarationStatementSyntax localVariableDeclarationStatement: Render(localVariableDeclarationStatement); break;
                case JassSetStatementSyntax setStatement: Render(setStatement); break;
                case JassCallStatementSyntax callStatement: Render(callStatement); break;
                case JassIfCustomScriptAction ifCustomScriptAction: Render(ifCustomScriptAction); break;
                case JassElseIfCustomScriptAction elseIfCustomScriptAction: Render(elseIfCustomScriptAction); break;
                case JassElseCustomScriptAction elseCustomScriptAction: Render(elseCustomScriptAction); break;
                case JassEndIfCustomScriptAction endIfCustomScriptAction: Render(endIfCustomScriptAction); break;
                case JassLoopCustomScriptAction loopCustomScriptAction: Render(loopCustomScriptAction); break;
                case JassEndLoopCustomScriptAction endLoopCustomScriptAction: Render(endLoopCustomScriptAction); break;
                case JassExitStatementSyntax exitStatement: Render(exitStatement); break;
                case JassReturnStatementSyntax returnStatement: Render(returnStatement); break;
                case JassFunctionCustomScriptAction functionCustomScriptAction: Render(functionCustomScriptAction); break;
                case JassEndFunctionCustomScriptAction functionCustomScriptAction: Render(functionCustomScriptAction); break;
                case JassDebugCustomScriptAction debugCustomScriptAction: Render(debugCustomScriptAction); break;

                default: throw new NotSupportedException();
            }
        }
    }
}