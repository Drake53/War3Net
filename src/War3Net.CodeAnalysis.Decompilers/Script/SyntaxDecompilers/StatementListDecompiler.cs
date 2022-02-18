// ------------------------------------------------------------------------------
// <copyright file="StatementListDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileActionStatementList(JassStatementListSyntax statementList, [NotNullWhen(true)] out List<TriggerFunction>? actionFunctions)
        {
            var result = new List<TriggerFunction>();

            for (var i = 0; i < statementList.Statements.Length; i++)
            {
                if (!TryDecompileActionStatement(statementList, ref i, ref result))
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

        private bool TryDecompileConditionStatementList(
            JassStatementListSyntax statementList,
            [NotNullWhen(true)] out List<TriggerFunction>? conditionFunctions)
        {
            var result = new List<TriggerFunction>();

            foreach (var conditionStatement in statementList.Statements.SkipLast(1))
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
            if (statementList.Statements[^1] is not JassReturnStatementSyntax finalReturnStatement ||
                finalReturnStatement.Value is not JassBooleanLiteralExpressionSyntax returnBooleanLiteralExpression ||
                !returnBooleanLiteralExpression.Value)
            {
                conditionFunctions = null;
                return false;
            }

            conditionFunctions = result;
            return true;
        }

        private bool TryDecompileAndOrMultipleStatementList(
            JassStatementListSyntax statementList,
            [NotNullWhen(true)] out TriggerFunction? conditionFunction)
        {
            var result = new TriggerFunction
            {
                Type = TriggerFunctionType.Condition,
                IsEnabled = true,
            };

            // Last statement must be "return true" or "return false"
            if (statementList.Statements[^1] is not JassReturnStatementSyntax finalReturnStatement ||
                finalReturnStatement.Value is not JassBooleanLiteralExpressionSyntax returnBooleanLiteralExpression)
            {
                conditionFunction = null;
                return false;
            }

            result.Name = returnBooleanLiteralExpression.Value ? "AndMultiple" : "OrMultiple";

            foreach (var conditionStatement in statementList.Statements.SkipLast(1))
            {
                if (TryDecompileConditionStatement(conditionStatement, returnBooleanLiteralExpression.Value, out var function))
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