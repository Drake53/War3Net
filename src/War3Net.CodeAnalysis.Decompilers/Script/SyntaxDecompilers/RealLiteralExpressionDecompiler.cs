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