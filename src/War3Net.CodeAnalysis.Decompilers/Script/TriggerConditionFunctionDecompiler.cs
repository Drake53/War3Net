// ------------------------------------------------------------------------------
// <copyright file="TriggerConditionFunctionDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Decompilers.Extensions;
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        /// <param name="returnValue"><see langword="true"/> for AND conditions, <see langword="false"/> for OR conditions.</param>
        private bool TryDecompileTriggerConditionFunction(IStatementSyntax statement, bool returnValue, [NotNullWhen(true)] out TriggerFunction? conditionFunction)
        {
            if (statement is JassIfStatementSyntax ifStatement)
            {
                if (ifStatement.ElseIfClauses.IsEmpty &&
                    ifStatement.ElseClause is null &&
                    ifStatement.Body.Statements.Length == 1 &&
                    ifStatement.Body.Statements[0] is JassReturnStatementSyntax returnStatement &&
                    returnStatement.Value is JassBooleanLiteralExpressionSyntax booleanLiteralExpression &&
                    booleanLiteralExpression.Value != returnValue)
                {
                    var conditionExpression = ifStatement.Condition.Deparenthesize();

                    if (returnValue)
                    {
                        if (conditionExpression is JassUnaryExpressionSyntax unaryExpression &&
                            unaryExpression.Operator == UnaryOperatorType.Not)
                        {
                            conditionExpression = unaryExpression.Expression.Deparenthesize();
                        }
                        else
                        {
                            conditionFunction = null;
                            return false;
                        }
                    }

                    return TryDecompileConditionExpression(conditionExpression, out conditionFunction);
                }
                else
                {
                    conditionFunction = null;
                    return false;
                }
            }
            else if (statement is JassReturnStatementSyntax returnStatement)
            {
                var returnExpression = returnStatement.Value.Deparenthesize();

                return TryDecompileTriggerConditionFunction(returnExpression, out conditionFunction);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

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

                        if (conditionsFunction.Body.Statements.Length == 1)
                        {
                            if (conditionsFunction.Body.Statements[0] is JassReturnStatementSyntax singleReturnStatement)
                            {
                                return TryDecompileTriggerConditionFunction(singleReturnStatement, true, out conditionFunction);
                            }
                            else
                            {
                                conditionFunction = null;
                                return false;
                            }
                        }
                        else
                        {
                            // Last statement must be "return true" or "return false"
                            if (conditionsFunction.Body.Statements[^1] is not JassReturnStatementSyntax finalReturnStatement ||
                                finalReturnStatement.Value is not JassBooleanLiteralExpressionSyntax returnBooleanLiteralExpression)
                            {
                                conditionFunction = null;
                                return false;
                            }

                            var function = new TriggerFunction
                            {
                                Type = TriggerFunctionType.Condition,
                                IsEnabled = true,
                                Name = returnBooleanLiteralExpression.Value ? "AndMultiple" : "OrMultiple",
                            };

                            foreach (var conditionStatement in conditionsFunction.Body.Statements.SkipLast(1))
                            {
                                if (TryDecompileTriggerConditionFunction(conditionStatement, returnBooleanLiteralExpression.Value, out var conditionSubFunction))
                                {
                                    conditionSubFunction.Branch = 0;
                                    function.ChildFunctions.Add(conditionSubFunction);
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
                    }
                    else
                    {
                        conditionFunction = null;
                        return false;
                    }
                }
            }
            else
            {
                return TryDecompileTriggerConditionFunction(expression, out conditionFunction);
            }
        }

        private bool TryDecompileTriggerConditionFunction(IExpressionSyntax compareExpression, [NotNullWhen(true)] out TriggerFunction? conditionFunction)
        {
            conditionFunction = null;

            if (compareExpression is JassBinaryExpressionSyntax binaryExpression)
            {
                var function = new TriggerFunction
                {
                    Type = TriggerFunctionType.Condition,
                    IsEnabled = true,
                };

                if (TryDecompileOperatorCompareOperand(binaryExpression.Left, out var leftFunctionParameter, out var leftTriggerCondition) &&
                    TryDecompileOperatorCompareOperand(binaryExpression.Right, out var rightFunctionParameter, out var rightTriggerCondition))
                {
                    if (leftTriggerCondition is null)
                    {
                        if (rightTriggerCondition is null)
                        {
                            return false;
                        }
                        else
                        {
                            leftTriggerCondition = rightTriggerCondition;
                        }
                    }
                    else if (rightTriggerCondition is not null && !ReferenceEquals(leftTriggerCondition, rightTriggerCondition))
                    {
                        return false;
                    }

                    if (Context.TriggerData.TriggerParams.TryGetValue(leftTriggerCondition.ArgumentTypes[1], out var triggerParamsForType) &&
                        triggerParamsForType.TryGetValue($"\"{binaryExpression.Operator.GetSymbol()}\"", out var triggerParams))
                    {
                        var operatorFunctionParameter = new TriggerFunctionParameter
                        {
                            Type = TriggerFunctionParameterType.Preset,
                            Value = triggerParams.Single().ParameterName,
                        };

                        function.Name = leftTriggerCondition.FunctionName;
                        function.Parameters.Add(leftFunctionParameter);
                        function.Parameters.Add(operatorFunctionParameter);
                        function.Parameters.Add(rightFunctionParameter);

                        conditionFunction = function;
                        return true;
                    }
                    else
                    {
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
                return false;
            }
        }

        private bool TryDecompileOperatorCompareOperand(
            IExpressionSyntax expression,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter,
            out TriggerData.TriggerCondition? triggerCondition)
        {
            var expressionString = expression.ToString();
            if (!string.Equals(expressionString, "0", StringComparison.Ordinal) &&
                Context.TriggerData.TryGetTriggerConditionForUnknownType(expressionString, out triggerCondition, out var triggerParam))
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.Preset,
                    Value = triggerParam.ParameterName,
                };

                return true;
            }
            else if (expression is JassInvocationExpressionSyntax invocationExpression &&
                     Context.TriggerData.TriggerData.TriggerCalls.TryGetValue(invocationExpression.IdentifierName.Name, out var triggerCall) &&
                     Context.TriggerData.TriggerConditions.TryGetValue(triggerCall.ReturnType, out triggerCondition))
            {
                if (TryDecompileTriggerCallFunction(invocationExpression, out var callFunction))
                {
                    functionParameter = new TriggerFunctionParameter
                    {
                        Type = TriggerFunctionParameterType.Function,
                        Value = invocationExpression.IdentifierName.Name,
                        Function = callFunction,
                    };

                    return true;
                }
                else
                {
                    functionParameter = null;
                    triggerCondition = null;
                    return false;
                }
            }
            else if (TryDecompileTriggerFunctionParameterStringForUnknownType(expression, out functionParameter, out var literalType))
            {
                if (!string.IsNullOrEmpty(literalType))
                {
                    if (Context.TriggerData.TriggerConditions.TryGetValue(literalType, out triggerCondition))
                    {
                        return true;
                    }
                    else
                    {
                        functionParameter = null;
                        triggerCondition = null;
                        return false;
                    }
                }
                else
                {
                    triggerCondition = null;
                    return true;
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}