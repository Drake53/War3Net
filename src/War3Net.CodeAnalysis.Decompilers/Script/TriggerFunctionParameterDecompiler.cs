// ------------------------------------------------------------------------------
// <copyright file="TriggerFunctionParameterDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileTriggerFunctionParameter(
            JassExpressionSyntax expression,
            string expectedType,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            return expression switch
            {
                JassLiteralExpressionSyntax literalExpression => literalExpression.Token.SyntaxKind switch
                {
                    JassSyntaxKind.CharacterLiteralToken => TryDecompileCharacterLiteralExpression(literalExpression, expectedType, out functionParameter),
                    JassSyntaxKind.FourCCLiteralToken => TryDecompileFourCCLiteralExpression(literalExpression, expectedType, out functionParameter),
                    JassSyntaxKind.HexadecimalLiteralToken => TryDecompileHexadecimalLiteralExpression(literalExpression, expectedType, out functionParameter),
                    JassSyntaxKind.RealLiteralToken => TryDecompileRealLiteralExpression(literalExpression, expectedType, out functionParameter),
                    JassSyntaxKind.OctalLiteralToken => TryDecompileOctalLiteralExpression(literalExpression, expectedType, out functionParameter),
                    JassSyntaxKind.DecimalLiteralToken => TryDecompileDecimalLiteralExpression(literalExpression, expectedType, out functionParameter),
                    JassSyntaxKind.TrueKeyword or JassSyntaxKind.FalseKeyword => TryDecompileBooleanLiteralExpression(literalExpression, expectedType, out functionParameter),
                    JassSyntaxKind.StringLiteralToken => TryDecompileStringLiteralExpression(literalExpression, expectedType, out functionParameter),
                    JassSyntaxKind.NullKeyword => TryDecompileNullLiteralExpression(literalExpression, expectedType, out functionParameter),

                    _ => throw new NotSupportedException(),
                },
                JassFunctionReferenceExpressionSyntax functionReferenceExpression => TryDecompileFunctionReferenceExpression(functionReferenceExpression, expectedType, out functionParameter),
                JassInvocationExpressionSyntax invocationExpression => TryDecompileInvocationExpression(invocationExpression, expectedType, out functionParameter),
                JassElementAccessExpressionSyntax elementAccessExpression => TryDecompileElementAccessExpression(elementAccessExpression, expectedType, out functionParameter),
                JassIdentifierNameSyntax identifierName => TryDecompileVariableReferenceExpression(identifierName, expectedType, out functionParameter),
                JassParenthesizedExpressionSyntax parenthesizedExpression => TryDecompileParenthesizedExpression(parenthesizedExpression, expectedType, out functionParameter),
                JassUnaryExpressionSyntax unaryExpression => TryDecompileUnaryExpression(unaryExpression, expectedType, out functionParameter),
                JassBinaryExpressionSyntax binaryExpression => TryDecompileBinaryExpression(binaryExpression, expectedType, out functionParameter),

                _ => throw new NotSupportedException(),
            };
        }

        private bool TryDecompileTriggerFunctionParameter(
            JassExpressionSyntax expression,
            [NotNullWhen(true)] out List<DecompileOption>? decompileOptions)
        {
            return expression switch
            {
                JassLiteralExpressionSyntax literalExpression => literalExpression.Token.SyntaxKind switch
                {
                    JassSyntaxKind.CharacterLiteralToken => TryDecompileCharacterLiteralExpression(literalExpression, out decompileOptions),
                    JassSyntaxKind.FourCCLiteralToken => TryDecompileFourCCLiteralExpression(literalExpression, out decompileOptions),
                    JassSyntaxKind.HexadecimalLiteralToken => TryDecompileHexadecimalLiteralExpression(literalExpression, out decompileOptions),
                    JassSyntaxKind.RealLiteralToken => TryDecompileRealLiteralExpression(literalExpression, out decompileOptions),
                    JassSyntaxKind.OctalLiteralToken => TryDecompileOctalLiteralExpression(literalExpression, out decompileOptions),
                    JassSyntaxKind.DecimalLiteralToken => TryDecompileDecimalLiteralExpression(literalExpression, out decompileOptions),
                    JassSyntaxKind.TrueKeyword or JassSyntaxKind.FalseKeyword => TryDecompileBooleanLiteralExpression(literalExpression, out decompileOptions),
                    JassSyntaxKind.StringLiteralToken => TryDecompileStringLiteralExpression(literalExpression, out decompileOptions),
                    JassSyntaxKind.NullKeyword => TryDecompileNullLiteralExpression(literalExpression, out decompileOptions),

                    _ => throw new NotSupportedException(),
                },
                JassFunctionReferenceExpressionSyntax functionReferenceExpression => TryDecompileFunctionReferenceExpression(functionReferenceExpression, out decompileOptions),
                JassInvocationExpressionSyntax invocationExpression => TryDecompileInvocationExpression(invocationExpression, out decompileOptions),
                JassElementAccessExpressionSyntax elementAccessExpression => TryDecompileElementAccessExpression(elementAccessExpression, out decompileOptions),
                JassIdentifierNameSyntax identifierName => TryDecompileVariableReferenceExpression(identifierName, out decompileOptions),
                JassParenthesizedExpressionSyntax parenthesizedExpression => TryDecompileParenthesizedExpression(parenthesizedExpression, out decompileOptions),
                JassUnaryExpressionSyntax unaryExpression => TryDecompileUnaryExpression(unaryExpression, out decompileOptions),
                JassBinaryExpressionSyntax binaryExpression => TryDecompileBinaryExpression(binaryExpression, out decompileOptions),

                _ => throw new NotSupportedException(),
            };
        }

        private bool TryDecompileTriggerFunctionParameter(JassSyntaxKind binaryOperatorSyntaxKind, string type, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (Context.TriggerData.TriggerParams.TryGetValue(type, out var triggerParamsForType) &&
                triggerParamsForType.TryGetValue($"\"{JassSyntaxFacts.GetText(binaryOperatorSyntaxKind)}\"", out var triggerParams))
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.Preset,
                    Value = triggerParams.Single().ParameterName,
                };

                return true;
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileTriggerFunctionParameterPreset(
            string scriptText,
            string? expectedType,
            [NotNullWhen(true)] out string? type,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (string.IsNullOrEmpty(expectedType))
            {
                // todo

                type = null;
                functionParameter = null;
                return false;
            }
            else
            {
                if (Context.TriggerData.TriggerParams.TryGetValue(expectedType, out var triggerParamsForType) &&
                    triggerParamsForType.TryGetValue(scriptText, out var triggerParams) &&
                    triggerParams.Length == 1)
                {
                    var triggerParam = triggerParams[0];

                    type = triggerParam.VariableType;
                    functionParameter = new TriggerFunctionParameter
                    {
                        Type = TriggerFunctionParameterType.Preset,
                        Value = triggerParam.ParameterName,
                    };

                    return true;
                }
            }

            type = null;
            functionParameter = null;
            return false;
        }

        private bool TryDecompileTriggerFunctionParameterVariable(JassSetStatementSyntax setStatement, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter, [NotNullWhen(true)] out string? type)
        {
            if (setStatement.IdentifierName.Token.Text.StartsWith("udg_", StringComparison.Ordinal) &&
                Context.VariableDeclarations.TryGetValue(setStatement.IdentifierName.Token.Text, out var variableDeclaration))
            {
                functionParameter = DecompileVariableTriggerFunctionParameter(setStatement.IdentifierName.Token.Text);
                type = variableDeclaration.Type;

                if (setStatement.ElementAccessClause is null)
                {
                    return true;
                }
                else if (TryDecompileTriggerFunctionParameter(setStatement.ElementAccessClause.Expression, JassKeyword.Integer, out var arrayIndexer))
                {
                    functionParameter.ArrayIndexer = arrayIndexer;

                    return true;
                }
            }

            functionParameter = null;
            type = null;
            return false;
        }

        private TriggerFunctionParameter DecompileVariableTriggerFunctionParameter(string variableName, TriggerFunctionParameter? arrayIndexer = null)
        {
            return new TriggerFunctionParameter
            {
                Type = TriggerFunctionParameterType.Variable,
                Value = variableName.StartsWith("udg_", StringComparison.Ordinal) ? variableName["udg_".Length..] : variableName,
                ArrayIndexer = arrayIndexer,
            };
        }
    }
}