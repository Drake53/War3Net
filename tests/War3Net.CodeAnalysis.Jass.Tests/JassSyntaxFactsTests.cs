// ------------------------------------------------------------------------------
// <copyright file="JassSyntaxFactsTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace War3Net.CodeAnalysis.Jass.Tests
{
    [TestClass]
    public class JassSyntaxFactsTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetValidIdentifiers), DynamicDataSourceType.Method)]
        public void TestValidIdentifiers(string? identifier)
        {
            Assert.IsTrue(JassSyntaxFacts.IsValidIdentifier(identifier));
        }

        [DataTestMethod]
        [DynamicData(nameof(GetInvalidIdentifiers), DynamicDataSourceType.Method)]
        public void TestInvalidIdentifiers(string? identifier)
        {
            Assert.IsFalse(JassSyntaxFacts.IsValidIdentifier(identifier));
        }

        private static IEnumerable<object?[]> GetValidIdentifiers()
        {
            yield return new object?[] { "foo" };
            yield return new object?[] { "f00" };
            yield return new object?[] { "foo_bar" };
            yield return new object?[] { "FOO" };
            yield return new object?[] { "F_0" };
        }

        private static IEnumerable<object?[]> GetInvalidIdentifiers()
        {
            yield return new object?[] { null };
            yield return new object?[] { string.Empty };
            yield return new object?[] { "_foo" };
            yield return new object?[] { "foo_" };
            yield return new object?[] { "9foo" };
            yield return new object?[] { "foo bar" };
        }
    }
}