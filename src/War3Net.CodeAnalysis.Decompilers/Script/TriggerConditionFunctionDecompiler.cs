﻿// ------------------------------------------------------------------------------
// <copyright file="TriggerConditionFunctionDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileConditionExpression(IExpressionSyntax expression, [NotNullWhen(true)] out TriggerFunction? conditionFunction)
        {
            expression = expression.Deparenthesize();

            if (expression is JassInvocationExpressionSyntax invocationExpression)
            {
                if (string.Equals(invocationExpression.IdentifierName.Name, "GetBooleanAnd", StringComparison.Ordinal) ||
                    string.Equals(invocationExpression.IdentifierName.Name, "GetBooleanOr", StringComparison.Ordinal))
                {
                    if (invocationExpression.Arguments.Arguments.Length == 2)
                    {
                        var function = new TriggerFunction
                        {
                            Type = TriggerFunctionType.Condition,
                            IsEnabled = true,
                            Name = invocationExpression.IdentifierName.Name,
                        };

                        foreach (var argument in invocationExpression.Arguments.Arguments)
                        {
                            if (TryDecompileConditionExpression(argument, out var conditionSubFunction))
                            {
                                function.Parameters.Add(new TriggerFunctionParameter
                                {
                                    Type = TriggerFunctionParameterType.Function,
                                    Value = string.Empty,
                                    Function = conditionSubFunction,
                                });
                            }
                            else
                            {
                                conditionFunction = null;
                                return false;
                            }
                        }

                        conditionFunction = function;
                        return true;
                    }
                    else
                    {
                        conditionFunction = null;
                        return false;
                    }
                }
                else
                {
                    if (invocationExpression.Arguments.Arguments.IsEmpty &&
                        Context.FunctionDeclarations.TryGetValue(invocationExpression.IdentifierName.Name, out var conditionsFunctionDeclaration) &&
                        conditionsFunctionDeclaration.IsConditionsFunction)
                    {
                        var conditionsFunction = conditionsFunctionDeclaration.FunctionDeclaration;

                        if (TryDecompileAndOrMultipleStatementList(conditionsFunction.Body, out conditionFunction))
                        {
                            return true;
                        }

                        if (conditionsFunction.Body.Statements.Length == 1)
                        {
                            if (conditionsFunction.Body.Statements[0] is JassReturnStatementSyntax singleReturnStatement)
                            {
                                return TryDecompileReturnStatement(singleReturnStatement, out conditionFunction);
                            }
                            else
                            {
                                conditionFunction = null;
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        conditionFunction = null;
                        return false;
                    }
                }
            }
            else if (expression is JassBinaryExpressionSyntax binaryExpression)
            {
                var function = new TriggerFunction
                {
                    Type = TriggerFunctionType.Condition,
                    IsEnabled = true,
                };

                if (TryDecompileTriggerFunctionParameter(binaryExpression.Left, out var leftDecompileOptions) &&
                    TryDecompileTriggerFunctionParameter(binaryExpression.Right, out var rightDecompileOptions))
                {
                    if (TryGetBinaryOperandsDecompileOptions(leftDecompileOptions, rightDecompileOptions, out var decompileOptions))
                    {
                        foreach (var decompileOption in decompileOptions)
                        {
                            if (Context.TriggerData.TriggerConditions.TryGetValue(decompileOption.Type, out var triggerCondition) &&
                                triggerCondition.ArgumentTypes.Length == 3)
                            {
                                if (!TryDecompileTriggerFunctionParameter(binaryExpression.Operator, triggerCondition.ArgumentTypes[1], out var operatorFunctionParameter))
                                {
                                    continue;
                                }

                                function.Name = triggerCondition.FunctionName;
                                function.Parameters.Add(decompileOption.LeftParameter);
                                function.Parameters.Add(operatorFunctionParameter);
                                function.Parameters.Add(decompileOption.RightParameter);

                                conditionFunction = function;
                                return true;
                            }
                        }
                    }
                }
            }

            conditionFunction = null;
            return false;
        }
    }
}