using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using War3Net.Build.Script;
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
                    var conditionExpression = DeparenthesizeExpression(ifStatement.Condition);

                    if (returnValue)
                    {
                        if (conditionExpression is JassUnaryExpressionSyntax unaryExpression &&
                            unaryExpression.Operator == UnaryOperatorType.Not)
                        {
                            conditionExpression = DeparenthesizeExpression(unaryExpression.Expression);
                        }
                        else
                        {
                            conditionFunction = null;
                            return false;
                        }
                    }

                    if (conditionExpression is JassInvocationExpressionSyntax invocationExpression)
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
                                    var conditionInvocationExpression = (JassInvocationExpressionSyntax)argument;
                                    var conditionsFunction = (JassFunctionDeclarationSyntax)Context.CompilationUnit.Declarations.Single(declaration =>
                                        declaration is JassFunctionDeclarationSyntax functionDeclaration &&
                                        string.Equals(functionDeclaration.FunctionDeclarator.IdentifierName.Name, conditionInvocationExpression.IdentifierName.Name, StringComparison.Ordinal));

                                    if (conditionsFunction.Body.Statements.Length == 1)
                                    {
                                        if (conditionsFunction.Body.Statements[0] is JassReturnStatementSyntax singleReturnStatement)
                                        {
                                            if (TryDecompileTriggerConditionFunction(singleReturnStatement, true, out var conditionSubFunction))
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
                                        else
                                        {
                                            conditionFunction = null;
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        foreach (var conditionStatement in conditionsFunction.Body.Statements.SkipLast(1))
                                        {
                                            if (TryDecompileTriggerConditionFunction(conditionStatement, true, out var conditionSubFunction))
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

                                        // Last statement must be "return true"
                                        if (conditionsFunction.Body.Statements.Last() is not JassReturnStatementSyntax finalReturnStatement ||
                                            finalReturnStatement.Value is not JassBooleanLiteralExpressionSyntax returnBooleanLiteralExpression ||
                                            !returnBooleanLiteralExpression.Value)
                                        {
                                            conditionFunction = null;
                                            return false;
                                        }
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
                            if (invocationExpression.Arguments.Arguments.IsEmpty)
                            {
                                var conditionsFunction = (JassFunctionDeclarationSyntax)Context.CompilationUnit.Declarations.Single(declaration =>
                                    declaration is JassFunctionDeclarationSyntax functionDeclaration &&
                                    string.Equals(functionDeclaration.FunctionDeclarator.IdentifierName.Name, invocationExpression.IdentifierName.Name, StringComparison.Ordinal));

                                // Last statement must be "return true" or "return false"
                                if (conditionsFunction.Body.Statements.Last() is not JassReturnStatementSyntax finalReturnStatement ||
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
                            else
                            {
                                conditionFunction = null;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return TryDecompileTriggerConditionFunction(conditionExpression, out conditionFunction);
                    }
                }
                else
                {
                    conditionFunction = null;
                    return false;
                }
            }
            else if (statement is JassReturnStatementSyntax returnStatement)
            {
                var returnExpression = DeparenthesizeExpression(returnStatement.Value);

                return TryDecompileTriggerConditionFunction(returnExpression, out conditionFunction);
            }
            else
            {
                throw new NotImplementedException();
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

                if (TryDecompileOperatorCompareOperand(binaryExpression.Left, out var leftFunctionParameter, out var operatorCompareTypeLeft, out var operatorTypeLeft) &&
                    TryDecompileOperatorCompareOperand(binaryExpression.Right, out var rightFunctionParameter, out var operatorCompareTypeRight, out var operatorTypeRight))
                {
                    if (string.IsNullOrEmpty(operatorCompareTypeLeft) && string.IsNullOrEmpty(operatorCompareTypeRight))
                    {
                        return false;
                    }
                    else if (string.IsNullOrEmpty(operatorCompareTypeLeft))
                    {
                        operatorCompareTypeLeft = operatorCompareTypeRight;
                    }
                    else if (!string.IsNullOrEmpty(operatorCompareTypeRight) && !string.Equals(operatorCompareTypeLeft, operatorCompareTypeRight, StringComparison.Ordinal))
                    {
                        return false;
                    }

                    if (string.IsNullOrEmpty(operatorTypeLeft) && string.IsNullOrEmpty(operatorTypeRight))
                    {
                        return false;
                    }
                    else if (string.IsNullOrEmpty(operatorTypeLeft))
                    {
                        operatorTypeLeft = operatorTypeRight;
                    }
                    else if (!string.IsNullOrEmpty(operatorTypeRight) && !string.Equals(operatorTypeLeft, operatorTypeRight, StringComparison.Ordinal))
                    {
                        return false;
                    }

                    if (!Context.TriggerData.TryGetTriggerParamPreset(operatorTypeLeft, binaryExpression.Operator.GetSymbol(), out var value))
                    {
                        return false;
                    }

                    var operatorFunctionParameter = new TriggerFunctionParameter
                    {
                        Type = TriggerFunctionParameterType.Preset,
                        Value = value,
                    };

                    function.Name = operatorCompareTypeLeft;
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

        private bool TryDecompileOperatorCompareOperand(
            IExpressionSyntax expression,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter,
            [NotNullWhen(true)] out string? operatorCompareType,
            [NotNullWhen(true)] out string? operatorType)
        {
            var expressionString = expression.ToString();
            if (!string.Equals(expressionString, "0", StringComparison.Ordinal) &&
                Context.TriggerData.TryGetTriggerParamPreset(expressionString, out var presetValue, out var presetType) &&
                Context.TriggerData.TryGetOperatorCompareType(presetType, out operatorCompareType, out operatorType))
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.Preset,
                    Value = presetValue,
                };

                return true;
            }
            else if (expression is JassInvocationExpressionSyntax invocationExpression &&
                     Context.TriggerData.TryGetReturnType(invocationExpression.IdentifierName.Name, out var returnType) &&
                     Context.TriggerData.TryGetOperatorCompareType(returnType, out operatorCompareType, out operatorType))
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
                    operatorCompareType = null;
                    operatorType = null;
                    return false;
                }
            }
            else if (TryDecompileTriggerFunctionParameterStringForUnknownType(expression, out functionParameter, out var literalType))
            {
                if (!string.IsNullOrEmpty(literalType))
                {
                    if (Context.TriggerData.TryGetOperatorCompareType(literalType, out operatorCompareType, out operatorType))
                    {
                        return true;
                    }
                    else
                    {
                        functionParameter = null;
                        operatorCompareType = null;
                        operatorType = null;
                        return false;
                    }
                }
                else
                {
                    operatorCompareType = string.Empty;
                    operatorType = string.Empty;
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