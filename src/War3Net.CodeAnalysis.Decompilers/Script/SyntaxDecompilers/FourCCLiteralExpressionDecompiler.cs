// ------------------------------------------------------------------------------
// <copyright file="FourCCLiteralExpressionDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileFourCCLiteralExpression(
            JassLiteralExpressionSyntax fourCCLiteralExpression,
            string expectedType,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (TryDecompileTriggerFunctionParameterPreset(fourCCLiteralExpression.Token.Text, expectedType, out _, out functionParameter))
            {
                return true;
            }

            if (string.Equals(expectedType, JassKeyword.Integer, StringComparison.Ordinal))
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = fourCCLiteralExpression.Token.Text[1..^1],
                };

                return true;
            }
            else if (Context.TriggerData.TriggerTypes.TryGetValue(JassKeyword.Integer, out var customTypes) && customTypes.ContainsKey(expectedType))
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = fourCCLiteralExpression.Token.Text[1..^1],
                };

                return true;
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileFourCCLiteralExpression(
            JassLiteralExpressionSyntax fourCCLiteralExpression,
            [NotNullWhen(true)] out List<DecompileOption>? decompileOptions)
        {
            if (Context.TriggerData.TriggerTypes.TryGetValue(JassKeyword.Integer, out var customTypes))
            {
                decompileOptions = new();
                foreach (var customType in customTypes)
                {
                    decompileOptions.Add(new DecompileOption
                    {
                        Type = customType.Key,
                        Parameter = new TriggerFunctionParameter
                        {
                            Type = TriggerFunctionParameterType.String,
                            Value = fourCCLiteralExpression.Token.Text[1..^1],
                        },
                    });
                }

                return true;
            }

            decompileOptions = null;
            return false;
        }
    }
}