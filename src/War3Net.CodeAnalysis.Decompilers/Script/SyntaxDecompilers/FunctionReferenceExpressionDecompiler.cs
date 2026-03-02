namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileFunctionReferenceExpression(
            JassFunctionReferenceExpressionSyntax functionReferenceExpression,
            string expectedType,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            functionParameter = null;
            return false;
        }

        private bool TryDecompileFunctionReferenceExpression(
            JassFunctionReferenceExpressionSyntax functionReferenceExpression,
            [NotNullWhen(true)] out List<DecompileOption>? decompileOptions)
        {
            decompileOptions = null;
            return false;
        }
    }
}