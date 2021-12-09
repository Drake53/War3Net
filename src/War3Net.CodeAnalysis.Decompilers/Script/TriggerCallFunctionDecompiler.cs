using System.Diagnostics.CodeAnalysis;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileTriggerCallFunction(JassInvocationExpressionSyntax invocationExpression, [NotNullWhen(true)] out TriggerFunction? callFunction)
        {
            var parameters = Context.TriggerData.GetParameters(TriggerFunctionType.Call, invocationExpression.IdentifierName.Name);
            if (parameters.Length == invocationExpression.Arguments.Arguments.Length)
            {
                var function = new TriggerFunction
                {
                    Type = TriggerFunctionType.Call,
                    IsEnabled = true,
                    Name = invocationExpression.IdentifierName.Name,
                };

                for (var i = 0; i < invocationExpression.Arguments.Arguments.Length; i++)
                {
                    if (TryDecompileTriggerFunctionParameter(invocationExpression.Arguments.Arguments[i], parameters[i], out var functionParameter))
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
            else
            {
                callFunction = null;
                return false;
            }
        }
    }
}