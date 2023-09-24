// ------------------------------------------------------------------------------
// <copyright file="VariableReferenceExpressionDecompiler.cs" company="Drake53">
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
        private bool TryDecompileVariableReferenceExpression(
            JassIdentifierNameSyntax identifierName,
            string expectedType,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (Context.TriggerData.TriggerParams.TryGetValue(string.Empty, out var triggerParamsForAllTypes) &&
                triggerParamsForAllTypes.TryGetValue(identifierName.Token.Text, out var triggerParams))
            {
                var triggerParam = triggerParams.SingleOrDefault(param => string.Equals(param.VariableType, expectedType, StringComparison.Ordinal));
                if (triggerParam is not null)
                {
                    functionParameter = new TriggerFunctionParameter
                    {
                        Type = TriggerFunctionParameterType.Preset,
                        Value = triggerParam.ParameterName,
                    };

                    return true;
                }
            }

            return TryDecompileVariableDeclarationReference(
                identifierName.Token.Text,
                null,
                expectedType,
                out functionParameter);
        }

        private bool TryDecompileVariableReferenceExpression(
            JassIdentifierNameSyntax identifierName,
            [NotNullWhen(true)] out List<DecompileOption>? decompileOptions)
        {
            if (Context.TriggerData.TriggerParams.TryGetValue(string.Empty, out var triggerParamsForAllTypes) &&
                triggerParamsForAllTypes.TryGetValue(identifierName.Token.Text, out var triggerParams) &&
                triggerParams.Length == 1)
            {
                var triggerParam = triggerParams[0];

                decompileOptions = new();
                decompileOptions.Add(new DecompileOption
                {
                    Type = triggerParam.VariableType,
                    Parameter = new TriggerFunctionParameter
                    {
                        Type = TriggerFunctionParameterType.Preset,
                        Value = triggerParam.ParameterName,
                    },
                });

                return true;
            }

            return TryDecompileVariableDeclarationReference(
                identifierName.Token.Text,
                null,
                out decompileOptions);
        }

        private bool TryDecompileVariableDeclarationReference(
            string variableName,
            TriggerFunctionParameter? arrayIndexer,
            string? expectedType,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (Context.VariableDeclarations.TryGetValue(variableName, out var variableDeclaration) &&
                (arrayIndexer is not null == variableDeclaration.IsArray))
            {
                if (string.IsNullOrEmpty(expectedType))
                {
                    if (Context.TriggerData.TriggerData.TriggerTypes.TryGetValue(variableDeclaration.Type, out var triggerType) && triggerType.UsableAsGlobalVariable)
                    {
                        functionParameter = new TriggerFunctionParameter
                        {
                            Type = TriggerFunctionParameterType.Variable,
                            Value = variableName.StartsWith("udg_", StringComparison.Ordinal) ? variableName["udg_".Length..] : variableName,
                            ArrayIndexer = arrayIndexer,
                        };

                        return true;
                    }
                }
                else if (Context.TriggerData.TriggerData.TriggerTypes.TryGetValue(expectedType, out var triggerType) && triggerType.UsableAsGlobalVariable)
                {
                    if (string.Equals(variableDeclaration.Type, expectedType, StringComparison.Ordinal))
                    {
                        functionParameter = new TriggerFunctionParameter
                        {
                            Type = TriggerFunctionParameterType.Variable,
                            Value = variableName.StartsWith("udg_", StringComparison.Ordinal) ? variableName["udg_".Length..] : variableName,
                            ArrayIndexer = arrayIndexer,
                        };

                        return true;
                    }
                    else if (Context.TriggerData.TriggerTypes.TryGetValue(variableDeclaration.Type, out var customTypes) && customTypes.ContainsKey(expectedType))
                    {
                        variableDeclaration.Type = expectedType;
                        if (variableDeclaration.VariableDefinition is not null)
                        {
                            variableDeclaration.VariableDefinition.Type = expectedType;
                        }

                        functionParameter = new TriggerFunctionParameter
                        {
                            Type = TriggerFunctionParameterType.Variable,
                            Value = variableName.StartsWith("udg_", StringComparison.Ordinal) ? variableName["udg_".Length..] : variableName,
                            ArrayIndexer = arrayIndexer,
                        };

                        return true;
                    }
                }
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileVariableDeclarationReference(
            string variableName,
            TriggerFunctionParameter? arrayIndexer,
            [NotNullWhen(true)] out List<DecompileOption>? decompileOptions)
        {
            if (Context.VariableDeclarations.TryGetValue(variableName, out var variableDeclaration) &&
                (arrayIndexer is not null == variableDeclaration.IsArray))
            {
                if (Context.TriggerData.TriggerData.TriggerTypes.TryGetValue(variableDeclaration.Type, out var triggerType) && triggerType.UsableAsGlobalVariable)
                {
                    decompileOptions = new();

                    var functionParameter = new TriggerFunctionParameter
                    {
                        Type = TriggerFunctionParameterType.Variable,
                        Value = variableName.StartsWith("udg_", StringComparison.Ordinal) ? variableName["udg_".Length..] : variableName,
                        ArrayIndexer = arrayIndexer,
                    };

                    decompileOptions.Add(new DecompileOption
                    {
                        Type = variableDeclaration.Type,
                        Parameter = functionParameter,
                    });

                    if (Context.TriggerData.TriggerTypes.TryGetValue(variableDeclaration.Type, out var customTypes))
                    {
                        foreach (var customType in customTypes)
                        {
                            if (Context.TriggerData.TriggerData.TriggerTypes.TryGetValue(customType.Key, out var customTriggerType) && customTriggerType.UsableAsGlobalVariable)
                            {
                                decompileOptions.Add(new DecompileOption
                                {
                                    Type = customType.Key,
                                    Parameter = functionParameter,
                                });
                            }
                        }
                    }

                    return true;
                }
            }

            decompileOptions = null;
            return false;
        }
    }
}