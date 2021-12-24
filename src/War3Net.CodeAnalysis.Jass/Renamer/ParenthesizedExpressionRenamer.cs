// ------------------------------------------------------------------------------
// <copyright file="ParenthesizedExpressionRenamer.cs" company="Drake53">
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
        private bool TryRenameParenthesizedExpression(JassParenthesizedExpressionSyntax parenthesizedExpression, [NotNullWhen(true)] out IExpressionSyntax? renamedParenthesizedExpression)
        {
            if (TryRenameExpression(parenthesizedExpression.Expression, out var renamedExpression))
            {
                renamedParenthesizedExpression = new JassParenthesizedExpressionSyntax(renamedExpression);
                return true;
            }

            renamedParenthesizedExpression = null;
            return false;
        }
    }
}