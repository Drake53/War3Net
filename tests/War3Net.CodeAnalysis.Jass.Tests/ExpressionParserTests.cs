// ------------------------------------------------------------------------------
// <copyright file="ExpressionParserTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.Common.Extensions;
using War3Net.Common.Testing;

using static War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.CodeAnalysis.Jass.Tests
{
    [TestClass]
    public class ExpressionParserTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetTestExpressions), DynamicDataSourceType.Method)]
        public void TestExpressionParser(string expression, IExpressionSyntax? expected = null)
        {
            if (expected is null)
            {
                SyntaxAssert.ExpressionThrowsException(expression);
            }
            else
            {
                var actual = ParseExpression(expression);
                SyntaxAssert.AreEqual(expected, actual);
            }
        }

        private static IEnumerable<object?[]> GetTestExpressions()
        {
            #region InvocationExpression
            yield return new object?[] { @"foo()", InvocationExpression(@"foo") };
            yield return new object?[] { @"foo( bar )", InvocationExpression(@"foo", VariableReferenceExpression(@"bar")) };
            yield return new object?[] { @"foo ( a , b )", InvocationExpression(@"foo", VariableReferenceExpression(@"a"), VariableReferenceExpression(@"b")) };
            yield return new object?[] { @"foo(a,b)", InvocationExpression(@"foo", VariableReferenceExpression(@"a"), VariableReferenceExpression(@"b")) };
            yield return new object?[] { @"foo(,)" };
            yield return new object?[] { @"foo(a,)" };
            yield return new object?[] { @"foo(,b)" };

            yield return new object?[] { @"foo())" };
            yield return new object?[] { @"foo() )" };
            yield return new object?[] { @"foo( ))" };
            yield return new object?[] { @"foo( ) )" };
            #endregion

            #region ArrayReferenceExpression
            yield return new object?[] { @"foo[bar]", ArrayReferenceExpression(@"foo", VariableReferenceExpression(@"bar")) };
            yield return new object?[] { @"foo[bar" };
            #endregion

            #region FunctionReferenceExpression
            yield return new object?[] { @"function foo", FunctionReferenceExpression(@"foo") };
            yield return new object?[] { @"function 6" };
            yield return new object?[] { @"function foo_" };
            #endregion

            #region VariableReferenceExpression
            yield return new object?[] { @"player_id", VariableReferenceExpression(@"player_id") };
            yield return new object?[] { @"player_6", VariableReferenceExpression(@"player_6") };
            yield return new object?[] { @"player_" };
            yield return new object?[] { @"_player" };
            yield return new object?[] { @"6player" };
            yield return new object?[] { @"play(er" };
            yield return new object?[] { @"play)er" };
            yield return new object?[] { @"play[er" };
            yield return new object?[] { @"play]er" };
            #endregion

            #region DecimalLiteralExpression
            yield return new object?[] { @"1", new JassDecimalLiteralExpressionSyntax(1) };
            yield return new object?[] { @"255", new JassDecimalLiteralExpressionSyntax(255) };
            yield return new object?[] { @"255abc" };
            yield return new object?[] { @"255_" };
            #endregion

            #region OctalLiteralExpression
            yield return new object?[] { @"0", new JassOctalLiteralExpressionSyntax(0) };
            yield return new object?[] { @"010", new JassOctalLiteralExpressionSyntax(8) };
            yield return new object?[] { @"0abc" };
            yield return new object?[] { @"0_" };
            #endregion

            #region HexadecimalLiteralExpression
            yield return new object?[] { @"$6", new JassHexadecimalLiteralExpressionSyntax(6) };
            yield return new object?[] { @"$A", new JassHexadecimalLiteralExpressionSyntax(10) };
            yield return new object?[] { @"$FF", new JassHexadecimalLiteralExpressionSyntax(255) };
            yield return new object?[] { @"0x6", new JassHexadecimalLiteralExpressionSyntax(6) };
            yield return new object?[] { @"0xA", new JassHexadecimalLiteralExpressionSyntax(10) };
            yield return new object?[] { @"0xFF", new JassHexadecimalLiteralExpressionSyntax(255) };
            yield return new object?[] { @"0X6", new JassHexadecimalLiteralExpressionSyntax(6) };
            yield return new object?[] { @"0XA", new JassHexadecimalLiteralExpressionSyntax(10) };
            yield return new object?[] { @"0XFF", new JassHexadecimalLiteralExpressionSyntax(255) };
            yield return new object?[] { @"$ALOL" };
            yield return new object?[] { @"$A_" };
            yield return new object?[] { @"0xLOL" };
            yield return new object?[] { @"0x_" };
            yield return new object?[] { @"0XLOL" };
            yield return new object?[] { @"0X_" };
            #endregion

            #region FourCCLiteralExpression
            yield return new object?[] { @"'hpea'", new JassFourCCLiteralExpressionSyntax(@"hpea".FromRawcode()) };
            yield return new object?[] { @"'hpeasant'" };
            yield return new object?[] { @"'pea'" };
            yield return new object?[] { @"''" };
            yield return new object?[] { @"'hpea" };
            #endregion

            #region RealLiteralExpression
            yield return new object?[] { @"0.", LiteralExpression(0f) };
            yield return new object?[] { @".0", LiteralExpression(0f) };
            yield return new object?[] { @"3.141", LiteralExpression(3.141f) };
            yield return new object?[] { @"." };
            yield return new object?[] { @"0.abc" };
            yield return new object?[] { @"0.0abc" };
            yield return new object?[] { @".0abc" };
            #endregion

            #region BooleanLiteralExpression
            yield return new object?[] { @"true", JassBooleanLiteralExpressionSyntax.True };
            yield return new object?[] { @"false", JassBooleanLiteralExpressionSyntax.False };
            #endregion

            #region StringLiteralExpression
            yield return new object?[] { "\"  true  \"", new JassStringLiteralExpressionSyntax("  true  ") };
            yield return new object?[] { "\"  \\\"true\\\"  \"", new JassStringLiteralExpressionSyntax("  \\\"true\\\"  ") };
            yield return new object?[] { "\"  \r\t\\\\  \"", new JassStringLiteralExpressionSyntax("  \r\t\\\\  ") };
            yield return new object?[] { "\"  true" };
            yield return new object?[] { "\"  \n  \"" };
            #endregion

            #region NullLiteralExpression
            yield return new object?[] { @"null", JassNullLiteralExpressionSyntax.Value };
            #endregion

            #region ParenthesizedExpression
            yield return new object?[] { @"(0)", new JassParenthesizedExpressionSyntax(new JassOctalLiteralExpressionSyntax(0)) };
            yield return new object?[] { @"(1)", new JassParenthesizedExpressionSyntax(new JassDecimalLiteralExpressionSyntax(1)) };
            yield return new object?[] { @"(player_id)", new JassParenthesizedExpressionSyntax(VariableReferenceExpression(@"player_id")) };
            yield return new object?[] { @"( player_id )", new JassParenthesizedExpressionSyntax(VariableReferenceExpression(@"player_id")) };
            yield return new object?[] { @"(player_id" };
            yield return new object?[] { @"player_id)" };
            yield return new object?[] { @"()" };

            yield return new object?[] { @"(foo())", ParenthesizedExpression(InvocationExpression("foo")), };
            yield return new object?[] { @"( foo(  ))", ParenthesizedExpression(InvocationExpression("foo")), };
            yield return new object?[] { @"( foo(  ) )", ParenthesizedExpression(InvocationExpression("foo")), };

            yield return new object?[]
            {
                @"(5 > 0)",
                ParenthesizedExpression(new JassBinaryExpressionSyntax(
                    BinaryOperatorType.GreaterThan,
                    new JassDecimalLiteralExpressionSyntax(5),
                    new JassOctalLiteralExpressionSyntax(0))),
            };

            yield return new object?[]
            {
                @"(0 > foo())",
                new JassParenthesizedExpressionSyntax(new JassBinaryExpressionSyntax(
                    BinaryOperatorType.GreaterThan,
                    InvocationExpression("foo"),
                    new JassOctalLiteralExpressionSyntax(0))),
            };

            yield return new object?[]
            {
                @"(foo() > 0)",
                new JassParenthesizedExpressionSyntax(new JassBinaryExpressionSyntax(
                    BinaryOperatorType.GreaterThan,
                    InvocationExpression("foo"),
                    new JassOctalLiteralExpressionSyntax(0))),
            };

            yield return new object?[]
            {
                "(GetUnitState(oldUnit, UNIT_STATE_MAX_LIFE) > 0)",
                new JassParenthesizedExpressionSyntax(new JassBinaryExpressionSyntax(
                    BinaryOperatorType.GreaterThan,
                    InvocationExpression(
                        "GetUnitState",
                        VariableReferenceExpression("oldUnit"),
                        VariableReferenceExpression("UNIT_STATE_MAX_LIFE")),
                    new JassOctalLiteralExpressionSyntax(0))),
            };
            #endregion

            #region UnaryExpression
            yield return new object?[] { @"+6", new JassUnaryExpressionSyntax(UnaryOperatorType.Plus, new JassDecimalLiteralExpressionSyntax(6)) };
            yield return new object?[] { @"-7", new JassUnaryExpressionSyntax(UnaryOperatorType.Minus, new JassDecimalLiteralExpressionSyntax(7)) };
            yield return new object?[] { @"+ 6", new JassUnaryExpressionSyntax(UnaryOperatorType.Plus, new JassDecimalLiteralExpressionSyntax(6)) };
            yield return new object?[] { @"- 7", new JassUnaryExpressionSyntax(UnaryOperatorType.Minus, new JassDecimalLiteralExpressionSyntax(7)) };
            yield return new object?[] { @"not true", new JassUnaryExpressionSyntax(UnaryOperatorType.Not, JassBooleanLiteralExpressionSyntax.True) };
            yield return new object?[] { @"not(true)", new JassUnaryExpressionSyntax(UnaryOperatorType.Not, new JassParenthesizedExpressionSyntax(JassBooleanLiteralExpressionSyntax.True)) };
            yield return new object?[] { @"nottrue", VariableReferenceExpression(@"nottrue") };
            #endregion

            yield return new object?[] { @"trueandfalseornull", VariableReferenceExpression(@"trueandfalseornull") };

            var expr1 = new JassBinaryExpressionSyntax(
                BinaryOperatorType.Add,
                new JassDecimalLiteralExpressionSyntax(50),
                new JassDecimalLiteralExpressionSyntax(60));

            yield return new object?[] { @"50+60", expr1 };
            yield return new object?[] { @"50 + 60", expr1 };
            yield return new object?[] { @"50 +60      ", expr1 };

            yield return new object?[]
            {
                @"2 + 6 * 10",
                new JassBinaryExpressionSyntax(
                    BinaryOperatorType.Add,
                    new JassDecimalLiteralExpressionSyntax(2),
                    new JassBinaryExpressionSyntax(
                        BinaryOperatorType.Multiplication,
                        new JassDecimalLiteralExpressionSyntax(6),
                        new JassDecimalLiteralExpressionSyntax(10))),
            };

            yield return new object?[]
            {
                @"(2 + 6) * 10",
                new JassBinaryExpressionSyntax(
                    BinaryOperatorType.Multiplication,
                    new JassParenthesizedExpressionSyntax(new JassBinaryExpressionSyntax(
                        BinaryOperatorType.Add,
                        new JassDecimalLiteralExpressionSyntax(2),
                        new JassDecimalLiteralExpressionSyntax(6))),
                    new JassDecimalLiteralExpressionSyntax(10)),
            };
            yield return new object?[]
            {
                @"(player_id) * 10",
                new JassBinaryExpressionSyntax(
                    BinaryOperatorType.Multiplication,
                    new JassParenthesizedExpressionSyntax(VariableReferenceExpression(@"player_id")),
                    new JassDecimalLiteralExpressionSyntax(10)),
            };

            yield return new object?[]
            {
                @"ExecuteFunction(function Foo)",
                InvocationExpression(
                    @"ExecuteFunction",
                    FunctionReferenceExpression(@"Foo")),
            };

            yield return new object?[]
            {
                @"FORCE_ALL_PLAYERS[(player_id - 1)] == ConvertedPlayer(player_id)",
                new JassBinaryExpressionSyntax(
                    BinaryOperatorType.Equals,
                    ArrayReferenceExpression(
                        @"FORCE_ALL_PLAYERS",
                        new JassParenthesizedExpressionSyntax(new JassBinaryExpressionSyntax(
                            BinaryOperatorType.Subtract,
                            VariableReferenceExpression(@"player_id"),
                            new JassDecimalLiteralExpressionSyntax(1)))),
                    InvocationExpression(
                        @"ConvertedPlayer",
                        VariableReferenceExpression(@"player_id"))),
            };
        }
    }
}