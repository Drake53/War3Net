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
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.Common.Testing
{
    public static class SyntaxAssert
    {
        public static void AreEqual(IExpressionSyntax expected, IExpressionSyntax actual)
        {
            var expectedString = expected.ToString();
            var actualString = actual.ToString();
            var expectedType = expected.GetType().Name;
            var actualType = actual.GetType().Name;
            var isStringCorrect = string.Equals(expectedString, actualString, StringComparison.Ordinal);
            var isTypeCorrect = string.Equals(expectedType, actualType, StringComparison.Ordinal);
            var message = isStringCorrect == isTypeCorrect
                ? $"\r\nExpected: '{expectedString}'<{expected.GetType().Name}>.\r\n  Actual: '{actualString}'<{actual.GetType().Name}>"
                : isStringCorrect
                    ? $"\r\nExpected: <{expectedType}>.\r\n  Actual: <{actualType}>."
                    : $"\r\nExpected: '{expectedString}'.\r\n  Actual: '{actualString}'.";

            Assert.IsTrue(expected.Equals(actual), message);
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