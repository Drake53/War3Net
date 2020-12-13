// ------------------------------------------------------------------------------
// <copyright file="BinaryExpressionFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static NewExpressionSyntax BinaryAdditionExpression(NewExpressionSyntax left, NewExpressionSyntax right)
        {
            return BinaryExpression(left, right, SyntaxTokenType.PlusOperator);
        }

        public static NewExpressionSyntax BinarySubtractionExpression(NewExpressionSyntax left, NewExpressionSyntax right)
        {
            return BinaryExpression(left, right, SyntaxTokenType.MinusOperator);
        }

        public static NewExpressionSyntax BinaryMultiplicationExpression(NewExpressionSyntax left, NewExpressionSyntax right)
        {
            return BinaryExpression(left, right, SyntaxTokenType.MultiplicationOperator);
        }

        public static NewExpressionSyntax BinaryDivisionExpression(NewExpressionSyntax left, NewExpressionSyntax right)
        {
            return BinaryExpression(left, right, SyntaxTokenType.DivisionOperator);
        }

        public static NewExpressionSyntax BinaryGreaterThanExpression(NewExpressionSyntax left, NewExpressionSyntax right)
        {
            return BinaryExpression(left, right, SyntaxTokenType.GreaterThanOperator);
        }

        public static NewExpressionSyntax BinaryLessThanExpression(NewExpressionSyntax left, NewExpressionSyntax right)
        {
            return BinaryExpression(left, right, SyntaxTokenType.LessThanOperator);
        }

        public static NewExpressionSyntax BinaryEqualsExpression(NewExpressionSyntax left, NewExpressionSyntax right)
        {
            return BinaryExpression(left, right, SyntaxTokenType.EqualityOperator);
        }

        public static NewExpressionSyntax BinaryNotEqualsExpression(NewExpressionSyntax left, NewExpressionSyntax right)
        {
            return BinaryExpression(left, right, SyntaxTokenType.UnequalityOperator);
        }

        public static NewExpressionSyntax BinaryGreaterOrEqualExpression(NewExpressionSyntax left, NewExpressionSyntax right)
        {
            return BinaryExpression(left, right, SyntaxTokenType.GreaterOrEqualOperator);
        }

        public static NewExpressionSyntax BinaryLessOrEqualExpression(NewExpressionSyntax left, NewExpressionSyntax right)
        {
            return BinaryExpression(left, right, SyntaxTokenType.LessOrEqualOperator);
        }

        public static NewExpressionSyntax BinaryAndExpression(NewExpressionSyntax left, NewExpressionSyntax right)
        {
            return BinaryExpression(left, right, SyntaxTokenType.AndOperator);
        }

        public static NewExpressionSyntax BinaryOrExpression(NewExpressionSyntax left, NewExpressionSyntax right)
        {
            return BinaryExpression(left, right, SyntaxTokenType.OrOperator);
        }

        public static NewExpressionSyntax BinaryExpression(NewExpressionSyntax left, NewExpressionSyntax right, SyntaxTokenType @operator)
        {
            return new NewExpressionSyntax(
                left.Expression,
                new BinaryExpressionTailSyntax(new BinaryOperatorSyntax(Token(@operator)), right));
        }
    }
}