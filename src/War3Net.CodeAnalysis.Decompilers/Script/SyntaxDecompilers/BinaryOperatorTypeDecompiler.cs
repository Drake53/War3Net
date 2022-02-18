// ------------------------------------------------------------------------------
// <copyright file="BinaryOperatorTypeDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileBinaryOperatorType(
            BinaryOperatorType binaryOperatorType,
            string expectedType,
            TriggerFunctionParameter leftOperandParameter,
            TriggerFunctionParameter rightOperandParameter,
            [NotNullWhen(true)] out TriggerFunction? function)
        {
            var functionName = expectedType switch
            {
                JassKeyword.Integer => "OperatorInt",
                JassKeyword.Real => "OperatorReal",
                JassKeyword.String => "OperatorString",

                _ => throw new NotSupportedException(),
            };

            if (Context.TriggerData.TriggerData.TriggerCalls.TryGetValue(functionName, out var triggerCall))
            {
                if (triggerCall.ArgumentTypes.Length == 2)
                {
                    if (binaryOperatorType == BinaryOperatorType.Add)
                    {
                        function = new TriggerFunction
                        {
                            Type = TriggerFunctionType.Call,
                            IsEnabled = true,
                            Name = functionName,
                        };

                        function.Parameters.Add(leftOperandParameter);
                        function.Parameters.Add(rightOperandParameter);

                        return true;
                    }
                }
                else if (triggerCall.ArgumentTypes.Length == 3)
                {
                    if (TryDecompileTriggerFunctionParameter(binaryOperatorType, triggerCall.ArgumentTypes[1], out var operatorFunctionParameter))
                    {
                        function = new TriggerFunction
                        {
                            Type = TriggerFunctionType.Call,
                            IsEnabled = true,
                            Name = functionName,
                        };

                        function.Parameters.Add(leftOperandParameter);
                        function.Parameters.Add(operatorFunctionParameter);
                        function.Parameters.Add(rightOperandParameter);

                        return true;
                    }
                }
            }

            function = null;
            return false;
        }
    }
}