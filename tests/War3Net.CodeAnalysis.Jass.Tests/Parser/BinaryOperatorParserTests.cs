// ------------------------------------------------------------------------------
// <copyright file="BinaryOperatorParserTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Tests.Parser
{
    [TestClass]
    public class BinaryOperatorParserTests
    {
        [TestMethod]
        [DynamicData(nameof(GetValidOperators), DynamicDataSourceType.Method)]
        public void TestValidOperators(string binaryOperator, BinaryOperatorType expected)
        {
            Assert.IsTrue(JassSyntaxFactory.TryParseBinaryOperator(binaryOperator, out var actual));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DynamicData(nameof(GetInvalidOperators), DynamicDataSourceType.Method)]
        public void TestInvalidOperators(string binaryOperator)
        {
            Assert.IsFalse(JassSyntaxFactory.TryParseBinaryOperator(binaryOperator, out _));
        }

        private static IEnumerable<object?[]> GetValidOperators()
        {
            yield return new object?[] { "+", BinaryOperatorType.Add };
            yield return new object?[] { "-", BinaryOperatorType.Subtract };
            yield return new object?[] { "*", BinaryOperatorType.Multiplication };
            yield return new object?[] { "/", BinaryOperatorType.Division };
            yield return new object?[] { ">", BinaryOperatorType.GreaterThan };
            yield return new object?[] { "<", BinaryOperatorType.LessThan };
            yield return new object?[] { "==", BinaryOperatorType.Equals };
            yield return new object?[] { "!=", BinaryOperatorType.NotEquals };
            yield return new object?[] { ">=", BinaryOperatorType.GreaterOrEqual };
            yield return new object?[] { "<=", BinaryOperatorType.LessOrEqual };
            yield return new object?[] { "and", BinaryOperatorType.And };
            yield return new object?[] { "or", BinaryOperatorType.Or };
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