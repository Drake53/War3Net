// ------------------------------------------------------------------------------
// <copyright file="ArrayReferenceExpressionDecompiler.cs" company="Drake53">
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
        private bool TryDecompileArrayReferenceExpression(
            JassArrayReferenceExpressionSyntax arrayReferenceExpression,
            string expectedType,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (Context.TriggerData.TriggerParams.TryGetValue(string.Empty, out var triggerParamsForAllTypes) &&
                triggerParamsForAllTypes.TryGetValue(arrayReferenceExpression.ToString(), out var triggerParams))
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

            if (TryDecompileTriggerFunctionParameter(arrayReferenceExpression.Indexer, JassKeyword.Integer, out var arrayIndexer))
            {
                return TryDecompileVariableDeclarationReference(
                    arrayReferenceExpression.IdentifierName.Name,
                    arrayIndexer,
                    expectedType,
                    out functionParameter);
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileArrayReferenceExpression(
            JassArrayReferenceExpressionSyntax arrayReferenceExpression,
            [NotNullWhen(true)] out List<DecompileOption>? decompileOptions)
        {
            if (Context.TriggerData.TriggerParams.TryGetValue(string.Empty, out var triggerParamsForAllTypes) &&
                triggerParamsForAllTypes.TryGetValue(arrayReferenceExpression.ToString(), out var triggerParams) &&
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

            if (TryDecompileTriggerFunctionParameter(arrayReferenceExpression.Indexer, JassKeyword.Integer, out var arrayIndexer))
            {
                return TryDecompileVariableDeclarationReference(
                    arrayReferenceExpression.IdentifierName.Name,
                    arrayIndexer,
                    out decompileOptions);
            }

            decompileOptions = null;
            return false;
        }
    }
}