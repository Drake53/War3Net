// ------------------------------------------------------------------------------
// <copyright file="CallStatementDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileCallStatement(
            JassCallStatementSyntax callStatement,
            ref List<TriggerFunction> functions)
        {
            if (!Context.TriggerData.TriggerActions.TryGetValue(callStatement.IdentifierName.Token.Text, out var actions))
            {
                functions.Add(DecompileCustomScriptAction(callStatement.ToString()));
                return true;
            }

            var action = actions.First(action => action.ArgumentTypes.Length == callStatement.ArgumentList.ArgumentList.Items.Length);

            if (TryDecompileForEachLoopActionFunction(callStatement, action.ArgumentTypes, out var loopActionFunction))
            {
                functions.Add(loopActionFunction);
                return true;
            }

            var function = new TriggerFunction
            {
                Type = TriggerFunctionType.Action,
                IsEnabled = true,
                Name = action.FunctionName,
            };

            for (var j = 0; j < callStatement.ArgumentList.ArgumentList.Items.Length; j++)
            {
                if (TryDecompileTriggerFunctionParameter(callStatement.ArgumentList.ArgumentList.Items[j], action.ArgumentTypes[j], out var functionParameter))
                {
                    function.Parameters.Add(functionParameter);
                }
                else
                {
                    functions.Add(DecompileCustomScriptAction(callStatement.ToString()));
                    return true;
                }
            }

            functions.Add(function);
            return true;
        }
    }
}