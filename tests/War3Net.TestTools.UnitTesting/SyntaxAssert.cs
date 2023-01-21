// ------------------------------------------------------------------------------
// <copyright file="SyntaxAssert.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Pidgin;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.TestTools.UnitTesting
{
    public static class SyntaxAssert
    {
        public static void AreEqual(JassCompilationUnitSyntax? expected, JassCompilationUnitSyntax? actual)
        {
            if (!expected.NullableEquivalentTo(actual))
            {
                Assert.Fail("Compilation units are not equal:\r\n" + GetAssertFailedMessage(expected, actual));
            }
        }

        //public static void AreEqual(IDeclarationLineSyntax? expected, IDeclarationLineSyntax? actual)
        //{
        //    if (!expected.NullableEquals(actual))
        //    {
        //        Assert.Fail("Declaration lines are not equal:\r\n" + GetAssertFailedMessage(expected, actual));
        //    }
        //}

        //public static void AreEqual(IGlobalLineSyntax? expected, IGlobalLineSyntax? actual)
        //{
        //    if (!expected.NullableEquals(actual))
        //    {
        //        Assert.Fail("Global lines are not equal:\r\n" + GetAssertFailedMessage(expected, actual));
        //    }
        //}

        //public static void AreEqual(IStatementLineSyntax? expected, IStatementLineSyntax? actual)
        //{
        //    if (!expected.NullableEquals(actual))
        //    {
        //        Assert.Fail("Statement lines are not equal:\r\n" + GetAssertFailedMessage(expected, actual));
        //    }
        //}

        public static void AreEqual(JassExpressionSyntax? expected, JassExpressionSyntax? actual)
        {
            if (!expected.NullableEquivalentTo(actual))
            {
                Assert.Fail("Expressions are not equal:\r\n" + GetAssertFailedMessage(expected, actual));
            }
        }

        public static void AreEqual(JassTopLevelDeclarationSyntax? expected, JassTopLevelDeclarationSyntax? actual)
        {
            if (!expected.NullableEquivalentTo(actual))
            {
                Assert.Fail("Declarations are not equal:\r\n" + GetAssertFailedMessage(expected, actual));
            }
        }

        public static void AreEqual(JassStatementSyntax? expected, JassStatementSyntax? actual)
        {
            if (!expected.NullableEquivalentTo(actual))
            {
                Assert.Fail("Statements are not equal:\r\n" + GetAssertFailedMessage(expected, actual));
            }
        }

        public static void AreNotEqual(JassCompilationUnitSyntax? expected, JassCompilationUnitSyntax? actual)
        {
            if (expected.NullableEquivalentTo(actual))
            {
                Assert.Fail($"Compilation units are equal:\r\n'{expected?.ToString()}'<{expected?.GetType().Name ?? "null"}>.");
            }
        }

        //public static void AreNotEqual(IDeclarationLineSyntax? expected, IDeclarationLineSyntax? actual)
        //{
        //    if (expected.NullableEquals(actual))
        //    {
        //        Assert.Fail($"Declaration lines are equal:\r\n'{expected?.ToString()}'<{expected?.GetType().Name ?? "null"}>.");
        //    }
        //}

        //public static void AreNotEqual(IGlobalLineSyntax? expected, IGlobalLineSyntax? actual)
        //{
        //    if (expected.NullableEquals(actual))
        //    {
        //        Assert.Fail($"Global lines are equal:\r\n'{expected?.ToString()}'<{expected?.GetType().Name ?? "null"}>.");
        //    }
        //}

        //public static void AreNotEqual(IStatementLineSyntax? expected, IStatementLineSyntax? actual)
        //{
        //    if (expected.NullableEquals(actual))
        //    {
        //        Assert.Fail($"Statement lines are equal:\r\n'{expected?.ToString()}'<{expected?.GetType().Name ?? "null"}>.");
        //    }
        //}

        public static void AreNotEqual(JassTopLevelDeclarationSyntax? expected, JassTopLevelDeclarationSyntax? actual)
        {
            if (expected.NullableEquivalentTo(actual))
            {
                Assert.Fail($"Declarations are equal:\r\n'{expected?.ToString()}'<{expected?.GetType().Name ?? "null"}>.");
            }
        }

        public static void AreNotEqual(JassExpressionSyntax? expected, JassExpressionSyntax? actual)
        {
            if (expected.NullableEquivalentTo(actual))
            {
                Assert.Fail($"Expressions are equal:\r\n'{expected?.ToString()}'<{expected?.GetType().Name ?? "null"}>.");
            }
        }

        public static void AreNotEqual(JassStatementSyntax? expected, JassStatementSyntax? actual)
        {
            if (expected.NullableEquivalentTo(actual))
            {
                Assert.Fail($"Statements are equal:\r\n'{expected?.ToString()}'<{expected?.GetType().Name ?? "null"}>.");
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
                ? $"Expected: '{expectedString}'<{expectedType}>.\r\n  Actual: '{actualString}'<{actualType}>."
                : isStringCorrect
                    ? $"Expected: <{expectedType}>.\r\n  Actual: <{actualType}>."
                    : $"Expected: '{expectedString}'.\r\n  Actual: '{actualString}'.";
        }

        private static string GetAssertFailedMessage(JassCompilationUnitSyntax? expected, JassCompilationUnitSyntax? actual)
        {
            if (expected is not null && actual is not null)
            {
                var messageParts = new List<string>();

                var length = expected.Declarations.Length;
                if (expected.Declarations.Length != actual.Declarations.Length)
                {
                    messageParts.Add($"Expected: {expected.Declarations.Length} declarations.");
                    messageParts.Add($"  Actual: {actual.Declarations.Length} declarations.");

                    if (expected.Declarations.Length > actual.Declarations.Length)
                    {
                        length = actual.Declarations.Length;
                    }
                }

                for (var i = 0; i < length; i++)
                {
                    if (!expected.Declarations[i].Equals(actual.Declarations[i]))
                    {
                        if (messageParts.Count > 20)
                        {
                            messageParts.Add("[...]");
                            break;
                        }

                        messageParts.Add($"Declaration #{i + 1}:");
                        messageParts.Add(GetAssertFailedMessage(expected.Declarations[i], actual.Declarations[i]));
                    }
                }

                return string.Join("\r\n", messageParts);
            }

            return GetAssertFailedMessage((object?)expected, actual);
        }

        private static string GetAssertFailedMessage(JassTopLevelDeclarationSyntax? expected, JassTopLevelDeclarationSyntax? actual)
        {
            if (expected is JassFunctionDeclarationSyntax expectedFunctionDeclaration && actual is JassFunctionDeclarationSyntax actualFunctionDeclaration)
            {
                var messageParts = new List<string>();

                if (!expectedFunctionDeclaration.FunctionDeclarator.Equals(actualFunctionDeclaration.FunctionDeclarator))
                {
                    messageParts.Add(GetAssertFailedMessage(expectedFunctionDeclaration.FunctionDeclarator, actualFunctionDeclaration.FunctionDeclarator));
                }

                var length = expectedFunctionDeclaration.Statements.Length;
                if (expectedFunctionDeclaration.Statements.Length != actualFunctionDeclaration.Statements.Length)
                {
                    messageParts.Add($"Expected: {expectedFunctionDeclaration.Statements.Length} statements.");
                    messageParts.Add($"  Actual: {actualFunctionDeclaration.Statements.Length} statements.");

                    if (expectedFunctionDeclaration.Statements.Length > actualFunctionDeclaration.Statements.Length)
                    {
                        length = actualFunctionDeclaration.Statements.Length;
                    }
                }

                for (var i = 0; i < length; i++)
                {
                    if (!expectedFunctionDeclaration.Statements[i].Equals(actualFunctionDeclaration.Statements[i]))
                    {
                        if (messageParts.Count > 20)
                        {
                            messageParts.Add("[...]");
                            break;
                        }

                        messageParts.Add($"Statement #{i + 1}:");
                        messageParts.Add(GetAssertFailedMessage(expectedFunctionDeclaration.Statements[i], actualFunctionDeclaration.Statements[i]));
                    }
                }

                return string.Join("\r\n", messageParts);
            }

            return GetAssertFailedMessage((object?)expected, actual);
        }

        public static void ExpressionThrowsException(string expression)
        {
            var message = new BoxedString();
            Assert.ThrowsException<ParseException>(() => message.String = GetExpressionDisplayString(JassSyntaxFactory.ParseExpression(expression)), "\r\n{0}", message);
        }

        private static string GetExpressionDisplayString(JassExpressionSyntax? expression)
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