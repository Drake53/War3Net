// ------------------------------------------------------------------------------
// <copyright file="UnaryExpressionFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static NewExpressionSyntax UnaryPlusExpression(NewExpressionSyntax expression)
        {
            return UnaryExpression(expression, SyntaxTokenType.PlusOperator);
        }

        public static NewExpressionSyntax UnaryMinusExpression(NewExpressionSyntax expression)
        {
            return UnaryExpression(expression, SyntaxTokenType.MinusOperator);
        }

        public static NewExpressionSyntax UnaryNotExpression(NewExpressionSyntax expression)
        {
            return UnaryExpression(expression, SyntaxTokenType.NotOperator);
        }

        private static NewExpressionSyntax UnaryExpression(NewExpressionSyntax expression, SyntaxTokenType @operator)
        {
            return new NewExpressionSyntax(
                new ExpressionSyntax(
                    new UnaryExpressionSyntax(
                        new UnaryOperatorSyntax(Token(@operator)),
                        expression)),
                Empty());
        }
    }
}