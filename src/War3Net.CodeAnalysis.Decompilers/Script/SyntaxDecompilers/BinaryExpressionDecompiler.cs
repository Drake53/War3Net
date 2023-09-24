// ------------------------------------------------------------------------------
// <copyright file="BinaryExpressionDecompiler.cs" company="Drake53">
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
        private bool TryDecompileBinaryExpression(
            JassBinaryExpressionSyntax binaryExpression,
            string expectedType,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (string.Equals(expectedType, JassKeyword.Integer, StringComparison.Ordinal))
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
            else if (string.Equals(expectedType, JassKeyword.Real, StringComparison.Ordinal))
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
            else if (string.Equals(expectedType, JassKeyword.String, StringComparison.Ordinal) ||
                     string.Equals(expectedType, "StringExt", StringComparison.Ordinal))
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

        private bool TryDecompileBinaryExpression(
            JassBinaryExpressionSyntax binaryExpression,
            [NotNullWhen(true)] out List<DecompileOption>? decompileOptions)
        {
            if (TryDecompileTriggerFunctionParameter(binaryExpression.Left, out var leftDecompileOptions) &&
                TryDecompileTriggerFunctionParameter(binaryExpression.Right, out var rightDecompileOptions))
            {
                var result = new List<DecompileOption>();

                var leftOptions = leftDecompileOptions.OrderBy(option => option.Type, StringComparer.Ordinal).ToArray();
                var rightOptions = rightDecompileOptions.OrderBy(option => option.Type, StringComparer.Ordinal).ToArray();

                var left = 0;
                var right = 0;
                while (left < leftOptions.Length && right < rightOptions.Length)
                {
                    var compare = string.Compare(leftOptions[left].Type, rightOptions[right].Type, StringComparison.Ordinal);
                    if (compare < 0)
                    {
                        left++;
                    }
                    else if (compare > 0)
                    {
                        right++;
                    }
                    else
                    {
                        if (TryDecompileBinaryOperatorType(
                            binaryExpression.OperatorToken.SyntaxKind,
                            leftOptions[left].Type,
                            leftOptions[left].Parameter,
                            rightOptions[right].Parameter,
                            out var function))
                        {
                            result.Add(new DecompileOption
                            {
                                Type = leftOptions[left].Type,
                                Parameter = new TriggerFunctionParameter
                                {
                                    Type = TriggerFunctionParameterType.Function,
                                    Value = function.Name,
                                    Function = function,
                                },
                            });
                        }

                        left++;
                        right++;
                    }
                }

                if (result.Count > 0)
                {
                    decompileOptions = result;
                    return true;
                }
            }

            decompileOptions = null;
            return false;
        }

        private static bool TryGetBinaryOperandsDecompileOptions(
            List<DecompileOption> leftDecompileOptions,
            List<DecompileOption> rightDecompileOptions,
            [NotNullWhen(true)] out List<BinaryDecompileOption>? decompileOptions)
        {
            var leftOptions = leftDecompileOptions.ToDictionary(option => option.Type, StringComparer.Ordinal);
            var rightOptions = rightDecompileOptions.ToDictionary(option => option.Type, StringComparer.Ordinal);

            var types = leftOptions.Keys.ToHashSet(StringComparer.Ordinal);
            types.IntersectWith(rightOptions.Keys);

            if (types.Count > 0)
            {
                decompileOptions = types
                    .Select(type => new BinaryDecompileOption
                    {
                        Type = type,
                        LeftParameter = leftOptions[type].Parameter,
                        RightParameter = rightOptions[type].Parameter,
                    })
                    .ToList();

                return true;
            }

            decompileOptions = null;
            return false;
        }
    }
}