// ------------------------------------------------------------------------------
// <copyright file="CharacterLiteralExpressionDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileCharacterLiteralExpression(
            JassCharacterLiteralExpressionSyntax characterLiteralExpression,
            string expectedType,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (string.Equals(expectedType, JassKeyword.Integer, StringComparison.Ordinal) ||
                string.Equals(expectedType, JassKeyword.Real, StringComparison.Ordinal))
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = ((int)characterLiteralExpression.Value).ToString(CultureInfo.InvariantCulture),
                };

                return true;
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileCharacterLiteralExpression(
            JassCharacterLiteralExpressionSyntax characterLiteralExpression,
            [NotNullWhen(true)] out List<DecompileOption>? decompileOptions)
        {
            var value = ((int)characterLiteralExpression.Value).ToString(CultureInfo.InvariantCulture);

            decompileOptions = new();

            decompileOptions.Add(new DecompileOption
            {
                Type = JassKeyword.Integer,
                Parameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = value,
                },
            });

            decompileOptions.Add(new DecompileOption
            {
                Type = JassKeyword.Real,
                Parameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = value,
                },
            });

            return true;
        }
    }
}