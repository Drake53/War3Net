// ------------------------------------------------------------------------------
// <copyright file="TriggerFunctionParameterDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Decompilers.Extensions;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileTriggerFunctionParameter(IExpressionSyntax expression, string type, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
#if DEBUG
        {
            if (TryDecompileTriggerFunctionParameterInternal(expression, type, out functionParameter))
            {
                return true;
            }

            return false;
        }

        private bool TryDecompileTriggerFunctionParameterInternal(IExpressionSyntax expression, string type, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
#endif
        {
            expression = expression.Deparenthesize();

            return TryDecompileTriggerFunctionParameterVariable(expression, type, out functionParameter)
                || TryDecompileTriggerFunctionParameterPreset(expression, type, out functionParameter)
                || TryDecompileTriggerFunctionParameterFunction(expression, type, out functionParameter)
                || TryDecompileTriggerFunctionParameterValue(expression, type, out functionParameter);
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

        private bool TryDecompileTriggerFunctionParameterPreset(IExpressionSyntax expression, string type, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (Context.TriggerData.TriggerParams.TryGetValue(type, out var triggerParamsForType) &&
                triggerParamsForType.TryGetValue(expression.ToString(), out var triggerParams))
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

        private bool TryDecompileTriggerFunctionParameterVariable(IExpressionSyntax expression, string type, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (Context.TriggerData.TriggerData.TriggerTypes.TryGetValue(type, out var triggerType) &&
                triggerType.UsableAsGlobalVariable)
            {
                if (expression is JassVariableReferenceExpressionSyntax variableReferenceExpression)
                {
                    return TryDecompileVariableTriggerFunctionParameter(type, variableReferenceExpression.IdentifierName.Name, null, out functionParameter);
                }
                else if (expression is JassArrayReferenceExpressionSyntax arrayReferenceExpression)
                {
                    if (TryDecompileTriggerFunctionParameter(arrayReferenceExpression.Indexer, JassKeyword.Integer, out var arrayIndexer))
                    {
                        return TryDecompileVariableTriggerFunctionParameter(type, arrayReferenceExpression.IdentifierName.Name, arrayIndexer, out functionParameter);
                    }
                }
            }

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

        private bool TryDecompileTriggerFunctionParameterFunction(IExpressionSyntax expression, string type, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (expression is JassInvocationExpressionSyntax invocationExpression)
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
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileTriggerFunctionParameterValue(IExpressionSyntax expression, string type, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            switch (type)
            {
                case "abilcode":
                    return TryDecompileTriggerFunctionAbilCodeParameter(expression, out functionParameter);

                case "attachpoint":
                    return TryDecompileTriggerFunctionAttachPointParameter(expression, out functionParameter);

                case "buffcode":
                    return TryDecompileTriggerFunctionBuffCodeParameter(expression, out functionParameter);

                case "code":
                    return TryDecompileTriggerFunctionCodeParameter(expression, out functionParameter);

                case "heroskillcode":
                    return TryDecompileTriggerFunctionHeroSkillCodeParameter(expression, out functionParameter);

                case "imagefile":
                    return TryDecompileTriggerFunctionImageFileParameter(expression, out functionParameter);

                case "integer":
                    return TryDecompileTriggerFunctionIntegerParameter(expression, out functionParameter);

                case "itemcode":
                    return TryDecompileTriggerFunctionItemCodeParameter(expression, out functionParameter);

                case "modelfile":
                    return TryDecompileTriggerFunctionModelFileParameter(expression, out functionParameter);

                case "musicfile":
                    return TryDecompileTriggerFunctionMusicFileParameter(expression, out functionParameter);

                case "real":
                    return TryDecompileTriggerFunctionRealParameter(expression, out functionParameter);

                case "skymodelstring":
                    return TryDecompileTriggerFunctionSkyModelStringParameter(expression, out functionParameter);

                case "string":
                case "StringExt":
                    return TryDecompileTriggerFunctionStringParameter(expression, out functionParameter);

                case "stringnoformat":
                    return TryDecompileTriggerFunctionStringNoFormatParameter(expression, out functionParameter);

                case "techcode":
                    return TryDecompileTriggerFunctionTechCodeParameter(expression, out functionParameter);

                case "unitcode":
                    return TryDecompileTriggerFunctionUnitCodeParameter(expression, out functionParameter);

                default:
                    // throw new ArgumentException($"Unknown parameter type '{type}'.", nameof(type));
                    functionParameter = null;
                    return false;
            }
        }

        private bool TryDecompileTriggerFunctionParameterStringForObjectCode(IExpressionSyntax expression, ImmutableHashSet<int> knownObjectCodes, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (expression is JassFourCCLiteralExpressionSyntax fourCCLiteralExpression)
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = fourCCLiteralExpression.Value.ToJassRawcode(),
                };

                return true;
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileTriggerFunctionParameterStringForUnknownType(IExpressionSyntax expression, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter, [NotNullWhen(true)] out string? type)
        {
            if (expression is JassStringLiteralExpressionSyntax stringLiteralExpression)
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = stringLiteralExpression.Value,
                };

                type = "string";
                return true;
            }
            else if (expression is JassBooleanLiteralExpressionSyntax booleanLiteralExpression)
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = booleanLiteralExpression.Value.ToString().ToLowerInvariant(),
                };

                type = "boolean";
                return true;
            }
            else if (expression is JassFourCCLiteralExpressionSyntax fourCCLiteralExpression)
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = fourCCLiteralExpression.Value.ToJassRawcode(),
                };

                type = string.Empty;
                return true;
            }
            else if (expression is JassDecimalLiteralExpressionSyntax decimalLiteralExpression)
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = decimalLiteralExpression.Value.ToString(),
                };

                type = "integer";
                return true;
            }
            else if (expression is JassOctalLiteralExpressionSyntax octalLiteralExpression)
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = octalLiteralExpression.ToString(),
                };

                type = "integer";
                return true;
            }
            else if (expression is JassVariableReferenceExpressionSyntax variableReferenceExpression)
            {
                functionParameter = DecompileVariableTriggerFunctionParameter(variableReferenceExpression.IdentifierName.Name);
                type = Context.VariableDeclarations.TryGetValue(variableReferenceExpression.IdentifierName.Name, out var variableDeclaration) ? variableDeclaration.Type : string.Empty;
                return true;
            }
            else if (expression is JassArrayReferenceExpressionSyntax arrayReferenceExpression)
            {
                if (TryDecompileTriggerFunctionParameter(arrayReferenceExpression.Indexer, JassKeyword.Integer, out var arrayIndexer))
                {
                    functionParameter = DecompileVariableTriggerFunctionParameter(arrayReferenceExpression.IdentifierName.Name, arrayIndexer);
                    type = Context.VariableDeclarations.TryGetValue(arrayReferenceExpression.IdentifierName.Name, out var variableDeclaration) ? variableDeclaration.Type : string.Empty;
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

        private bool TryDecompileVariableTriggerFunctionParameter(string expectedType, string variableName, TriggerFunctionParameter? arrayIndexer, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (Context.VariableDeclarations.TryGetValue(variableName, out var variableDeclaration))
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

            functionParameter = null;
            return false;
        }

        private bool TryDecompileTriggerFunctionAbilCodeParameter(IExpressionSyntax expression, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            return TryDecompileTriggerFunctionParameterStringForObjectCode(expression, Context.ObjectData.KnownAbilityIds, out functionParameter);
        }

        private bool TryDecompileTriggerFunctionAttachPointParameter(IExpressionSyntax expression, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (expression is JassStringLiteralExpressionSyntax stringLiteralExpression)
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = stringLiteralExpression.Value,
                };

                return true;
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileTriggerFunctionBuffCodeParameter(IExpressionSyntax expression, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            return TryDecompileTriggerFunctionParameterStringForObjectCode(expression, Context.ObjectData.KnownBuffIds, out functionParameter);
        }

        private bool TryDecompileTriggerFunctionCodeParameter(IExpressionSyntax expression, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            functionParameter = null;
            return false;
        }

        private bool TryDecompileTriggerFunctionHeroSkillCodeParameter(IExpressionSyntax expression, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            return TryDecompileTriggerFunctionParameterStringForObjectCode(expression, Context.ObjectData.KnownAbilityIds, out functionParameter);
        }

        private bool TryDecompileTriggerFunctionImageFileParameter(IExpressionSyntax expression, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (expression is JassStringLiteralExpressionSyntax stringLiteralExpression)
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = stringLiteralExpression.Value,
                };

                return true;
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileTriggerFunctionIntegerParameter(IExpressionSyntax expression, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (expression is JassDecimalLiteralExpressionSyntax decimalLiteralExpression)
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = decimalLiteralExpression.ToString(),
                };

                return true;
            }
            else if (expression is JassOctalLiteralExpressionSyntax octalLiteralExpression)
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = octalLiteralExpression.ToString(),
                };

                return true;
            }
            else if (expression is JassUnaryExpressionSyntax unaryExpression)
            {
                if (unaryExpression.Operator == UnaryOperatorType.Plus ||
                    unaryExpression.Operator == UnaryOperatorType.Minus)
                {
                    functionParameter = new TriggerFunctionParameter
                    {
                        Type = TriggerFunctionParameterType.String,
                        Value = unaryExpression.ToString(),
                    };

                    return true;
                }
            }
            else if (expression is JassBinaryExpressionSyntax binaryExpression)
            {
                if (TryDecompileTriggerCallFunction(binaryExpression, JassKeyword.Integer, out var callFunction))
                {
                    functionParameter = new TriggerFunctionParameter
                    {
                        Type = TriggerFunctionParameterType.Function,
                        Value = callFunction.Name,
                        Function = callFunction,
                    };

                    return true;
                }
            }
            else if (expression is JassFourCCLiteralExpressionSyntax fourCCLiteralExpression)
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = fourCCLiteralExpression.Value.ToString(),
                };

                return true;
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileTriggerFunctionItemCodeParameter(IExpressionSyntax expression, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            return TryDecompileTriggerFunctionParameterStringForObjectCode(expression, Context.ObjectData.KnownItemIds, out functionParameter);
        }

        private bool TryDecompileTriggerFunctionModelFileParameter(IExpressionSyntax expression, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (expression is JassStringLiteralExpressionSyntax stringLiteralExpression)
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = stringLiteralExpression.Value,
                };

                return true;
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileTriggerFunctionMusicFileParameter(IExpressionSyntax expression, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (expression is JassStringLiteralExpressionSyntax stringLiteralExpression)
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = stringLiteralExpression.Value,
                };

                return true;
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileTriggerFunctionRealParameter(IExpressionSyntax expression, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            var value = expression.ToString();
            if (float.TryParse(value, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _))
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = value,
                };

                return true;
            }
            else if (expression is JassBinaryExpressionSyntax binaryExpression)
            {
                if (TryDecompileTriggerCallFunction(binaryExpression, JassKeyword.Real, out var callFunction))
                {
                    functionParameter = new TriggerFunctionParameter
                    {
                        Type = TriggerFunctionParameterType.Function,
                        Value = callFunction.Name,
                        Function = callFunction,
                    };

                    return true;
                }
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileTriggerFunctionSkyModelStringParameter(IExpressionSyntax expression, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (expression is JassStringLiteralExpressionSyntax stringLiteralExpression)
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = stringLiteralExpression.Value,
                };

                return true;
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileTriggerFunctionStringParameter(IExpressionSyntax expression, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (expression is JassStringLiteralExpressionSyntax stringLiteralExpression)
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = stringLiteralExpression.Value,
                };

                return true;
            }
            else if (expression is JassInvocationExpressionSyntax invocationExpression)
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
            }
            else if (expression is JassBinaryExpressionSyntax binaryExpression)
            {
                if (TryDecompileTriggerCallFunction(binaryExpression, JassKeyword.String, out var callFunction))
                {
                    functionParameter = new TriggerFunctionParameter
                    {
                        Type = TriggerFunctionParameterType.Function,
                        Value = callFunction.Name,
                        Function = callFunction,
                    };

                    return true;
                }
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileTriggerFunctionStringNoFormatParameter(IExpressionSyntax expression, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (expression is JassStringLiteralExpressionSyntax stringLiteralExpression)
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = stringLiteralExpression.Value,
                };

                return true;
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileTriggerFunctionTechCodeParameter(IExpressionSyntax expression, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            return TryDecompileTriggerFunctionParameterStringForObjectCode(expression, Context.ObjectData.KnownTechIds, out functionParameter);
        }

        private bool TryDecompileTriggerFunctionUnitCodeParameter(IExpressionSyntax expression, [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            return TryDecompileTriggerFunctionParameterStringForObjectCode(expression, Context.ObjectData.KnownUnitIds, out functionParameter);
        }
    }
}