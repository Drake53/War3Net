// ------------------------------------------------------------------------------
// <copyright file="TriggerActionFunctionDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileTriggerActionFunctions(JassStatementListSyntax statementList, [NotNullWhen(true)] out List<TriggerFunction>? actionFunctions)
        {
            var result = new List<TriggerFunction>();

            for (var i = 0; i < statementList.Statements.Length; i++)
            {
                if (!TryDecompileStatement(statementList, ref i, ref result))
                {
                    if (statementList.Statements[i] is IStatementLineSyntax statementLine)
                    {
                        result.Add(DecompileCustomScriptAction(statementLine));
                    }
                    else
                    {
                        actionFunctions = null;
                        return false;
                    }
                }
            }

            actionFunctions = result;
            return true;
        }
    }
}