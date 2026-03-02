namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileUnaryExpression(
            JassUnaryExpressionSyntax unaryExpression,
            string expectedType,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            switch (unaryExpression.SyntaxKind)
            {
                case JassSyntaxKind.UnaryPlusExpression:
                case JassSyntaxKind.UnaryMinusExpression:
                    if (string.Equals(expectedType, JassKeyword.Integer, StringComparison.Ordinal) ||
                        string.Equals(expectedType, JassKeyword.Real, StringComparison.Ordinal))
                    {
                        if (TryDecompileTriggerFunctionParameter(unaryExpression.Expression, expectedType, out functionParameter))
                        {
                            functionParameter.Value = unaryExpression.OperatorToken.Text + functionParameter.Value;
                            return true;
                        }
                    }

                    break;
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileUnaryExpression(
            JassUnaryExpressionSyntax unaryExpression,
            [NotNullWhen(true)] out List<DecompileOption>? decompileOptions)
        {
            switch (unaryExpression.SyntaxKind)
            {
                case JassSyntaxKind.UnaryPlusExpression:
                case JassSyntaxKind.UnaryMinusExpression:
                    var result = new List<DecompileOption>();

                    if (TryDecompileUnaryExpression(unaryExpression, JassKeyword.Integer, out var functionParameterInt))
                    {
                        result.Add(new DecompileOption
                        {
                            Type = JassKeyword.Integer,
                            Parameter = functionParameterInt,
                        });
                    }

                    if (TryDecompileUnaryExpression(unaryExpression, JassKeyword.Real, out var functionParameterReal))
                    {
                        result.Add(new DecompileOption
                        {
                            Type = JassKeyword.Real,
                            Parameter = functionParameterReal,
                        });
                    }

                    if (result.Count > 0)
                    {
                        decompileOptions = result;
                        return true;
                    }

                    break;
            }

            decompileOptions = null;
            return false;
        }
    }
}