// ------------------------------------------------------------------------------
// <copyright file="BooleanLiteralExpressionDecompiler.cs" company="Drake53">
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
        private bool TryDecompileBooleanLiteralExpression(
            JassBooleanLiteralExpressionSyntax booleanLiteralExpression,
            string expectedType,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (string.Equals(expectedType, JassKeyword.Boolean, StringComparison.Ordinal))
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = booleanLiteralExpression.ToString(),
                };

                return true;
            }
            else if (TryDecompileTriggerFunctionParameterPreset(booleanLiteralExpression.ToString(), expectedType, out _, out functionParameter))
            {
                return true;
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileBooleanLiteralExpression(
            JassBooleanLiteralExpressionSyntax booleanLiteralExpression,
            [NotNullWhen(true)] out List<DecompileOption>? decompileOptions)
        {
            if (TryDecompileBooleanLiteralExpression(booleanLiteralExpression, JassKeyword.Boolean, out var functionParameter))
            {
                decompileOptions = new();
                decompileOptions.Add(new DecompileOption
                {
                    Type = JassKeyword.Boolean,
                    Parameter = functionParameter,
                });

                return true;
            }

            decompileOptions = null;
            return false;
        }
    }
}