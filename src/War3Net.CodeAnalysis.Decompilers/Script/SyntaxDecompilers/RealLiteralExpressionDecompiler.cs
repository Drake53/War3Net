// ------------------------------------------------------------------------------
// <copyright file="RealLiteralExpressionDecompiler.cs" company="Drake53">
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
        private bool TryDecompileRealLiteralExpression(
            JassLiteralExpressionSyntax realLiteralExpression,
            string expectedType,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (string.Equals(expectedType, JassKeyword.Real, StringComparison.Ordinal))
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = realLiteralExpression.Token.Text,
                };

                return true;
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileRealLiteralExpression(
            JassLiteralExpressionSyntax realLiteralExpression,
            [NotNullWhen(true)] out List<DecompileOption>? decompileOptions)
        {
            decompileOptions = new();

            decompileOptions.Add(new DecompileOption
            {
                Type = JassKeyword.Real,
                Parameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = realLiteralExpression.Token.Text,
                },
            });

            return true;
        }
    }
}