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
            JassLiteralExpressionSyntax stringLiteralExpression,
            string expectedType,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            var stringValue = JassLiteral.ParseString(stringLiteralExpression.Token.Text);

            if (TryDecompileTriggerFunctionParameterPreset($"`{stringValue}`", expectedType, out _, out functionParameter))
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
                    Value = Regex.Unescape(stringValue),
                };

                return true;
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileStringLiteralExpression(
            JassLiteralExpressionSyntax stringLiteralExpression,
            [NotNullWhen(true)] out List<DecompileOption>? decompileOptions)
        {
            var stringValue = JassLiteral.ParseString(stringLiteralExpression.Token.Text);
            var value = Regex.Unescape(stringValue);

            decompileOptions = new();
            decompileOptions.Add(new DecompileOption
            {
                Type = JassKeyword.String,
                Parameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = value,
                },
            });

            if (Context.TriggerData.TriggerTypes.TryGetValue(JassKeyword.String, out var customTypes))
            {
                foreach (var customType in customTypes)
                {
                    if (TryDecompileTriggerFunctionParameterPreset($"`{stringValue}`", customType.Key, out _, out var functionParameter))
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
                                Value = value,
                            },
                        });
                    }
                }
            }

            return true;
        }
    }
}