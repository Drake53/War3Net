using System.Diagnostics.CodeAnalysis;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameBinaryExpression(JassBinaryExpressionSyntax binaryExpression, [NotNullWhen(true)] out JassExpressionSyntax? renamedBinaryExpression)
        {
            if (TryRenameExpression(binaryExpression.Left, out var renamedLeftExpression) |
                TryRenameExpression(binaryExpression.Right, out var renamedRightExpression))
            {
                renamedBinaryExpression = new JassBinaryExpressionSyntax(
                    renamedLeftExpression ?? binaryExpression.Left,
                    binaryExpression.OperatorToken,
                    renamedRightExpression ?? binaryExpression.Right);

                return true;
            }

            renamedBinaryExpression = null;
            return false;
        }
    }
}