// ------------------------------------------------------------------------------
// <copyright file="SetStatementDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileSetStatement(
            JassSetStatementSyntax setStatement,
            ImmutableArray<JassStatementSyntax> statements,
            ref int i,
            ref List<TriggerFunction> functions)
        {
            var lookaheadStatement1 = i + 1 < statements.Length ? statements[i + 1] : null;
            var lookaheadStatement2 = i + 2 < statements.Length ? statements[i + 2] : null;

            if (TryDecompileForLoopActionFunction(setStatement, lookaheadStatement1, lookaheadStatement2, ref functions))
            {
                i += 2;
                return true;
            }
            else if (TryDecompileForLoopVarActionFunction(setStatement, lookaheadStatement1, ref functions))
            {
                i += 1;
                return true;
            }
            else if (TryDecompileTriggerFunctionParameterVariable(setStatement, out var variableFunctionParameter, out var variableType) &&
                     TryDecompileTriggerFunctionParameter(setStatement.Value.Expression, variableType, out var valueFunctionParameter))
            {
                var function = new TriggerFunction
                {
                    Type = TriggerFunctionType.Action,
                    IsEnabled = true,
                    Name = "SetVariable",
                };

                function.Parameters.Add(variableFunctionParameter);
                function.Parameters.Add(valueFunctionParameter);

                functions.Add(function);
                return true;
            }
            else
            {
                functions.Add(DecompileCustomScriptAction(setStatement.ToString()));
                return true;
            }
        }
    }
}