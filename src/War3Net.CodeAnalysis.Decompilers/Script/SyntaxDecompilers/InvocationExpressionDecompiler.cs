// ------------------------------------------------------------------------------
// <copyright file="InvocationExpressionDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileInvocationExpression(
            JassInvocationExpressionSyntax invocationExpression,
            string expectedType,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (TryDecompileTriggerFunctionParameterPreset(invocationExpression.ToString(), expectedType, out _, out functionParameter))
            {
                return true;
            }

            if (Context.TriggerData.TriggerData.TriggerCalls.TryGetValue(invocationExpression.IdentifierName.Name, out var triggerCall))
            {
                if (string.Equals(triggerCall.ReturnType, expectedType, StringComparison.Ordinal) &&
                    TryDecompileTriggerCallFunction(invocationExpression, triggerCall, out var callFunction))
                {
                    functionParameter = new TriggerFunctionParameter
                    {
                        Type = TriggerFunctionParameterType.Function,
                        Value = invocationExpression.IdentifierName.Name,
                        Function = callFunction,
                    };

                    return true;
                }
            }

            if (string.Equals(invocationExpression.IdentifierName.Name, "Condition", StringComparison.Ordinal))
            {
                if (invocationExpression.Arguments.Arguments.Length == 1 &&
                    invocationExpression.Arguments.Arguments[0] is JassFunctionReferenceExpressionSyntax functionReferenceExpression &&
                    Context.FunctionDeclarations.TryGetValue(functionReferenceExpression.IdentifierName.Name, out var conditionFunctionDeclaration) &&
                    conditionFunctionDeclaration.IsConditionsFunction)
                {
                    var conditionFunction = conditionFunctionDeclaration.FunctionDeclaration;

                    if (conditionFunction.Body.Statements.Length == 1 &&
                        TryDecompileConditionStatement(conditionFunction.Body.Statements[0], true, out var function))
                    {
                        functionParameter = new TriggerFunctionParameter
                        {
                            Type = TriggerFunctionParameterType.Function,
                            Value = string.Empty,
                            Function = function,
                        };

                        return true;
                    }
                }
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileInvocationExpression(
            JassInvocationExpressionSyntax invocationExpression,
            [NotNullWhen(true)] out List<DecompileOption>? decompileOptions)
        {
            var result = new List<DecompileOption>();

            if (Context.TriggerData.TriggerParams.TryGetValue(string.Empty, out var triggerParamsForAllTypes) &&
                triggerParamsForAllTypes.TryGetValue(invocationExpression.ToString(), out var triggerParams))
            {
                foreach (var triggerParam in triggerParams)
                {
                    result.Add(new DecompileOption
                    {
                        Type = triggerParam.VariableType,
                        Parameter = new TriggerFunctionParameter
                        {
                            Type = TriggerFunctionParameterType.Preset,
                            Value = triggerParam.ParameterName,
                        },
                    });
                }
            }

            if (Context.TriggerData.TriggerData.TriggerCalls.TryGetValue(invocationExpression.IdentifierName.Name, out var triggerCall) &&
                TryDecompileTriggerCallFunction(invocationExpression, triggerCall, out var callFunction))
            {
                result.Add(new DecompileOption
                {
                    Type = triggerCall.ReturnType,
                    Parameter = new TriggerFunctionParameter
                    {
                        Type = TriggerFunctionParameterType.Function,
                        Value = invocationExpression.IdentifierName.Name,
                        Function = callFunction,
                    },
                });
            }

            if (result.Count > 0)
            {
                decompileOptions = result;
                return true;
            }

            decompileOptions = null;
            return false;
        }
    }
}