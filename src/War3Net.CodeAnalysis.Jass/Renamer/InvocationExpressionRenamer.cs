using System.Diagnostics.CodeAnalysis;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameInvocationExpression(JassInvocationExpressionSyntax invocationExpression, [NotNullWhen(true)] out JassExpressionSyntax? renamedInvocationExpression)
        {
            if (TryRenameFunctionIdentifierName(invocationExpression.IdentifierName, out var renamedIdentifierName) |
                TryRenameArgumentList(invocationExpression.ArgumentList, out var renamedArguments))
            {
                renamedInvocationExpression = new JassInvocationExpressionSyntax(
                    renamedIdentifierName ?? invocationExpression.IdentifierName,
                    renamedArguments ?? invocationExpression.ArgumentList);

                return true;
            }

            renamedInvocationExpression = null;
            return false;
        }
    }
}