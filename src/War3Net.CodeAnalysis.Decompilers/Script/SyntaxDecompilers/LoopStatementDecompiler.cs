// ------------------------------------------------------------------------------
// <copyright file="LoopStatementDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass;
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

            if (!TryDecompileActionStatementList(loopStatement.Statements, out var loopActions))
            {
                return false;
            }

            functions.Add(DecompileCustomScriptAction(JassKeyword.Loop));
            functions.AddRange(loopActions);
            functions.Add(DecompileCustomScriptAction(JassKeyword.EndLoop));

            return true;
        }
    }
}