// ------------------------------------------------------------------------------
// <copyright file="UnaryExpressionRenamer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameUnaryExpression(JassUnaryExpressionSyntax unaryExpression, [NotNullWhen(true)] out IExpressionSyntax? renamedUnaryExpression)
        {
            if (TryRenameExpression(unaryExpression.Expression, out var renamedExpression))
            {
                renamedUnaryExpression = new JassUnaryExpressionSyntax(
                    unaryExpression.Operator,
                    renamedExpression);

                return true;
            }

            renamedUnaryExpression = null;
            return false;
        }
    }
}