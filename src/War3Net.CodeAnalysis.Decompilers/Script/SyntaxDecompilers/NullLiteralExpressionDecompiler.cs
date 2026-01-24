// ------------------------------------------------------------------------------
// <copyright file="NullLiteralExpressionDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileNullLiteralExpression(
            JassLiteralExpressionSyntax nullLiteralExpression,
            string expectedType,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            return TryDecompileTriggerFunctionParameterPreset(nullLiteralExpression.Token.Text, expectedType, out _, out functionParameter);
        }

        private bool TryDecompileNullLiteralExpression(
            JassLiteralExpressionSyntax nullLiteralExpression,
            [NotNullWhen(true)] out List<DecompileOption>? decompileOptions)
        {
            if (Context.TriggerData.TriggerParams.TryGetValue(string.Empty, out var triggerParamsForAllTypes) &&
                triggerParamsForAllTypes.TryGetValue(nullLiteralExpression.Token.Text, out var triggerParams))
            {
                decompileOptions = new();

                foreach (var triggerParam in triggerParams)
                {
                    decompileOptions.Add(new DecompileOption
                    {
                        Type = triggerParam.VariableType,
                        Parameter = new TriggerFunctionParameter
                        {
                            Type = TriggerFunctionParameterType.Preset,
                            Value = triggerParam.ParameterName,
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