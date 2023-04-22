// ------------------------------------------------------------------------------
// <copyright file="ExpressionParserTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.TestTools.UnitTesting;

using static War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.CodeAnalysis.Jass.Tests.Parser
{
    [TestClass]
    public class ExpressionParserTests
    {
        [DataTestMethod]
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
            yield return new object?[] { @"foo( bar )", InvocationExpression(@"foo", ParseIdentifierName(@"bar")) };
            yield return new object?[] { @"foo ( a , b )", InvocationExpression(@"foo", ParseIdentifierName(@"a"), ParseIdentifierName(@"b")) };
            yield return new object?[] { @"foo(a,b)", InvocationExpression(@"foo", ParseIdentifierName(@"a"), ParseIdentifierName(@"b")) };
            yield return new object?[] { @"foo(,)" };
            yield return new object?[] { @"foo(a,)" };
            yield return new object?[] { @"foo(,b)" };

            yield return new object?[] { @"foo())" };
            yield return new object?[] { @"foo() )" };
            yield return new object?[] { @"foo( ))" };
            yield return new object?[] { @"foo( ) )" };
            #endregion

            #region ElementAccessExpression
            yield return new object?[] { @"foo[bar]", ElementAccessExpression(@"foo", ParseIdentifierName(@"bar")) };
            yield return new object?[] { @"foo[bar" };
            #endregion

            #region FunctionReferenceExpression
            yield return new object?[] { @"function foo", FunctionReferenceExpression(@"foo") };
            yield return new object?[] { @"function 6" };
            yield return new object?[] { @"function foo_" };
            #endregion

            #region IdentifierName
            yield return new object?[] { @"player_id", ParseIdentifierName(@"player_id") };
            yield return new object?[] { @"player_6", ParseIdentifierName(@"player_6") };
            yield return new object?[] { @"player_" };
            yield return new object?[] { @"_player" };
            yield return new object?[] { @"6player" };
            yield return new object?[] { @"play(er" };
            yield return new object?[] { @"play)er" };
            yield return new object?[] { @"play[er" };
            yield return new object?[] { @"play]er" };
            #endregion

            #region DecimalLiteralExpression
            yield return new object?[] { @"1", new JassLiteralExpressionSyntax(Token(JassSyntaxKind.DecimalLiteralToken, "1")) };
            yield return new object?[] { @"255", new JassLiteralExpressionSyntax(Token(JassSyntaxKind.DecimalLiteralToken, "255")) };
            yield return new object?[] { @"255abc" };
            yield return new object?[] { @"255_" };
            #endregion

            #region OctalLiteralExpression
            yield return new object?[] { @"0", new JassLiteralExpressionSyntax(Token(JassSyntaxKind.OctalLiteralToken, "0")) };
            yield return new object?[] { @"010", new JassLiteralExpressionSyntax(Token(JassSyntaxKind.OctalLiteralToken, "010")) };
            yield return new object?[] { @"0abc" };
            yield return new object?[] { @"0_" };
            #endregion

            #region HexadecimalLiteralExpression
            yield return new object?[] { @"$6", new JassLiteralExpressionSyntax(Token(JassSyntaxKind.HexadecimalLiteralToken, "$6")) };
            yield return new object?[] { @"$A", new JassLiteralExpressionSyntax(Token(JassSyntaxKind.HexadecimalLiteralToken, "$A")) };
            yield return new object?[] { @"$FF", new JassLiteralExpressionSyntax(Token(JassSyntaxKind.HexadecimalLiteralToken, "$FF")) };
            yield return new object?[] { @"0x6", new JassLiteralExpressionSyntax(Token(JassSyntaxKind.HexadecimalLiteralToken, "0x6")) };
            yield return new object?[] { @"0xA", new JassLiteralExpressionSyntax(Token(JassSyntaxKind.HexadecimalLiteralToken, "0xA")) };
            yield return new object?[] { @"0xFF", new JassLiteralExpressionSyntax(Token(JassSyntaxKind.HexadecimalLiteralToken, "0xFF")) };
            yield return new object?[] { @"0X6", new JassLiteralExpressionSyntax(Token(JassSyntaxKind.HexadecimalLiteralToken, "0X6")) };
            yield return new object?[] { @"0XA", new JassLiteralExpressionSyntax(Token(JassSyntaxKind.HexadecimalLiteralToken, "0XA")) };
            yield return new object?[] { @"0XFF", new JassLiteralExpressionSyntax(Token(JassSyntaxKind.HexadecimalLiteralToken, "0XFF")) };
            yield return new object?[] { @"$ALOL" };
            yield return new object?[] { @"$A_" };
            yield return new object?[] { @"0xLOL" };
            yield return new object?[] { @"0x_" };
            yield return new object?[] { @"0XLOL" };
            yield return new object?[] { @"0X_" };
            #endregion

            #region FourCCLiteralExpression
            yield return new object?[] { @"'hpea'", new JassLiteralExpressionSyntax(Token(JassSyntaxKind.FourCCLiteralToken, "'hpea'")) };
            yield return new object?[] { @"'hpeasant'" };
            yield return new object?[] { @"'pea'" };
            yield return new object?[] { @"''" };
            yield return new object?[] { @"'hpea" };
            #endregion

            #region RealLiteralExpression
            yield return new object?[] { @"0.", LiteralExpression(0f, precision: 0) };
            yield return new object?[] { @".0", new JassLiteralExpressionSyntax(Token(JassSyntaxKind.RealLiteralToken, ".0")) };
            yield return new object?[] { @"3.141", LiteralExpression(3.141f, precision: 3) };
            yield return new object?[] { @"." };
            yield return new object?[] { @"0.abc" };
            yield return new object?[] { @"0.0abc" };
            yield return new object?[] { @".0abc" };
            #endregion

            #region BooleanLiteralExpression
            yield return new object?[] { @"true", new JassLiteralExpressionSyntax(Token(JassSyntaxKind.TrueKeyword)) };
            yield return new object?[] { @"false", new JassLiteralExpressionSyntax(Token(JassSyntaxKind.FalseKeyword)) };
            #endregion

            #region StringLiteralExpression
            yield return new object?[] { "\"  true  \"", new JassLiteralExpressionSyntax(Token(JassSyntaxKind.StringLiteralToken, "\"  true  \"")) };
            yield return new object?[] { "\"  \\\"true\\\"  \"", new JassLiteralExpressionSyntax(Token(JassSyntaxKind.StringLiteralToken, "\"  \\\"true\\\"  \"")) };
            yield return new object?[] { "\"  \r\t\\\\  \"", new JassLiteralExpressionSyntax(Token(JassSyntaxKind.StringLiteralToken, "\"  \r\t\\\\  \"")) };
            yield return new object?[] { "\"  true" };
            yield return new object?[] { "\"  \n  \"", new JassLiteralExpressionSyntax(Token(JassSyntaxKind.StringLiteralToken, "\"  \n  \"")) };
            #endregion

            #region NullLiteralExpression
            yield return new object?[] { @"null", new JassLiteralExpressionSyntax(Token(JassSyntaxKind.NullKeyword)) };
            #endregion

            #region ParenthesizedExpression
            yield return new object?[] { @"(0)", ParenthesizedExpression(new JassLiteralExpressionSyntax(Token(JassSyntaxKind.OctalLiteralToken, "0"))) };
            yield return new object?[] { @"(1)", ParenthesizedExpression(new JassLiteralExpressionSyntax(Token(JassSyntaxKind.DecimalLiteralToken, "1"))) };
            yield return new object?[] { @"(player_id)", ParenthesizedExpression(ParseIdentifierName(@"player_id")) };
            yield return new object?[] { @"( player_id )", ParenthesizedExpression(ParseIdentifierName(@"player_id")) };
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
                    new JassLiteralExpressionSyntax(Token(JassSyntaxKind.DecimalLiteralToken, "5")),
                    new JassLiteralExpressionSyntax(Token(JassSyntaxKind.OctalLiteralToken, "0")))),
            };

            yield return new object?[]
            {
                @"(0 > foo())",
                ParenthesizedExpression(BinaryGreaterThanExpression(
                    new JassLiteralExpressionSyntax(Token(JassSyntaxKind.OctalLiteralToken, "0")),
                    InvocationExpression("foo"))),
            };

            yield return new object?[]
            {
                @"(foo() > 0)",
                ParenthesizedExpression(BinaryGreaterThanExpression(
                    InvocationExpression("foo"),
                    new JassLiteralExpressionSyntax(Token(JassSyntaxKind.OctalLiteralToken, "0")))),
            };

            yield return new object?[]
            {
                "(GetUnitState(oldUnit, UNIT_STATE_MAX_LIFE) > 0)",
                ParenthesizedExpression(BinaryGreaterThanExpression(
                    InvocationExpression(
                        "GetUnitState",
                        ParseIdentifierName("oldUnit"),
                        ParseIdentifierName("UNIT_STATE_MAX_LIFE")),
                    new JassLiteralExpressionSyntax(Token(JassSyntaxKind.OctalLiteralToken, "0")))),
            };
            #endregion

            #region UnaryExpression
            yield return new object?[] { @"+6", UnaryPlusExpression(new JassLiteralExpressionSyntax(Token(JassSyntaxKind.DecimalLiteralToken, "6"))) };
            yield return new object?[] { @"-7", UnaryMinusExpression(new JassLiteralExpressionSyntax(Token(JassSyntaxKind.DecimalLiteralToken, "7"))) };
            yield return new object?[] { @"+ 6", UnaryPlusExpression(new JassLiteralExpressionSyntax(Token(JassSyntaxKind.DecimalLiteralToken, "6"))) };
            yield return new object?[] { @"- 7", UnaryMinusExpression(new JassLiteralExpressionSyntax(Token(JassSyntaxKind.DecimalLiteralToken, "7"))) };
            yield return new object?[] { @"not true", UnaryNotExpression(new JassLiteralExpressionSyntax(Token(JassSyntaxKind.TrueKeyword))) };
            yield return new object?[] { @"not(true)", UnaryNotExpression(ParenthesizedExpression(new JassLiteralExpressionSyntax(Token(JassSyntaxKind.TrueKeyword)))) };
            yield return new object?[] { @"nottrue", ParseIdentifierName(@"nottrue") };
            #endregion

            yield return new object?[] { @"trueandfalseornull", ParseIdentifierName(@"trueandfalseornull") };

            var expr1 = BinaryAdditionExpression(
                new JassLiteralExpressionSyntax(Token(JassSyntaxKind.DecimalLiteralToken, "50")),
                new JassLiteralExpressionSyntax(Token(JassSyntaxKind.DecimalLiteralToken, "60")));

            yield return new object?[] { @"50+60", expr1 };
            yield return new object?[] { @"50 + 60", expr1 };
            yield return new object?[] { @"50 +60      ", expr1 };

            yield return new object?[]
            {
                @"2 + 6 * 10",
                BinaryAdditionExpression(
                    new JassLiteralExpressionSyntax(Token(JassSyntaxKind.DecimalLiteralToken, "2")),
                    BinaryMultiplicationExpression(
                        new JassLiteralExpressionSyntax(Token(JassSyntaxKind.DecimalLiteralToken, "6")),
                        new JassLiteralExpressionSyntax(Token(JassSyntaxKind.DecimalLiteralToken, "10")))),
            };

            yield return new object?[]
            {
                @"(2 + 6) * 10",
                BinaryMultiplicationExpression(
                    ParenthesizedExpression(BinaryAdditionExpression(
                        new JassLiteralExpressionSyntax(Token(JassSyntaxKind.DecimalLiteralToken, "2")),
                        new JassLiteralExpressionSyntax(Token(JassSyntaxKind.DecimalLiteralToken, "6")))),
                    new JassLiteralExpressionSyntax(Token(JassSyntaxKind.DecimalLiteralToken, "10"))),
            };

            yield return new object?[]
            {
                @"(player_id) * 10",
                BinaryMultiplicationExpression(
                    ParenthesizedExpression(ParseIdentifierName(@"player_id")),
                    new JassLiteralExpressionSyntax(Token(JassSyntaxKind.DecimalLiteralToken, "10"))),
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
                        ParenthesizedExpression(BinarySubtractionExpression(
                            ParseIdentifierName(@"player_id"),
                            new JassLiteralExpressionSyntax(Token(JassSyntaxKind.DecimalLiteralToken, "1"))))),
                    InvocationExpression(
                        @"ConvertedPlayer",
                        ParseIdentifierName(@"player_id"))),
            };
        }
    }
}