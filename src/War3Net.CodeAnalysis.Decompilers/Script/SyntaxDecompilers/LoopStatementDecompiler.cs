using System.Collections.Generic;
using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileLoopStatement(
            JassLoopStatementSyntax loopStatement,
            ref List<TriggerFunction> functions)
        {
            if (TryDecompileWaitForConditionActionFunction(loopStatement, out var waitForConditionFunction))
            {
                functions.Add(waitForConditionFunction);
                return true;
            }

            if (!TryDecompileActionStatements(loopStatement.Statements, out var loopActions))
            {
                return false;
            }

            functions.Add(DecompileCustomScriptAction(loopStatement.LoopToken.ToString()));
            functions.AddRange(loopActions);
            DecompileLeadingTrivia(loopStatement.EndLoopToken.LeadingTrivia, ref functions);
            functions.Add(DecompileCustomScriptAction(loopStatement.EndLoopToken.ToString()));

            return true;
        }
    }
}