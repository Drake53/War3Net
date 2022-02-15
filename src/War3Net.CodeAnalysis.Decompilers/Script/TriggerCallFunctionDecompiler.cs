// ------------------------------------------------------------------------------
// <copyright file="TriggerCallFunctionDecompiler.cs" company="Drake53">
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
        private bool TryDecompileTriggerCallFunction(JassInvocationExpressionSyntax invocationExpression, [NotNullWhen(true)] out TriggerFunction? callFunction)
        {
            if (Context.TriggerData.TriggerData.TriggerCalls.TryGetValue(invocationExpression.IdentifierName.Name, out var triggerCall) &&
                triggerCall.ArgumentTypes.Length == invocationExpression.Arguments.Arguments.Length)
            {
                var function = new TriggerFunction
                {
                    Type = TriggerFunctionType.Call,
                    IsEnabled = true,
                    Name = invocationExpression.IdentifierName.Name,
                };

                for (var i = 0; i < invocationExpression.Arguments.Arguments.Length; i++)
                {
                    if (TryDecompileTriggerFunctionParameter(invocationExpression.Arguments.Arguments[i], triggerCall.ArgumentTypes[i], out var functionParameter))
                    {
                        function.Parameters.Add(functionParameter);
                    }
                    else
                    {
                        callFunction = null;
                        return false;
                    }
                }

                callFunction = function;
                return true;
            }

            callFunction = null;
            return false;
        }

        private bool TryDecompileTriggerCallFunction(JassBinaryExpressionSyntax binaryExpression, string type, [NotNullWhen(true)] out TriggerFunction? callFunction)
        {
            var functionName = type switch
            {
                JassKeyword.Integer => "OperatorInt",
                JassKeyword.Real => "OperatorReal",
                JassKeyword.String => "OperatorString",

                _ => throw new NotSupportedException(),
            };

            var argumentTypes = Context.TriggerData.TriggerData.TriggerCalls[functionName].ArgumentTypes;
            if (argumentTypes.Length == 2)
            {
                if (string.Equals(type, JassKeyword.String, StringComparison.Ordinal) &&
                    binaryExpression.Operator == BinaryOperatorType.Add &&
                    TryDecompileTriggerFunctionParameter(binaryExpression.Left, argumentTypes[0], out var leftFunctionParameter) &&
                    TryDecompileTriggerFunctionParameter(binaryExpression.Right, argumentTypes[1], out var rightFunctionParameter))
                {
                    var function = new TriggerFunction
                    {
                        Type = TriggerFunctionType.Call,
                        IsEnabled = true,
                        Name = functionName,
                    };

                    function.Parameters.Add(leftFunctionParameter);
                    function.Parameters.Add(rightFunctionParameter);

                    callFunction = function;
                    return true;
                }
            }
            else if (argumentTypes.Length == 3)
            {
                if (TryDecompileTriggerFunctionParameter(binaryExpression.Left, argumentTypes[0], out var leftFunctionParameter) &&
                    TryDecompileTriggerFunctionParameter(binaryExpression.Operator, argumentTypes[1], out var operatorFunctionParameter) &&
                    TryDecompileTriggerFunctionParameter(binaryExpression.Right, argumentTypes[2], out var rightFunctionParameter))
                {
                    var function = new TriggerFunction
                    {
                        Type = TriggerFunctionType.Call,
                        IsEnabled = true,
                        Name = functionName,
                    };

                    function.Parameters.Add(leftFunctionParameter);
                    function.Parameters.Add(operatorFunctionParameter);
                    function.Parameters.Add(rightFunctionParameter);

                    callFunction = function;
                    return true;
                }
            }

            callFunction = null;
            return false;
        }
    }
}