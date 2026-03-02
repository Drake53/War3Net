namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileHexadecimalLiteralExpression(
            JassLiteralExpressionSyntax hexadecimalLiteralExpression,
            string expectedType,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (string.Equals(expectedType, JassKeyword.Integer, StringComparison.Ordinal) ||
                string.Equals(expectedType, JassKeyword.Real, StringComparison.Ordinal))
            {
                var value = JassLiteral.ParseHex(hexadecimalLiteralExpression.Token.Text);

                functionParameter = new TriggerFunctionParameter
                {
                    Type = TriggerFunctionParameterType.String,
                    Value = value.ToString(CultureInfo.InvariantCulture),
                };

                return true;
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileHexadecimalLiteralExpression(
            JassLiteralExpressionSyntax hexadecimalLiteralExpression,
            [NotNullWhen(true)] out List<DecompileOption>? decompileOptions)
        {
            var value = JassLiteral.ParseHex(hexadecimalLiteralExpression.Token.Text).ToString(CultureInfo.InvariantCulture);

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