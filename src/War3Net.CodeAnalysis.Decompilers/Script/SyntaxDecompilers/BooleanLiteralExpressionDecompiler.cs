namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileBooleanLiteralExpression(
            JassLiteralExpressionSyntax booleanLiteralExpression,
            string expectedType,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (string.Equals(expectedType, JassKeyword.Boolean, StringComparison.Ordinal))
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = booleanLiteralExpression.Token.Text,
                };

                return true;
            }
            else if (TryDecompileTriggerFunctionParameterPreset(booleanLiteralExpression.Token.Text, expectedType, out _, out functionParameter))
            {
                return true;
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileBooleanLiteralExpression(
            JassLiteralExpressionSyntax booleanLiteralExpression,
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