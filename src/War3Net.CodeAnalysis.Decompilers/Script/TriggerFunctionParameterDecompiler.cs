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
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileTriggerFunctionParameter(
            IExpressionSyntax expression,
            string expectedType,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            return expression switch
            {
                JassCharacterLiteralExpressionSyntax characterLiteralExpression => TryDecompileCharacterLiteralExpression(characterLiteralExpression, expectedType, out functionParameter),
                JassFourCCLiteralExpressionSyntax fourCCLiteralExpression => TryDecompileFourCCLiteralExpression(fourCCLiteralExpression, expectedType, out functionParameter),
                JassHexadecimalLiteralExpressionSyntax hexadecimalLiteralExpression => TryDecompileHexadecimalLiteralExpression(hexadecimalLiteralExpression, expectedType, out functionParameter),
                JassRealLiteralExpressionSyntax realLiteralExpression => TryDecompileRealLiteralExpression(realLiteralExpression, expectedType, out functionParameter),
                JassOctalLiteralExpressionSyntax octalLiteralExpression => TryDecompileOctalLiteralExpression(octalLiteralExpression, expectedType, out functionParameter),
                JassDecimalLiteralExpressionSyntax decimalLiteralExpression => TryDecompileDecimalLiteralExpression(decimalLiteralExpression, expectedType, out functionParameter),
                JassBooleanLiteralExpressionSyntax booleanLiteralExpression => TryDecompileBooleanLiteralExpression(booleanLiteralExpression, expectedType, out functionParameter),
                JassStringLiteralExpressionSyntax stringLiteralExpression => TryDecompileStringLiteralExpression(stringLiteralExpression, expectedType, out functionParameter),
                JassNullLiteralExpressionSyntax nullLiteralExpression => TryDecompileNullLiteralExpression(nullLiteralExpression, expectedType, out functionParameter),
                JassFunctionReferenceExpressionSyntax functionReferenceExpression => TryDecompileFunctionReferenceExpression(functionReferenceExpression, expectedType, out functionParameter),
                JassInvocationExpressionSyntax invocationExpression => TryDecompileInvocationExpression(invocationExpression, expectedType, out functionParameter),
                JassArrayReferenceExpressionSyntax arrayReferenceExpression => TryDecompileArrayReferenceExpression(arrayReferenceExpression, expectedType, out functionParameter),
                JassVariableReferenceExpressionSyntax variableReferenceExpression => TryDecompileVariableReferenceExpression(variableReferenceExpression, expectedType, out functionParameter),
                JassParenthesizedExpressionSyntax parenthesizedExpression => TryDecompileParenthesizedExpression(parenthesizedExpression, expectedType, out functionParameter),
                JassUnaryExpressionSyntax unaryExpression => TryDecompileUnaryExpression(unaryExpression, expectedType, out functionParameter),
                JassBinaryExpressionSyntax binaryExpression => TryDecompileBinaryExpression(binaryExpression, expectedType, out functionParameter),

                _ => throw new NotSupportedException(),
            };
        }

        private bool TryDecompileTriggerFunctionParameter(
            IExpressionSyntax expression,
            [NotNullWhen(true)] out List<DecompileOption>? decompileOptions)
        {
            return expression switch
            {
                JassCharacterLiteralExpressionSyntax characterLiteralExpression => TryDecompileCharacterLiteralExpression(characterLiteralExpression, out decompileOptions),
                JassFourCCLiteralExpressionSyntax fourCCLiteralExpression => TryDecompileFourCCLiteralExpression(fourCCLiteralExpression, out decompileOptions),
                JassHexadecimalLiteralExpressionSyntax hexadecimalLiteralExpression => TryDecompileHexadecimalLiteralExpression(hexadecimalLiteralExpression, out decompileOptions),
                JassRealLiteralExpressionSyntax realLiteralExpression => TryDecompileRealLiteralExpression(realLiteralExpression, out decompileOptions),
                JassOctalLiteralExpressionSyntax octalLiteralExpression => TryDecompileOctalLiteralExpression(octalLiteralExpression, out decompileOptions),
                JassDecimalLiteralExpressionSyntax decimalLiteralExpression => TryDecompileDecimalLiteralExpression(decimalLiteralExpression, out decompileOptions),
                JassBooleanLiteralExpressionSyntax booleanLiteralExpression => TryDecompileBooleanLiteralExpression(booleanLiteralExpression, out decompileOptions),
                JassStringLiteralExpressionSyntax stringLiteralExpression => TryDecompileStringLiteralExpression(stringLiteralExpression, out decompileOptions),
                JassNullLiteralExpressionSyntax nullLiteralExpression => TryDecompileNullLiteralExpression(nullLiteralExpression, out decompileOptions),
                JassFunctionReferenceExpressionSyntax functionReferenceExpression => TryDecompileFunctionReferenceExpression(functionReferenceExpression, out decompileOptions),
                JassInvocationExpressionSyntax invocationExpression => TryDecompileInvocationExpression(invocationExpression, out decompileOptions),
                JassArrayReferenceExpressionSyntax arrayReferenceExpression => TryDecompileArrayReferenceExpression(arrayReferenceExpression, out decompileOptions),
                JassVariableReferenceExpressionSyntax variableReferenceExpression => TryDecompileVariableReferenceExpression(variableReferenceExpression, out decompileOptions),
                JassParenthesizedExpressionSyntax parenthesizedExpression => TryDecompileParenthesizedExpression(parenthesizedExpression, out decompileOptions),
                JassUnaryExpressionSyntax unaryExpression => TryDecompileUnaryExpression(unaryExpression, out decompileOptions),
                JassBinaryExpressionSyntax binaryExpression => TryDecompileBinaryExpression(binaryExpression, out decompileOptions),

                _ => throw new NotSupportedException(),
            };
        }

        private bool TryDecompileTriggerFunctionParameter(BinaryOperatorType binaryOperatorType, string type, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (Context.TriggerData.TriggerParams.TryGetValue(type, out var triggerParamsForType) &&
                triggerParamsForType.TryGetValue($"\"{binaryOperatorType.GetSymbol()}\"", out var triggerParams))
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
            if (setStatement.IdentifierName.Name.StartsWith("udg_", StringComparison.Ordinal) &&
                Context.VariableDeclarations.TryGetValue(setStatement.IdentifierName.Name, out var variableDeclaration))
            {
                functionParameter = DecompileVariableTriggerFunctionParameter(setStatement.IdentifierName.Name);
                type = variableDeclaration.Type;

                if (setStatement.Indexer is null)
                {
                    return true;
                }
                else if (TryDecompileTriggerFunctionParameter(setStatement.Indexer, JassKeyword.Integer, out var arrayIndexer))
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