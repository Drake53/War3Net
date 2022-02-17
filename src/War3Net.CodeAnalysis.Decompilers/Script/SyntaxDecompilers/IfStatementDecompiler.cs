// ------------------------------------------------------------------------------
// <copyright file="IfStatementDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileIfStatement(
            JassIfStatementSyntax ifStatement,
            ref List<TriggerFunction> functions)
        {
            if (TryDecompileIfThenElseActionFunction(ifStatement, out var function))
            {
                functions.Add(function);
                return true;
            }

            functions.Add(DecompileCustomScriptAction(new JassIfCustomScriptAction(ifStatement.Condition)));

            if (TryDecompileActionStatementList(ifStatement.Body, out var thenActions))
            {
                functions.AddRange(thenActions);
            }
            else
            {
                return false;
            }

            foreach (var elseIfClause in ifStatement.ElseIfClauses)
            {
                functions.Add(DecompileCustomScriptAction(new JassElseIfCustomScriptAction(elseIfClause.Condition)));

                if (TryDecompileActionStatementList(elseIfClause.Body, out var elseIfActions))
                {
                    functions.AddRange(elseIfActions);
                }
                else
                {
                    return false;
                }
            }

            if (ifStatement.ElseClause is not null)
            {
                functions.Add(DecompileCustomScriptAction(JassElseCustomScriptAction.Value));

                if (TryDecompileActionStatementList(ifStatement.ElseClause.Body, out var elseActions))
                {
                    functions.AddRange(elseActions);
                }
                else
                {
                    return false;
                }
            }

            functions.Add(DecompileCustomScriptAction(JassEndIfCustomScriptAction.Value));

            return true;
        }
    }
}