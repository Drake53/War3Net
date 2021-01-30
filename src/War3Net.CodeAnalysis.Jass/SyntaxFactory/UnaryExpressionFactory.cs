// ------------------------------------------------------------------------------
// <copyright file="UnaryExpressionFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassUnaryExpressionSyntax UnaryPlusExpression(IExpressionSyntax expression)
        {
            return new JassUnaryExpressionSyntax(UnaryOperatorType.Plus, expression);
        }

        public static JassUnaryExpressionSyntax UnaryMinusExpression(IExpressionSyntax expression)
        {
            return new JassUnaryExpressionSyntax(UnaryOperatorType.Minus, expression);
        }

        public static JassUnaryExpressionSyntax UnaryNotExpression(IExpressionSyntax expression)
        {
            return new JassUnaryExpressionSyntax(UnaryOperatorType.Not, expression);
        }
    }
}