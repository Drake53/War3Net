using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileTriggerActionFunctions(JassStatementListSyntax statementList, [NotNullWhen(true)] out List<TriggerFunction>? actionFunctions)
        {
            var result = new List<TriggerFunction>();

            for (var i = 0; i < statementList.Statements.Length; i++)
            {
                var statement = statementList.Statements[i];
                if (statement is JassCallStatementSyntax callStatement)
                {
                    if (Context.TriggerData.TryGetParametersByScriptName(callStatement.IdentifierName.Name, callStatement.Arguments.Arguments.Length, out var parameters, out var functionName))
                    {
                        var function = new TriggerFunction
                        {
                            Type = TriggerFunctionType.Action,
                            IsEnabled = true,
                            Name = functionName,
                        };

                        for (var j = 0; j < callStatement.Arguments.Arguments.Length; j++)
                        {
                            if (TryDecompileTriggerFunctionParameter(callStatement.Arguments.Arguments[j], parameters.Value[j], out var functionParameter))
                            {
                                function.Parameters.Add(functionParameter);
                            }
                            else
                            {
                                actionFunctions = null;
                                return false;
                            }
                        }

                        result.Add(function);
                        continue;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else if (statement is JassSetStatementSyntax setStatement)
                {
                    if (i + 2 < statementList.Statements.Length &&
                        statementList.Statements[i + 1] is JassSetStatementSyntax setIndexEndStatement &&
                        statementList.Statements[i + 2] is JassLoopStatementSyntax loopStatement &&
                        loopStatement.Body.Statements.Length >= 2 &&
                        loopStatement.Body.Statements.First() is JassExitStatementSyntax exitStatement &&
                        DeparenthesizeExpression(exitStatement.Condition) is JassBinaryExpressionSyntax exitExpression &&
                        exitExpression.Operator == BinaryOperatorType.GreaterThan &&
                        exitExpression.Left is JassVariableReferenceExpressionSyntax exitLeftVariableReferenceExpression &&
                        exitExpression.Right is JassVariableReferenceExpressionSyntax exitRightVariableReferenceExpression &&
                        loopStatement.Body.Statements.Last() is JassSetStatementSyntax incrementStatement &&
                        incrementStatement.Value.Expression is JassBinaryExpressionSyntax incrementExpression &&
                        incrementExpression.Operator == BinaryOperatorType.Add &&
                        incrementExpression.Left is JassVariableReferenceExpressionSyntax incrementVariableReferenceExpression &&
                        incrementExpression.Right is JassDecimalLiteralExpressionSyntax incrementLiteralExpression &&
                        incrementLiteralExpression.Value == 1 &&
                        string.Equals(setStatement.IdentifierName.Name, exitLeftVariableReferenceExpression.IdentifierName.Name, StringComparison.Ordinal) &&
                        string.Equals(setIndexEndStatement.IdentifierName.Name, exitRightVariableReferenceExpression.IdentifierName.Name, StringComparison.Ordinal) &&
                        string.Equals(setStatement.IdentifierName.Name, incrementStatement.IdentifierName.Name, StringComparison.Ordinal) &&
                        string.Equals(setStatement.IdentifierName.Name, incrementVariableReferenceExpression.IdentifierName.Name, StringComparison.Ordinal))
                    {
                        var loopFunction = new TriggerFunction
                        {
                            Type = TriggerFunctionType.Action,
                            IsEnabled = true,
                        };

                        if (TryDecompileTriggerFunctionParameter(setStatement.Value.Expression, "integer", out var indexFunctionParameter) &&
                            TryDecompileTriggerFunctionParameter(setIndexEndStatement.Value.Expression, "integer", out var indexEndFunctionParameter) &&
                            TryDecompileTriggerActionFunctions(loopStatement.Body, out var loopActionFunctions))
                        {
                            loopFunction.Parameters.Add(indexFunctionParameter);
                            loopFunction.Parameters.Add(indexEndFunctionParameter);

                            foreach (var loopActionFunction in loopActionFunctions.Skip(1).SkipLast(1))
                            {
                                loopActionFunction.Branch = 0;
                                loopFunction.ChildFunctions.Add(loopActionFunction);
                            }
                        }
                        else
                        {
                            actionFunctions = null;
                            return false;
                        }

                        if (string.Equals(setStatement.IdentifierName.Name, "bj_forLoopAIndex", StringComparison.Ordinal) &&
                            string.Equals(setIndexEndStatement.IdentifierName.Name, "bj_forLoopAIndexEnd", StringComparison.Ordinal))
                        {
                            loopFunction.Name = "ForLoopAMultiple";
                        }
                        else if (string.Equals(setStatement.IdentifierName.Name, "bj_forLoopBIndex", StringComparison.Ordinal) &&
                                 string.Equals(setIndexEndStatement.IdentifierName.Name, "bj_forLoopBIndexEnd", StringComparison.Ordinal))
                        {
                            loopFunction.Name = "ForLoopBMultiple";
                        }
                        else
                        {
                            actionFunctions = null;
                            return false;
                        }

                        result.Add(loopFunction);

                        i += 2;
                        continue;
                    }

                    var function = new TriggerFunction
                    {
                        Type = TriggerFunctionType.Action,
                        IsEnabled = true,
                        Name = "CustomScriptCode",
                        Parameters = new()
                        {
                            new TriggerFunctionParameter
                            {
                                Type = TriggerFunctionParameterType.String,
                                Value = setStatement.ToString(),
                            },
                        },
                    };

                    result.Add(function);
                    continue;
                }
                else if (statement is JassLocalVariableDeclarationStatementSyntax localVariableDeclarationStatement)
                {
                    var function = new TriggerFunction
                    {
                        Type = TriggerFunctionType.Action,
                        IsEnabled = true,
                        Name = "CustomScriptCode",
                        Parameters = new()
                        {
                            new TriggerFunctionParameter
                            {
                                Type = TriggerFunctionParameterType.String,
                                Value = localVariableDeclarationStatement.ToString(),
                            },
                        },
                    };

                    result.Add(function);
                    continue;
                }
                else if (statement is JassIfStatementSyntax ifStatement)
                {
                    if (ifStatement.ElseIfClauses.IsEmpty)
                    {
                        var function = new TriggerFunction
                        {
                            Type = TriggerFunctionType.Action,
                            IsEnabled = true,
                            Name = "IfThenElseMultiple",
                        };

                        if (DeparenthesizeExpression(ifStatement.Condition) is JassInvocationExpressionSyntax conditionInvocationExpression)
                        {
                            var conditionsFunction = (JassFunctionDeclarationSyntax)Context.CompilationUnit.Declarations.Single(declaration =>
                                declaration is JassFunctionDeclarationSyntax functionDeclaration &&
                                string.Equals(functionDeclaration.FunctionDeclarator.IdentifierName.Name, conditionInvocationExpression.IdentifierName.Name, StringComparison.Ordinal));

                            foreach (var conditionStatement in conditionsFunction.Body.Statements.SkipLast(1))
                            {
                                if (TryDecompileTriggerConditionFunction(conditionStatement, true, out var conditionFunction))
                                {
                                    conditionFunction.Branch = 0;
                                    function.ChildFunctions.Add(conditionFunction);
                                }
                                else
                                {
                                    actionFunctions = null;
                                    return false;
                                }
                            }

                            // Last statement must be "return true"
                            if (conditionsFunction.Body.Statements.Last() is not JassReturnStatementSyntax finalReturnStatement ||
                                finalReturnStatement.Value is not JassBooleanLiteralExpressionSyntax returnBooleanLiteralExpression ||
                                !returnBooleanLiteralExpression.Value)
                            {
                                actionFunctions = null;
                                return false;
                            }
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }

                        if (TryDecompileTriggerActionFunctions(ifStatement.Body, out var thenActions) &&
                            TryDecompileTriggerActionFunctions(ifStatement.ElseClause.Body, out var elseActions))
                        {
                            foreach (var action in thenActions)
                            {
                                action.Branch = 1;
                            }

                            foreach (var action in elseActions)
                            {
                                action.Branch = 2;
                            }

                            function.ChildFunctions.AddRange(thenActions);
                            function.ChildFunctions.AddRange(elseActions);
                        }
                        else
                        {
                            actionFunctions = null;
                            return false;
                        }

                        result.Add(function);
                        continue;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else if (statement is JassLoopStatementSyntax loopStatement)
                {
                    var function = new TriggerFunction
                    {
                        Type = TriggerFunctionType.Action,
                        IsEnabled = true,
                        Name = "CustomScriptCode",
                        Parameters = new()
                        {
                            new TriggerFunctionParameter
                            {
                                Type = TriggerFunctionParameterType.String,
                                Value = "loop",
                            },
                        },
                    };
                    result.Add(function);

                    if (TryDecompileTriggerActionFunctions(loopStatement.Body, out var loopActions))
                    {
                        result.AddRange(loopActions);
                    }
                    else
                    {
                        actionFunctions = null;
                        return false;
                    }

                    function = new TriggerFunction
                    {
                        Type = TriggerFunctionType.Action,
                        IsEnabled = true,
                        Name = "CustomScriptCode",
                        Parameters = new()
                        {
                            new TriggerFunctionParameter
                            {
                                Type = TriggerFunctionParameterType.String,
                                Value = "endloop",
                            },
                        },
                    };
                    result.Add(function);

                    continue;
                }
                else if (statement is JassExitStatementSyntax exitStatement)
                {
                    var function = new TriggerFunction
                    {
                        Type = TriggerFunctionType.Action,
                        IsEnabled = true,
                        Name = "CustomScriptCode",
                        Parameters = new()
                        {
                            new TriggerFunctionParameter
                            {
                                Type = TriggerFunctionParameterType.String,
                                Value = exitStatement.ToString(),
                            },
                        },
                    };

                    result.Add(function);
                    continue;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            actionFunctions = result;
            return true;
        }
    }
}