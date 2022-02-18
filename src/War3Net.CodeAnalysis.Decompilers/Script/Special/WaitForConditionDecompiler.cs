// ------------------------------------------------------------------------------
// <copyright file="WaitForConditionDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileWaitForConditionActionFunction(JassLoopStatementSyntax loopStatement, [NotNullWhen(true)] out TriggerFunction? actionFunction)
        {
            if (loopStatement.Body.Statements.Length == 2 &&
                loopStatement.Body.Statements[0] is JassExitStatementSyntax exitStatement &&
                exitStatement.Condition.Deparenthesize() is JassInvocationExpressionSyntax exitInvocationExpression &&
                exitInvocationExpression.Arguments.Arguments.IsEmpty &&
                loopStatement.Body.Statements[1] is JassCallStatementSyntax callStatement &&
                string.Equals(callStatement.IdentifierName.Name, "TriggerSleepAction", StringComparison.Ordinal) &&
                callStatement.Arguments.Arguments.Length == 1 &&
                callStatement.Arguments.Arguments[0] is JassInvocationExpressionSyntax callInvocationExpression &&
                string.Equals(callInvocationExpression.IdentifierName.Name, "RMaxBJ", StringComparison.Ordinal) &&
                callInvocationExpression.Arguments.Arguments.Length == 2 &&
                callInvocationExpression.Arguments.Arguments[0] is JassVariableReferenceExpressionSyntax variableReferenceExpression &&
                string.Equals(variableReferenceExpression.IdentifierName.Name, "bj_WAIT_FOR_COND_MIN_INTERVAL", StringComparison.Ordinal) &&
                Context.FunctionDeclarations.TryGetValue(exitInvocationExpression.IdentifierName.Name, out var exitFunctionDeclaration) &&
                exitFunctionDeclaration.IsConditionsFunction)
            {
                var exitFunction = exitFunctionDeclaration.FunctionDeclaration;

                if (exitFunction.Body.Statements.Length == 1 &&
                    TryDecompileConditionStatement(exitFunction.Body.Statements[0], true, out var conditionFunction) &&
                    TryDecompileTriggerFunctionParameter(callInvocationExpression.Arguments.Arguments[1], JassKeyword.Real, out var intervalParameter))
                {
                    actionFunction = new TriggerFunction
                    {
                        Name = "WaitForCondition",
                        IsEnabled = true,
                        Type = TriggerFunctionType.Action,
                    };

                    actionFunction.Parameters.Add(new TriggerFunctionParameter
                    {
                        Type = TriggerFunctionParameterType.Function,
                        Value = string.Empty,
                        Function = conditionFunction,
                    });

                    actionFunction.Parameters.Add(intervalParameter);

                    return true;
                }
            }

            actionFunction = null;
            return false;
        }
    }
}