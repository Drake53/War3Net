// ------------------------------------------------------------------------------
// <copyright file="ForLoopDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Decompilers.Extensions;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileForLoopActionFunction(JassSetStatementSyntax setStatement, IStatementSyntax statement2, IStatementSyntax statement3, [NotNullWhen(true)] out TriggerFunction? actionFunction)
        {
            if (statement2 is JassSetStatementSyntax setIndexEndStatement &&
                statement3 is JassLoopStatementSyntax loopStatement &&
                loopStatement.Body.Statements.Length >= 2 &&
                loopStatement.Body.Statements[0] is JassExitStatementSyntax exitStatement &&
                exitStatement.Condition.Deparenthesize() is JassBinaryExpressionSyntax exitExpression &&
                exitExpression.Operator == BinaryOperatorType.GreaterThan &&
                exitExpression.Left is JassVariableReferenceExpressionSyntax exitLeftVariableReferenceExpression &&
                exitExpression.Right is JassVariableReferenceExpressionSyntax exitRightVariableReferenceExpression &&
                loopStatement.Body.Statements[^1] is JassSetStatementSyntax incrementStatement &&
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

                var loopBody = new JassStatementListSyntax(ImmutableArray.CreateRange(loopStatement.Body.Statements, 1, loopStatement.Body.Statements.Length - 2, statement => statement));
                if (TryDecompileTriggerFunctionParameter(setStatement.Value.Expression, JassKeyword.Integer, out var indexFunctionParameter) &&
                    TryDecompileTriggerFunctionParameter(setIndexEndStatement.Value.Expression, JassKeyword.Integer, out var indexEndFunctionParameter) &&
                    TryDecompileTriggerActionFunctions(loopBody, out var loopActionFunctions))
                {
                    loopFunction.Parameters.Add(indexFunctionParameter);
                    loopFunction.Parameters.Add(indexEndFunctionParameter);

                    foreach (var loopActionFunction in loopActionFunctions)
                    {
                        loopActionFunction.Branch = 0;
                        loopFunction.ChildFunctions.Add(loopActionFunction);
                    }
                }
                else
                {
                    actionFunction = null;
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
                    actionFunction = null;
                    return false;
                }

                actionFunction = loopFunction;
                return true;
            }
            else
            {
                actionFunction = null;
                return false;
            }
        }

        private bool TryDecompileForLoopVarActionFunction(JassSetStatementSyntax setStatement, IStatementSyntax statement2, [NotNullWhen(true)] out TriggerFunction? actionFunction)
        {
            if (statement2 is JassLoopStatementSyntax loopStatement &&
                loopStatement.Body.Statements.Length >= 2 &&
                loopStatement.Body.Statements[0] is JassExitStatementSyntax exitStatement &&
                exitStatement.Condition.Deparenthesize() is JassBinaryExpressionSyntax exitExpression &&
                exitExpression.Operator == BinaryOperatorType.GreaterThan &&
                loopStatement.Body.Statements[^1] is JassSetStatementSyntax incrementStatement &&
                incrementStatement.Value.Expression is JassBinaryExpressionSyntax incrementExpression &&
                incrementExpression.Operator == BinaryOperatorType.Add &&
                incrementExpression.Right is JassDecimalLiteralExpressionSyntax incrementLiteralExpression &&
                incrementLiteralExpression.Value == 1 &&
                TryDecompileTriggerFunctionParameterVariable(setStatement, out var variableFunctionParameter, out var variableType) &&
                string.Equals(variableType, JassKeyword.Integer, StringComparison.Ordinal) &&
                TryDecompileTriggerFunctionParameter(setStatement.Value.Expression, JassKeyword.Integer, out var indexFunctionParameter) &&
                TryDecompileTriggerFunctionParameter(exitExpression.Right, JassKeyword.Integer, out var indexEndFunctionParameter))
            {
                if (setStatement.Indexer is null)
                {
                    if (incrementStatement.Indexer is null &&
                        exitExpression.Left is JassVariableReferenceExpressionSyntax exitLeftVariableReferenceExpression &&
                        incrementExpression.Left is JassVariableReferenceExpressionSyntax incrementVariableReferenceExpression &&
                        string.Equals(setStatement.IdentifierName.Name, incrementStatement.IdentifierName.Name, StringComparison.Ordinal) &&
                        string.Equals(setStatement.IdentifierName.Name, exitLeftVariableReferenceExpression.IdentifierName.Name, StringComparison.Ordinal) &&
                        string.Equals(setStatement.IdentifierName.Name, incrementVariableReferenceExpression.IdentifierName.Name, StringComparison.Ordinal))
                    {
                    }
                    else
                    {
                        actionFunction = null;
                        return false;
                    }
                }
                else
                {
                    var indexer = setStatement.Indexer.ToString();
                    if (incrementStatement.Indexer is not null &&
                        exitExpression.Left is JassArrayReferenceExpressionSyntax exitLeftArrayReferenceExpression &&
                        incrementExpression.Left is JassArrayReferenceExpressionSyntax incrementArrayReferenceExpression &&
                        string.Equals(setStatement.IdentifierName.Name, incrementStatement.IdentifierName.Name, StringComparison.Ordinal) &&
                        string.Equals(setStatement.IdentifierName.Name, exitLeftArrayReferenceExpression.IdentifierName.Name, StringComparison.Ordinal) &&
                        string.Equals(setStatement.IdentifierName.Name, incrementArrayReferenceExpression.IdentifierName.Name, StringComparison.Ordinal) &&
                        string.Equals(indexer, incrementStatement.Indexer.ToString(), StringComparison.Ordinal) &&
                        string.Equals(indexer, exitLeftArrayReferenceExpression.Indexer.ToString(), StringComparison.Ordinal) &&
                        string.Equals(indexer, incrementArrayReferenceExpression.Indexer.ToString(), StringComparison.Ordinal))
                    {
                    }
                    else
                    {
                        actionFunction = null;
                        return false;
                    }
                }

                var loopFunction = new TriggerFunction
                {
                    Type = TriggerFunctionType.Action,
                    IsEnabled = true,
                    Name = "ForLoopVarMultiple",
                };

                loopFunction.Parameters.Add(variableFunctionParameter);
                loopFunction.Parameters.Add(indexFunctionParameter);
                loopFunction.Parameters.Add(indexEndFunctionParameter);

                var loopBody = new JassStatementListSyntax(ImmutableArray.CreateRange(loopStatement.Body.Statements, 1, loopStatement.Body.Statements.Length - 2, statement => statement));
                if (TryDecompileTriggerActionFunctions(loopBody, out var loopActionFunctions))
                {
                    foreach (var loopActionFunction in loopActionFunctions)
                    {
                        loopActionFunction.Branch = 0;
                        loopFunction.ChildFunctions.Add(loopActionFunction);
                    }
                }
                else
                {
                    actionFunction = null;
                    return false;
                }

                actionFunction = loopFunction;
                return true;
            }
            else
            {
                actionFunction = null;
                return false;
            }
        }
    }
}