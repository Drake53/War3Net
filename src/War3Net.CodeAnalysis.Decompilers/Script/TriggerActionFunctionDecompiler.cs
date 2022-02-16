// ------------------------------------------------------------------------------
// <copyright file="TriggerActionFunctionDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

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
                    if (Context.TriggerData.TriggerActions.TryGetValue(callStatement.IdentifierName.Name, out var actions))
                    {
                        var action = actions.First(action => action.ArgumentTypes.Length == callStatement.Arguments.Arguments.Length);

                        if (TryDecompileForEachLoopActionFunction(callStatement, action.ArgumentTypes, out var loopActionFunction))
                        {
                            result.Add(loopActionFunction);
                        }
                        else
                        {
                            var function = new TriggerFunction
                            {
                                Type = TriggerFunctionType.Action,
                                IsEnabled = true,
                                Name = action.FunctionName,
                            };

                            for (var j = 0; j < callStatement.Arguments.Arguments.Length; j++)
                            {
                                if (TryDecompileTriggerFunctionParameter(callStatement.Arguments.Arguments[j], action.ArgumentTypes[j], out var functionParameter))
                                {
                                    function.Parameters.Add(functionParameter);
                                }
                                else
                                {
                                    function = DecompileCustomScriptAction(callStatement);
                                    break;
                                }
                            }

                            result.Add(function);
                        }
                    }
                    else
                    {
                        result.Add(DecompileCustomScriptAction(callStatement));
                    }
                }
                else if (statement is JassSetStatementSyntax setStatement)
                {
                    if (i + 2 < statementList.Statements.Length &&
                        TryDecompileForLoopActionFunction(setStatement, statementList.Statements[i + 1], statementList.Statements[i + 2], out var loopFunction))
                    {
                        result.Add(loopFunction);

                        i += 2;
                    }
                    else if (i + 1 < statementList.Statements.Length &&
                             TryDecompileForLoopVarActionFunction(setStatement, statementList.Statements[i + 1], out var loopVarFunction))
                    {
                        result.Add(loopVarFunction);

                        i += 1;
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

                        result.Add(function);
                    }
                    else
                    {
                        result.Add(DecompileCustomScriptAction(setStatement));
                    }
                }
                else if (statement is JassLocalVariableDeclarationStatementSyntax localVariableDeclarationStatement)
                {
                    result.Add(DecompileCustomScriptAction(localVariableDeclarationStatement));
                }
                else if (statement is JassIfStatementSyntax ifStatement)
                {
                    if (TryDecompileIfThenElseActionFunction(ifStatement, out var ifThenElseFunction))
                    {
                        result.Add(ifThenElseFunction);
                    }
                    else
                    {
                        result.Add(DecompileCustomScriptAction(new JassIfCustomScriptAction(ifStatement.Condition)));

                        if (TryDecompileTriggerActionFunctions(ifStatement.Body, out var thenActions))
                        {
                            result.AddRange(thenActions);
                        }
                        else
                        {
                            actionFunctions = null;
                            return false;
                        }

                        foreach (var elseIfClause in ifStatement.ElseIfClauses)
                        {
                            result.Add(DecompileCustomScriptAction(new JassElseIfCustomScriptAction(elseIfClause.Condition)));

                            if (TryDecompileTriggerActionFunctions(elseIfClause.Body, out var elseIfActions))
                            {
                                result.AddRange(elseIfActions);
                            }
                            else
                            {
                                actionFunctions = null;
                                return false;
                            }
                        }

                        if (ifStatement.ElseClause is not null)
                        {
                            result.Add(DecompileCustomScriptAction(JassElseCustomScriptAction.Value));

                            if (TryDecompileTriggerActionFunctions(ifStatement.ElseClause.Body, out var elseActions))
                            {
                                result.AddRange(elseActions);
                            }
                            else
                            {
                                actionFunctions = null;
                                return false;
                            }
                        }

                        result.Add(DecompileCustomScriptAction(JassEndIfCustomScriptAction.Value));
                    }
                }
                else if (statement is JassLoopStatementSyntax loopStatement)
                {
                    if (TryDecompileWaitForConditionActionFunction(loopStatement, out var waitForConditionFunction))
                    {
                        result.Add(waitForConditionFunction);
                    }
                    else
                    {
                        result.Add(DecompileCustomScriptAction(JassLoopCustomScriptAction.Value));

                        if (TryDecompileTriggerActionFunctions(loopStatement.Body, out var loopActions))
                        {
                            result.AddRange(loopActions);
                        }
                        else
                        {
                            actionFunctions = null;
                            return false;
                        }

                        result.Add(DecompileCustomScriptAction(JassEndLoopCustomScriptAction.Value));
                    }
                }
                else if (statement is JassExitStatementSyntax exitStatement)
                {
                    result.Add(DecompileCustomScriptAction(exitStatement));
                }
                else if (statement is JassCommentSyntax comment)
                {
                    if (comment.Comment.Length > 1 &&
                        comment.Comment.StartsWith(' '))
                    {
                        result.Add(new TriggerFunction
                        {
                            Type = TriggerFunctionType.Action,
                            IsEnabled = true,
                            Name = "CommentString",
                            Parameters = new()
                            {
                                new TriggerFunctionParameter
                                {
                                    Type = TriggerFunctionParameterType.String,
                                    Value = comment.Comment[1..],
                                },
                            },
                        });
                    }
                    else
                    {
                        result.Add(DecompileCustomScriptAction((IStatementLineSyntax)comment));
                    }
                }
                else if (statement is JassReturnStatementSyntax returnStatement)
                {
                    if (returnStatement.Value is null)
                    {
                        result.Add(new TriggerFunction
                        {
                            Type = TriggerFunctionType.Action,
                            IsEnabled = true,
                            Name = "ReturnAction",
                        });
                    }
                    else
                    {
                        result.Add(DecompileCustomScriptAction(returnStatement));
                    }
                }
                else
                {
                    throw new NotSupportedException();
                }
            }

            actionFunctions = result;
            return true;
        }
    }
}