namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameUnaryExpression(JassUnaryExpressionSyntax unaryExpression, [NotNullWhen(true)] out JassExpressionSyntax? renamedUnaryExpression)
        {
            if (TryRenameExpression(unaryExpression.Expression, out var renamedExpression))
            {
                renamedUnaryExpression = new JassUnaryExpressionSyntax(
                    unaryExpression.OperatorToken,
                    renamedExpression);

                return true;
            }

            renamedUnaryExpression = null;
            return false;
        }
    }
}