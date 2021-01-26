// ------------------------------------------------------------------------------
// <copyright file="UnaryExpressionRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassUnaryExpressionSyntax unaryExpression)
        {
            if (unaryExpression.Operator == UnaryOperatorType.Not)
            {
                Write($"{unaryExpression.Operator.GetSymbol()} ");
            }
            else
            {
                Write(unaryExpression.Operator.GetSymbol());
            }

            Render(unaryExpression.Expression);
        }
    }
}