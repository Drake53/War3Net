namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileBinaryOperator(
            JassSyntaxToken operatorToken,
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
                    if (operatorToken.SyntaxKind == JassSyntaxKind.PlusToken)
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
                    if (TryDecompileTriggerFunctionParameter(operatorToken, triggerCall.ArgumentTypes[1], out var operatorFunctionParameter))
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