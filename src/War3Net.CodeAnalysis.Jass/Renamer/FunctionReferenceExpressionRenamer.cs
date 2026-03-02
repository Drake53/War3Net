namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameFunctionReferenceExpression(JassFunctionReferenceExpressionSyntax functionReferenceExpression, [NotNullWhen(true)] out JassExpressionSyntax? renamedFunctionReferenceExpression)
        {
            if (TryRenameFunctionIdentifierName(functionReferenceExpression.IdentifierName, out var renamedIdentifierName))
            {
                renamedFunctionReferenceExpression = new JassFunctionReferenceExpressionSyntax(
                    functionReferenceExpression.FunctionToken,
                    renamedIdentifierName);

                return true;
            }

            renamedFunctionReferenceExpression = null;
            return false;
        }
    }
}