// ------------------------------------------------------------------------------
// <copyright file="ExpressionParserTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.TestTools.UnitTesting;

using static War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.CodeAnalysis.Jass.Tests.Parser
{
    [TestClass]
    public class ExpressionParserTests
    {
        [TestMethod]
        [DynamicData(nameof(GetTestExpressions), DynamicDataSourceType.Method)]
        public void TestExpressionParser(string expression, JassExpressionSyntax? expected = null)
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
            yield return new object?[] { @"foo( bar )", InvocationExpression(@"foo", IdentifierName(@"bar")) };
            yield return new object?[] { @"foo ( a , b )", InvocationExpression(@"foo", IdentifierName(@"a"), IdentifierName(@"b")) };
            yield return new object?[] { @"foo(a,b)", InvocationExpression(@"foo", IdentifierName(@"a"), IdentifierName(@"b")) };
            yield return new object?[] { @"foo(,)" };
            yield return new object?[] { @"foo(a,)" };
            yield return new object?[] { @"foo(,b)" };

            yield return new object?[] { @"foo())" };
            yield return new object?[] { @"foo() )" };
            yield return new object?[] { @"foo( ))" };
            yield return new object?[] { @"foo( ) )" };
            #endregion

            #region ArrayReferenceExpression
            yield return new object?[] { @"foo[bar]", ElementAccessExpression(@"foo", IdentifierName(@"bar")) };
            yield return new object?[] { @"foo[bar" };
            #endregion

            #region FunctionReferenceExpression
            yield return new object?[] { @"function foo", FunctionReferenceExpression(@"foo") };
            yield return new object?[] { @"function 6" };
            yield return new object?[] { @"function foo_" };
            #endregion

            #region VariableReferenceExpression
            yield return new object?[] { @"player_id", IdentifierName(@"player_id") };
            yield return new object?[] { @"player_6", IdentifierName(@"player_6") };
            yield return new object?[] { @"player_" };
            yield return new object?[] { @"_player" };
            yield return new object?[] { @"6player" };
            yield return new object?[] { @"play(er" };
            yield return new object?[] { @"play)er" };
            yield return new object?[] { @"play[er" };
            yield return new object?[] { @"play]er" };
            #endregion

            #region DecimalLiteralExpression
            yield return new object?[] { @"0", LiteralExpression(Literal(0)) };
            yield return new object?[] { @"1", LiteralExpression(Literal(1)) };
            yield return new object?[] { @"255", LiteralExpression(Literal(255)) };
            yield return new object?[] { @"255abc" };
            yield return new object?[] { @"255_" };
            #endregion

            #region OctalLiteralExpression
            yield return new object?[] { @"010", LiteralExpression(Token(JassSyntaxKind.OctalLiteralToken, "010")) };
            yield return new object?[] { @"0abc" };
            yield return new object?[] { @"0_" };
            #endregion

            #region HexadecimalLiteralExpression
            yield return new object?[] { @"$6", LiteralExpression(Token(JassSyntaxKind.HexadecimalLiteralToken, "$6")) };
            yield return new object?[] { @"$A", LiteralExpression(Token(JassSyntaxKind.HexadecimalLiteralToken, "$A")) };
            yield return new object?[] { @"$FF", LiteralExpression(Token(JassSyntaxKind.HexadecimalLiteralToken, "$FF")) };
            yield return new object?[] { @"0x6", LiteralExpression(Token(JassSyntaxKind.HexadecimalLiteralToken, "0x6")) };
            yield return new object?[] { @"0xA", LiteralExpression(Token(JassSyntaxKind.HexadecimalLiteralToken, "0xA")) };
            yield return new object?[] { @"0xFF", LiteralExpression(Token(JassSyntaxKind.HexadecimalLiteralToken, "0xFF")) };
            yield return new object?[] { @"0X6", LiteralExpression(Token(JassSyntaxKind.HexadecimalLiteralToken, "0X6")) };
            yield return new object?[] { @"0XA", LiteralExpression(Token(JassSyntaxKind.HexadecimalLiteralToken, "0XA")) };
            yield return new object?[] { @"0XFF", LiteralExpression(Token(JassSyntaxKind.HexadecimalLiteralToken, "0XFF")) };
            yield return new object?[] { @"$ALOL" };
            yield return new object?[] { @"$A_" };
            yield return new object?[] { @"0xLOL" };
            yield return new object?[] { @"0x_" };
            yield return new object?[] { @"0XLOL" };
            yield return new object?[] { @"0X_" };
            #endregion

            #region FourCCLiteralExpression
            yield return new object?[] { @"'hpea'", LiteralExpression(FourCCLiteral(@"hpea".FromJassRawcode())) };
            yield return new object?[] { @"'hpeasant'" };
            yield return new object?[] { @"'pea'" };
            yield return new object?[] { @"''" };
            yield return new object?[] { @"'hpea" };
            #endregion

            #region RealLiteralExpression
            yield return new object?[] { @"0.", LiteralExpression(Token(JassSyntaxKind.RealLiteralToken, "0.")) };
            yield return new object?[] { @".0", LiteralExpression(Token(JassSyntaxKind.RealLiteralToken, ".0")) };
            yield return new object?[] { @"3.141", LiteralExpression(Token(JassSyntaxKind.RealLiteralToken, "3.141")) };
            yield return new object?[] { @"." };
            yield return new object?[] { @"0.abc" };
            yield return new object?[] { @"0.0abc" };
            yield return new object?[] { @".0abc" };
            #endregion

            #region BooleanLiteralExpression
            yield return new object?[] { @"true", LiteralExpression(Literal(true)) };
            yield return new object?[] { @"false", LiteralExpression(Literal(false)) };
            #endregion

            #region StringLiteralExpression
            yield return new object?[] { "\"  true  \"", LiteralExpression(Literal("  true  ")) };
            yield return new object?[] { "\"  \\\"true\\\"  \"", LiteralExpression(Literal("  \\\"true\\\"  ")) };
            yield return new object?[] { "\"  \r\t\\\\  \"", LiteralExpression(Literal("  \r\t\\\\  ")) };
            yield return new object?[] { "\"  true" };
            yield return new object?[] { "\"  \n  \"", LiteralExpression(Literal("  \n  ")) };
            #endregion

            #region NullLiteralExpression
            yield return new object?[] { @"null", LiteralExpression(Literal(null)) };
            #endregion

            #region ParenthesizedExpression
            yield return new object?[] { @"(0)", ParenthesizedExpression(LiteralExpression(Literal(0))) };
            yield return new object?[] { @"(1)", ParenthesizedExpression(LiteralExpression(Literal(1))) };
            yield return new object?[] { @"(player_id)", ParenthesizedExpression(IdentifierName(@"player_id")) };
            yield return new object?[] { @"( player_id )", ParenthesizedExpression(IdentifierName(@"player_id")) };
            yield return new object?[] { @"(player_id" };
            yield return new object?[] { @"player_id)" };
            yield return new object?[] { @"()" };

            yield return new object?[] { @"(foo())", ParenthesizedExpression(InvocationExpression("foo")), };
            yield return new object?[] { @"( foo(  ))", ParenthesizedExpression(InvocationExpression("foo")), };
            yield return new object?[] { @"( foo(  ) )", ParenthesizedExpression(InvocationExpression("foo")), };

            yield return new object?[]
            {
                @"(5 > 0)",
                ParenthesizedExpression(BinaryGreaterThanExpression(
                    LiteralExpression(Literal(5)),
                    LiteralExpression(Literal(0)))),
            };

            yield return new object?[]
            {
                @"(0 > foo())",
                ParenthesizedExpression(BinaryGreaterThanExpression(
                    LiteralExpression(Literal(0)),
                    InvocationExpression("foo"))),
            };

            yield return new object?[]
            {
                @"(foo() > 0)",
                ParenthesizedExpression(BinaryGreaterThanExpression(
                    InvocationExpression("foo"),
                    LiteralExpression(Literal(0)))),
            };

            yield return new object?[]
            {
                "(GetUnitState(oldUnit, UNIT_STATE_MAX_LIFE) > 0)",
                ParenthesizedExpression(BinaryGreaterThanExpression(
                    InvocationExpression(
                        "GetUnitState",
                        IdentifierName("oldUnit"),
                        IdentifierName("UNIT_STATE_MAX_LIFE")),
                    LiteralExpression(Literal(0)))),
            };
            #endregion

            #region UnaryExpression
            yield return new object?[] { @"+6", UnaryPlusExpression(LiteralExpression(Literal(6))) };
            yield return new object?[] { @"-7", UnaryMinusExpression(LiteralExpression(Literal(7))) };
            yield return new object?[] { @"+ 6", UnaryPlusExpression(LiteralExpression(Literal(6))) };
            yield return new object?[] { @"- 7", UnaryMinusExpression(LiteralExpression(Literal(7))) };
            yield return new object?[] { @"not true", UnaryNotExpression(LiteralExpression(Literal(true))) };
            yield return new object?[] { @"not(true)", UnaryNotExpression(ParenthesizedExpression(LiteralExpression(Literal(true)))) };
            yield return new object?[] { @"nottrue", IdentifierName(@"nottrue") };
            #endregion

            yield return new object?[] { @"trueandfalseornull", IdentifierName(@"trueandfalseornull") };

            var expr1 = BinaryAddExpression(
                LiteralExpression(Literal(50)),
                LiteralExpression(Literal(60)));

            yield return new object?[] { @"50+60", expr1 };
            yield return new object?[] { @"50 + 60", expr1 };
            yield return new object?[] { @"50 +60      ", expr1 };

            yield return new object?[]
            {
                @"2 + 6 * 10",
                BinaryAddExpression(
                    LiteralExpression(Literal(2)),
                    BinaryMultiplyExpression(
                        LiteralExpression(Literal(6)),
                        LiteralExpression(Literal(10)))),
            };

            yield return new object?[]
            {
                @"(2 + 6) * 10",
                BinaryMultiplyExpression(
                    ParenthesizedExpression(BinaryAddExpression(
                        LiteralExpression(Literal(2)),
                        LiteralExpression(Literal(6)))),
                    LiteralExpression(Literal(10))),
            };

            yield return new object?[]
            {
                @"(player_id) * 10",
                BinaryMultiplyExpression(
                    ParenthesizedExpression(IdentifierName(@"player_id")),
                    LiteralExpression(Literal(10))),
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
                BinaryEqualsExpression(
                    ElementAccessExpression(
                        @"FORCE_ALL_PLAYERS",
                        ParenthesizedExpression(BinarySubtractExpression(
                            IdentifierName(@"player_id"),
                            LiteralExpression(Literal(1))))),
                    InvocationExpression(
                        @"ConvertedPlayer",
                        IdentifierName(@"player_id"))),
            };
        }
    }
}