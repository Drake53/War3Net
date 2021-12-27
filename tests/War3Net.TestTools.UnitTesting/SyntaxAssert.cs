// ------------------------------------------------------------------------------
// <copyright file="SyntaxAssert.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Pidgin;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.TestTools.UnitTesting
{
    public static class SyntaxAssert
    {
        public static void AreEqual(IExpressionSyntax? expected, IExpressionSyntax? actual)
        {
            if (!expected.NullableEquals(actual))
            {
                Assert.Fail(GetAssertFailedMessage(expected, actual));
            }
        }

        public static void AreNotEqual(IExpressionSyntax? expected, IExpressionSyntax? actual)
        {
            if (expected.NullableEquals(actual))
            {
                Assert.Fail($"Expressions are equal: '{expected?.ToString()}'<{expected?.GetType().Name ?? "null"}>");
            }
        }

        private static string GetAssertFailedMessage(object? expected, object? actual)
        {
            var expectedString = expected?.ToString();
            var actualString = actual?.ToString();
            var expectedType = expected?.GetType().Name ?? "null";
            var actualType = actual?.GetType().Name ?? "null";
            var isStringCorrect = string.Equals(expectedString, actualString, StringComparison.Ordinal);
            var isTypeCorrect = string.Equals(expectedType, actualType, StringComparison.Ordinal);

            return isStringCorrect == isTypeCorrect
                ? $"\r\nExpected: '{expectedString}'<{expectedType}>.\r\n  Actual: '{actualString}'<{actualType}>"
                : isStringCorrect
                    ? $"\r\nExpected: <{expectedType}>.\r\n  Actual: <{actualType}>."
                    : $"\r\nExpected: '{expectedString}'.\r\n  Actual: '{actualString}'.";
        }

        public static void ExpressionThrowsException(string expression)
        {
            var message = new BoxedString();
            Assert.ThrowsException<ParseException>(() => message.String = GetExpressionDisplayString(JassSyntaxFactory.ParseExpression(expression)), "\r\n{0}", message);
        }

        private static string GetExpressionDisplayString(IExpressionSyntax? expression)
        {
            if (expression is null)
            {
                return "<null>";
            }

            return $"'{expression}'<{expression.GetType().Name}>";
        }

        private class BoxedString
        {
            public string String { get; set; }

            public override string ToString() => String;
        }
    }
}