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
            if (loopStatement.Statements.Length == 2 &&
                loopStatement.Statements[0] is JassExitStatementSyntax exitStatement &&
                exitStatement.Condition.Deparenthesize() is JassInvocationExpressionSyntax exitInvocationExpression &&
                exitInvocationExpression.ArgumentList.ArgumentList.Items.IsEmpty &&
                loopStatement.Statements[1] is JassCallStatementSyntax callStatement &&
                string.Equals(callStatement.IdentifierName.Token.Text, "TriggerSleepAction", StringComparison.Ordinal) &&
                callStatement.ArgumentList.ArgumentList.Items.Length == 1 &&
                callStatement.ArgumentList.ArgumentList.Items[0] is JassInvocationExpressionSyntax callInvocationExpression &&
                string.Equals(callInvocationExpression.IdentifierName.Token.Text, "RMaxBJ", StringComparison.Ordinal) &&
                callInvocationExpression.ArgumentList.ArgumentList.Items.Length == 2 &&
                callInvocationExpression.ArgumentList.ArgumentList.Items[0].TryGetIdentifierNameValue(out var variableName) &&
                string.Equals(variableName, "bj_WAIT_FOR_COND_MIN_INTERVAL", StringComparison.Ordinal) &&
                Context.FunctionDeclarations.TryGetValue(exitInvocationExpression.IdentifierName.Token.Text, out var exitFunctionDeclaration) &&
                exitFunctionDeclaration.IsConditionsFunction)
            {
                var exitFunction = exitFunctionDeclaration.FunctionDeclaration;

                if (exitFunction.Statements.Length == 1 &&
                    TryDecompileConditionStatement(exitFunction.Statements[0], true, out var conditionFunction) &&
                    TryDecompileTriggerFunctionParameter(callInvocationExpression.ArgumentList.ArgumentList.Items[1], JassKeyword.Real, out var intervalParameter))
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