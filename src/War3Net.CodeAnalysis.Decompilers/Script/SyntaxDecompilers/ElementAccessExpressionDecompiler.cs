namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileElementAccessExpression(
            JassElementAccessExpressionSyntax elementAccessExpression,
            string expectedType,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            if (Context.TriggerData.TriggerParams.TryGetValue(string.Empty, out var triggerParamsForAllTypes) &&
                triggerParamsForAllTypes.TryGetValue(elementAccessExpression.ToString(), out var triggerParams))
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

            if (TryDecompileTriggerFunctionParameter(elementAccessExpression.ElementAccessClause.Argument, JassKeyword.Integer, out var arrayIndexer))
            {
                return TryDecompileVariableDeclarationReference(
                    elementAccessExpression.IdentifierName.Token.Text,
                    arrayIndexer,
                    expectedType,
                    out functionParameter);
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileElementAccessExpression(
            JassElementAccessExpressionSyntax elementAccessExpression,
            [NotNullWhen(true)] out List<DecompileOption>? decompileOptions)
        {
            if (Context.TriggerData.TriggerParams.TryGetValue(string.Empty, out var triggerParamsForAllTypes) &&
                triggerParamsForAllTypes.TryGetValue(elementAccessExpression.ToString(), out var triggerParams) &&
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

            if (TryDecompileTriggerFunctionParameter(elementAccessExpression.ElementAccessClause.Argument, JassKeyword.Integer, out var arrayIndexer))
            {
                return TryDecompileVariableDeclarationReference(
                    elementAccessExpression.IdentifierName.Token.Text,
                    arrayIndexer,
                    out decompileOptions);
            }

            decompileOptions = null;
            return false;
        }
    }
}