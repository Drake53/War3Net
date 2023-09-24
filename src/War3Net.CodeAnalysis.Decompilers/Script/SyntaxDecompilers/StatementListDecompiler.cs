// ------------------------------------------------------------------------------
// <copyright file="StatementListDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileActionStatementList(ImmutableArray<JassStatementSyntax> statements, [NotNullWhen(true)] out List<TriggerFunction>? actionFunctions)
        {
            var result = new List<TriggerFunction>();

            for (var i = 0; i < statements.Length; i++)
            {
                if (!TryDecompileActionStatement(statements, ref i, ref result))
                {
                    actionFunctions = null;
                    return false;
                }
            }

            actionFunctions = result;
            return true;
        }

        private bool TryDecompileConditionStatementList(
            ImmutableArray<JassStatementSyntax> statements,
            [NotNullWhen(true)] out List<TriggerFunction>? conditionFunctions)
        {
            var result = new List<TriggerFunction>();

            foreach (var conditionStatement in statements.SkipLast(1))
            {
                if (TryDecompileConditionStatement(conditionStatement, true, out var conditionFunction))
                {
                    result.Add(conditionFunction);
                }
                else
                {
                    conditionFunctions = null;
                    return false;
                }
            }

            // Last statement must be "return true"
            if (statements[^1] is not JassReturnStatementSyntax finalReturnStatement ||
                finalReturnStatement.Value is null ||
                !finalReturnStatement.Value.TryGetBooleanExpressionValue(out var returnValue) ||
                !returnValue)
            {
                conditionFunctions = null;
                return false;
            }

            conditionFunctions = result;
            return true;
        }

        private bool TryDecompileAndOrMultipleStatementList(
            ImmutableArray<JassStatementSyntax> statements,
            [NotNullWhen(true)] out TriggerFunction? conditionFunction)
        {
            var result = new TriggerFunction
            {
                Type = TriggerFunctionType.Condition,
                IsEnabled = true,
            };

            // Last statement must be "return true" or "return false"
            if (statements[^1] is not JassReturnStatementSyntax finalReturnStatement ||
                finalReturnStatement.Value is null ||
                !finalReturnStatement.Value.TryGetBooleanExpressionValue(out var returnValue))
            {
                conditionFunction = null;
                return false;
            }

            result.Name = returnValue ? "AndMultiple" : "OrMultiple";

            foreach (var conditionStatement in statements.SkipLast(1))
            {
                if (TryDecompileConditionStatement(conditionStatement, returnValue, out var function))
                {
                    function.Branch = 0;
                    result.ChildFunctions.Add(function);
                }
                else
                {
                    conditionFunction = null;
                    return false;
                }
            }

            conditionFunction = result;
            return true;
        }
    }
}