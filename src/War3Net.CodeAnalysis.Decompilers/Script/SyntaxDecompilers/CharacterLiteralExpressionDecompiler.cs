namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileCharacterLiteralExpression(
            JassLiteralExpressionSyntax characterLiteralExpression,
            string expectedType,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (string.Equals(expectedType, JassKeyword.Integer, StringComparison.Ordinal) ||
                string.Equals(expectedType, JassKeyword.Real, StringComparison.Ordinal))
            {
                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = ((int)JassLiteral.ParseChar(characterLiteralExpression.Token.Text)).ToString(CultureInfo.InvariantCulture),
                };

                return true;
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileCharacterLiteralExpression(
            JassLiteralExpressionSyntax characterLiteralExpression,
            [NotNullWhen(true)] out List<DecompileOption>? decompileOptions)
        {
            var value = ((int)JassLiteral.ParseChar(characterLiteralExpression.Token.Text)).ToString(CultureInfo.InvariantCulture);

            decompileOptions = new();

            decompileOptions.Add(new DecompileOption
            {
                Type = JassKeyword.Integer,
                Parameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = value,
                },
            });

            decompileOptions.Add(new DecompileOption
            {
                Type = JassKeyword.Real,
                Parameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = value,
                },
            });

            return true;
        }
    }
}