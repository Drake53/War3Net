namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameParenthesizedExpression(JassParenthesizedExpressionSyntax parenthesizedExpression, [NotNullWhen(true)] out JassExpressionSyntax? renamedParenthesizedExpression)
        {
            if (TryRenameExpression(parenthesizedExpression.Expression, out var renamedExpression))
            {
                renamedParenthesizedExpression = new JassParenthesizedExpressionSyntax(
                    parenthesizedExpression.OpenParenToken,
                    renamedExpression,
                    parenthesizedExpression.CloseParenToken);

                return true;
            }

            renamedParenthesizedExpression = null;
            return false;
        }
    }
}