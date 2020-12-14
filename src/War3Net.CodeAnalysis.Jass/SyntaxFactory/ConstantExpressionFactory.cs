// ------------------------------------------------------------------------------
// <copyright file="ConstantExpressionFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.Common.Extensions;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static NewExpressionSyntax ConstantExpression(int integer)
        {
            return new ConstantExpressionSyntax(new IntegerSyntax(Token(SyntaxTokenType.DecimalNumber, integer.ToString()))).ToNewExpressionSyntax();
        }

        public static NewExpressionSyntax ConstantExpression(bool boolean)
        {
            return new ConstantExpressionSyntax(new BooleanSyntax(Token(boolean ? SyntaxTokenType.TrueKeyword : SyntaxTokenType.FalseKeyword))).ToNewExpressionSyntax();
        }

        public static NewExpressionSyntax ConstantExpression(string? expression)
        {
            if (expression is null)
            {
                return NullExpression();
            }

            var @string = string.IsNullOrEmpty(expression)
                ? new StringSyntax(Token(SyntaxTokenType.DoubleQuotes), Empty(), Token(SyntaxTokenType.DoubleQuotes))
                : new StringSyntax(Token(SyntaxTokenType.DoubleQuotes), Token(SyntaxTokenType.String, expression), Token(SyntaxTokenType.DoubleQuotes));

            return new ConstantExpressionSyntax(@string).ToNewExpressionSyntax();
        }

        public static NewExpressionSyntax ConstantExpression(float real, int precision = 3)
        {
            var isPositive = real >= 0f;
            var realString = $"{(isPositive ? real : -real).ToString($"F{precision}")}";

            var expr = new ConstantExpressionSyntax(Token(SyntaxTokenType.RealNumber, realString)).ToNewExpressionSyntax();
            return isPositive ? expr : UnaryExpression(expr, SyntaxTokenType.MinusOperator);
        }

        public static NewExpressionSyntax FourCCExpression(string value)
        {
            return new ConstantExpressionSyntax(new IntegerSyntax(new FourCCIntegerSyntax(
                Token(SyntaxTokenType.SingleQuote),
                Token(SyntaxTokenType.FourCCNumber, value),
                Token(SyntaxTokenType.SingleQuote))))
                .ToNewExpressionSyntax();
        }

        public static NewExpressionSyntax FourCCExpression(int value)
        {
            return FourCCExpression(value.ToRawcode());
        }

        public static NewExpressionSyntax NullExpression()
        {
            return new ConstantExpressionSyntax(Token(SyntaxTokenType.NullKeyword)).ToNewExpressionSyntax();
        }

        private static NewExpressionSyntax ToNewExpressionSyntax(this ConstantExpressionSyntax constantExpression)
        {
            return new NewExpressionSyntax(new ExpressionSyntax(constantExpression), Empty());
        }
    }
}