// ------------------------------------------------------------------------------
// <copyright file="StringLiteralExpressionDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileStringLiteralExpression(
            JassStringLiteralExpressionSyntax stringLiteralExpression,
            string expectedType,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (TryDecompileTriggerFunctionParameterPreset($"`{stringLiteralExpression.Value}`", expectedType, out _, out functionParameter))
            {
                return true;
            }

            if (string.Equals(expectedType, JassKeyword.String, StringComparison.Ordinal) ||
                (Context.TriggerData.TriggerTypes.TryGetValue(JassKeyword.String, out var customTypes) &&
                 customTypes.ContainsKey(expectedType)))
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = Regex.Unescape(stringLiteralExpression.Value),
                };

                return true;
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileStringLiteralExpression(
            JassStringLiteralExpressionSyntax stringLiteralExpression,
            [NotNullWhen(true)] out List<DecompileOption>? decompileOptions)
        {
            if (Context.TriggerData.TriggerTypes.TryGetValue(JassKeyword.String, out var customTypes))
            {
                decompileOptions = new();

                foreach (var customType in customTypes)
                {
                    if (TryDecompileTriggerFunctionParameterPreset($"`{stringLiteralExpression.Value}`", customType.Key, out _, out var functionParameter))
                    {
                        decompileOptions.Add(new DecompileOption
                        {
                            Type = customType.Key,
                            Parameter = functionParameter,
                        });
                    }
                    else
                    {
                        decompileOptions.Add(new DecompileOption
                        {
                            Type = customType.Key,
                            Parameter = new TriggerFunctionParameter
                            {
                                Type = TriggerFunctionParameterType.String,
                                Value = Regex.Unescape(stringLiteralExpression.Value),
                            },
                        });
                    }
                }

                return true;
            }

            decompileOptions = null;
            return false;
        }
    }
}