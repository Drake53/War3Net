// ------------------------------------------------------------------------------
// <copyright file="ConstantExpressionFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static NewExpressionSyntax ConstantExpression(int integer)
        {
            return new NewExpressionSyntax(
                new ExpressionSyntax(
                    new ConstantExpressionSyntax(
                        new IntegerSyntax(
                            new TokenNode(new SyntaxToken(SyntaxTokenType.DecimalNumber, integer.ToString()), 0)))),
                new EmptyNode(0));
        }

        public static NewExpressionSyntax ConstantExpression(bool boolean)
        {
            return new NewExpressionSyntax(
                new ExpressionSyntax(
                    new ConstantExpressionSyntax(
                        new BooleanSyntax(
                            new TokenNode(
                                new SyntaxToken(boolean ? SyntaxTokenType.TrueKeyword : SyntaxTokenType.FalseKeyword), 0)))),
                new EmptyNode(0));
        }

        public static NewExpressionSyntax ConstantExpression(string expression)
        {
            if (expression == null)
            {
                return NullExpression();
            }
            else if (string.IsNullOrEmpty(expression))
            {
                return new NewExpressionSyntax(
                    new ExpressionSyntax(
                        new ConstantExpressionSyntax(
                            new StringSyntax(
                                new TokenNode(new SyntaxToken(SyntaxTokenType.DoubleQuotes), 0),
                                new EmptyNode(0),
                                new TokenNode(new SyntaxToken(SyntaxTokenType.DoubleQuotes), 0)))),
                    new EmptyNode(0));
            }
            else
            {
                return new NewExpressionSyntax(
                    new ExpressionSyntax(
                        new ConstantExpressionSyntax(
                            new StringSyntax(
                                new TokenNode(new SyntaxToken(SyntaxTokenType.DoubleQuotes), 0),
                                new TokenNode(new SyntaxToken(SyntaxTokenType.String, expression), 0),
                                new TokenNode(new SyntaxToken(SyntaxTokenType.DoubleQuotes), 0)))),
                    new EmptyNode(0));
            }
        }

        public static NewExpressionSyntax ConstantExpression(float real)
        {
            var isPositive = real >= 0f;
            var isIntegral = real % 1 == 0f;
            var realString = $"{(isPositive ? real : -real)}{(isIntegral ? "." : string.Empty)}";

            var expr = new NewExpressionSyntax(
                new ExpressionSyntax(
                    new ConstantExpressionSyntax(
                        new TokenNode(new SyntaxToken(SyntaxTokenType.RealNumber, realString), 0))),
                new EmptyNode(0));

            return isPositive
                ? expr
                : new NewExpressionSyntax(
                    new ExpressionSyntax(
                        new UnaryExpressionSyntax(
                            new UnaryOperatorSyntax(new TokenNode(new SyntaxToken(SyntaxTokenType.MinusOperator), 0)),
                            expr)),
                    new EmptyNode(0));
        }

        public static NewExpressionSyntax NullExpression()
        {
            return new NewExpressionSyntax(
                new ExpressionSyntax(
                    new ConstantExpressionSyntax(
                        new TokenNode(new SyntaxToken(SyntaxTokenType.NullKeyword), 0))),
                new EmptyNode(0));
        }
    }
}