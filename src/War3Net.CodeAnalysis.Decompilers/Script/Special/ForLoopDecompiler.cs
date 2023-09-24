// ------------------------------------------------------------------------------
// <copyright file="ForLoopDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileForLoopActionFunction(
            JassSetStatementSyntax setStatement,
            JassStatementSyntax? statement2,
            JassStatementSyntax? statement3,
            ref List<TriggerFunction> functions)
        {
            if (statement2 is JassSetStatementSyntax setIndexEndStatement &&
                statement3 is JassLoopStatementSyntax loopStatement &&
                loopStatement.Statements.Length >= 2 &&
                loopStatement.Statements[0] is JassExitStatementSyntax exitStatement &&
                exitStatement.Condition.Deparenthesize() is JassBinaryExpressionSyntax exitExpression &&
                exitExpression.OperatorToken.SyntaxKind == JassSyntaxKind.GreaterThanToken &&
                exitExpression.Left.TryGetIdentifierNameValue(out var exitLeftVariableName) &&
                exitExpression.Right.TryGetIdentifierNameValue(out var exitRightVariableName) &&
                loopStatement.Statements[^1] is JassSetStatementSyntax incrementStatement &&
                incrementStatement.Value.Expression is JassBinaryExpressionSyntax incrementExpression &&
                incrementExpression.OperatorToken.SyntaxKind == JassSyntaxKind.PlusToken &&
                incrementExpression.Left.TryGetIdentifierNameValue(out var incrementVariableName) &&
                incrementExpression.Right.TryGetIntegerExpressionValue(out var incrementValue) &&
                incrementValue == 1 &&
                string.Equals(setStatement.IdentifierName.Token.Text, exitLeftVariableName, StringComparison.Ordinal) &&
                string.Equals(setIndexEndStatement.IdentifierName.Token.Text, exitRightVariableName, StringComparison.Ordinal) &&
                string.Equals(setStatement.IdentifierName.Token.Text, incrementStatement.IdentifierName.Token.Text, StringComparison.Ordinal) &&
                string.Equals(setStatement.IdentifierName.Token.Text, incrementVariableName, StringComparison.Ordinal))
            {
                var loopFunction = new TriggerFunction
                {
                    Type = TriggerFunctionType.Action,
                    IsEnabled = true,
                };

                var loopBody = ImmutableArray.CreateRange(loopStatement.Statements, 1, loopStatement.Statements.Length - 2, statement => statement);
                if (TryDecompileTriggerFunctionParameter(setStatement.Value.Expression, JassKeyword.Integer, out var indexFunctionParameter) &&
                    TryDecompileTriggerFunctionParameter(setIndexEndStatement.Value.Expression, JassKeyword.Integer, out var indexEndFunctionParameter) &&
                    TryDecompileActionStatementList(loopBody, out var loopActionFunctions))
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
                    return false;
                }

                if (string.Equals(setStatement.IdentifierName.Token.Text, "bj_forLoopAIndex", StringComparison.Ordinal) &&
                    string.Equals(setIndexEndStatement.IdentifierName.Token.Text, "bj_forLoopAIndexEnd", StringComparison.Ordinal))
                {
                    loopFunction.Name = "ForLoopAMultiple";
                }
                else if (string.Equals(setStatement.IdentifierName.Token.Text, "bj_forLoopBIndex", StringComparison.Ordinal) &&
                         string.Equals(setIndexEndStatement.IdentifierName.Token.Text, "bj_forLoopBIndexEnd", StringComparison.Ordinal))
                {
                    loopFunction.Name = "ForLoopBMultiple";
                }
                else
                {
                    return false;
                }

                functions.Add(loopFunction);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool TryDecompileForLoopVarActionFunction(
            JassSetStatementSyntax setStatement,
            JassStatementSyntax? statement2,
            ref List<TriggerFunction> functions)
        {
            if (statement2 is JassLoopStatementSyntax loopStatement &&
                loopStatement.Statements.Length >= 2 &&
                loopStatement.Statements[0] is JassExitStatementSyntax exitStatement &&
                exitStatement.Condition.Deparenthesize() is JassBinaryExpressionSyntax exitExpression &&
                exitExpression.OperatorToken.SyntaxKind == JassSyntaxKind.GreaterThanToken &&
                loopStatement.Statements[^1] is JassSetStatementSyntax incrementStatement &&
                incrementStatement.Value.Expression is JassBinaryExpressionSyntax incrementExpression &&
                incrementExpression.OperatorToken.SyntaxKind == JassSyntaxKind.PlusToken &&
                incrementExpression.Right.TryGetIntegerExpressionValue(out var incrementValue) &&
                incrementValue == 1 &&
                TryDecompileTriggerFunctionParameterVariable(setStatement, out var variableFunctionParameter, out var variableType) &&
                string.Equals(variableType, JassKeyword.Integer, StringComparison.Ordinal) &&
                TryDecompileTriggerFunctionParameter(setStatement.Value.Expression, JassKeyword.Integer, out var indexFunctionParameter) &&
                TryDecompileTriggerFunctionParameter(exitExpression.Right, JassKeyword.Integer, out var indexEndFunctionParameter))
            {
                if (setStatement.ElementAccessClause is null)
                {
                    if (incrementStatement.ElementAccessClause is null &&
                        exitExpression.Left.TryGetIdentifierNameValue(out var exitLeftVariableName) &&
                        incrementExpression.Left.TryGetIdentifierNameValue(out var incrementVariableName) &&
                        string.Equals(setStatement.IdentifierName.Token.Text, incrementStatement.IdentifierName.Token.Text, StringComparison.Ordinal) &&
                        string.Equals(setStatement.IdentifierName.Token.Text, exitLeftVariableName, StringComparison.Ordinal) &&
                        string.Equals(setStatement.IdentifierName.Token.Text, incrementVariableName, StringComparison.Ordinal))
                    {
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    var elementAccessExpressionString = setStatement.ElementAccessClause.Expression.ToString();
                    if (incrementStatement.ElementAccessClause is not null &&
                        exitExpression.Left is JassElementAccessExpressionSyntax exitLeftElementAccessExpression &&
                        incrementExpression.Left is JassElementAccessExpressionSyntax incrementElementAccessExpression &&
                        string.Equals(setStatement.IdentifierName.Token.Text, incrementStatement.IdentifierName.Token.Text, StringComparison.Ordinal) &&
                        string.Equals(setStatement.IdentifierName.Token.Text, exitLeftElementAccessExpression.IdentifierName.Token.Text, StringComparison.Ordinal) &&
                        string.Equals(setStatement.IdentifierName.Token.Text, incrementElementAccessExpression.IdentifierName.Token.Text, StringComparison.Ordinal) &&
                        string.Equals(elementAccessExpressionString, incrementStatement.ElementAccessClause.Expression.ToString(), StringComparison.Ordinal) &&
                        string.Equals(elementAccessExpressionString, exitLeftElementAccessExpression.ElementAccessClause.Expression.ToString(), StringComparison.Ordinal) &&
                        string.Equals(elementAccessExpressionString, incrementElementAccessExpression.ElementAccessClause.Expression.ToString(), StringComparison.Ordinal))
                    {
                    }
                    else
                    {
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

                var loopBody = ImmutableArray.CreateRange(loopStatement.Statements, 1, loopStatement.Statements.Length - 2, statement => statement);
                if (TryDecompileActionStatementList(loopBody, out var loopActionFunctions))
                {
                    foreach (var loopActionFunction in loopActionFunctions)
                    {
                        loopActionFunction.Branch = 0;
                        loopFunction.ChildFunctions.Add(loopActionFunction);
                    }
                }
                else
                {
                    return false;
                }

                functions.Add(loopFunction);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}