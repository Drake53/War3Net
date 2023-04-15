// ------------------------------------------------------------------------------
// <copyright file="BinaryOperatorParserTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace War3Net.CodeAnalysis.Jass.Tests.Parser
{
    [TestClass]
    public class BinaryOperatorParserTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetValidOperators), DynamicDataSourceType.Method)]
        public void TestValidOperators(string binaryOperator, JassSyntaxKind expected)
        {
            var actual = JassSyntaxFactory.ParseBinaryOperator(binaryOperator);
            Assert.AreEqual(expected, actual.SyntaxKind);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetInvalidOperators), DynamicDataSourceType.Method)]
        public void TestInvalidOperators(string binaryOperator)
        {
            Assert.IsFalse(JassSyntaxFactory.TryParseBinaryOperator(binaryOperator, out _));
        }

        private static IEnumerable<object?[]> GetValidOperators()
        {
            yield return new object?[] { "+", JassSyntaxKind.PlusToken };
            yield return new object?[] { "-", JassSyntaxKind.MinusToken };
            yield return new object?[] { "*", JassSyntaxKind.AsteriskToken };
            yield return new object?[] { "/", JassSyntaxKind.SlashToken };
            yield return new object?[] { ">", JassSyntaxKind.GreaterThanToken };
            yield return new object?[] { "<", JassSyntaxKind.LessThanToken };
            yield return new object?[] { "==", JassSyntaxKind.EqualsEqualsToken };
            yield return new object?[] { "!=", JassSyntaxKind.ExclamationEqualsToken };
            yield return new object?[] { ">=", JassSyntaxKind.GreaterThanEqualsToken };
            yield return new object?[] { "<=", JassSyntaxKind.LessThanEqualsToken };
            yield return new object?[] { "and", JassSyntaxKind.AndKeyword };
            yield return new object?[] { "or", JassSyntaxKind.OrKeyword };
        }

        private static IEnumerable<object?[]> GetInvalidOperators()
        {
            yield return new object?[] { string.Empty };
            yield return new object?[] { "_foo" };
            yield return new object?[] { "foo_" };
            yield return new object?[] { "9foo" };
            yield return new object?[] { "foo bar" };
            yield return new object?[] { "=" };
        }
    }
}