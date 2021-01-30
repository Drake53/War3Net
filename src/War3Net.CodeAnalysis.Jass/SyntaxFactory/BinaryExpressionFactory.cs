// ------------------------------------------------------------------------------
// <copyright file="BinaryExpressionFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassBinaryExpressionSyntax BinaryAdditionExpression(IExpressionSyntax left, IExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(BinaryOperatorType.Add, left, right);
        }

        public static JassBinaryExpressionSyntax BinarySubtractionExpression(IExpressionSyntax left, IExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(BinaryOperatorType.Subtract, left, right);
        }

        public static JassBinaryExpressionSyntax BinaryMultiplicationExpression(IExpressionSyntax left, IExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(BinaryOperatorType.Multiplication, left, right);
        }

        public static JassBinaryExpressionSyntax BinaryDivisionExpression(IExpressionSyntax left, IExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(BinaryOperatorType.Division, left, right);
        }

        public static JassBinaryExpressionSyntax BinaryGreaterThanExpression(IExpressionSyntax left, IExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(BinaryOperatorType.GreaterThan, left, right);
        }

        public static JassBinaryExpressionSyntax BinaryLessThanExpression(IExpressionSyntax left, IExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(BinaryOperatorType.LessThan, left, right);
        }

        public static JassBinaryExpressionSyntax BinaryEqualsExpression(IExpressionSyntax left, IExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(BinaryOperatorType.Equals, left, right);
        }

        public static JassBinaryExpressionSyntax BinaryNotEqualsExpression(IExpressionSyntax left, IExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(BinaryOperatorType.NotEquals, left, right);
        }

        public static JassBinaryExpressionSyntax BinaryGreaterOrEqualExpression(IExpressionSyntax left, IExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(BinaryOperatorType.GreaterOrEqual, left, right);
        }

        public static JassBinaryExpressionSyntax BinaryLessOrEqualExpression(IExpressionSyntax left, IExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(BinaryOperatorType.LessOrEqual, left, right);
        }

        public static JassBinaryExpressionSyntax BinaryAndExpression(IExpressionSyntax left, IExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(BinaryOperatorType.And, left, right);
        }

        public static JassBinaryExpressionSyntax BinaryOrExpression(IExpressionSyntax left, IExpressionSyntax right)
        {
            return new JassBinaryExpressionSyntax(BinaryOperatorType.Or, left, right);
        }

        public static JassBinaryExpressionSyntax BinaryExpression(IExpressionSyntax left, IExpressionSyntax right, BinaryOperatorType @operator)
        {
            return new JassBinaryExpressionSyntax(@operator, left, right);
        }
    }
}