// ------------------------------------------------------------------------------
// <copyright file="InitCustomTeamsTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Tests
{
    public partial class MapScriptBuilderTests
    {
        [FlakyTestMethod]
        [DynamicData(nameof(GetTestDataInitCustomTeams), DynamicDataSourceType.Method)]
        public void TestBodyInitCustomTeams(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions["InitCustomTeams"];
            var actual = testData.MapScriptBuilder.InitCustomTeams(testData.Map);

            SyntaxAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionInitCustomTeams(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey("InitCustomTeams");
            var actual = testData.MapScriptBuilder.InitCustomTeamsCondition(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestInvokeConditionInitCustomTeams(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.TryGetValue("config", out var config) &&
                config.Body.Statements.Any(statement =>
                    statement is JassCallStatementSyntax callStatement &&
                    string.Equals(callStatement.IdentifierName.Name, "InitCustomTeams", StringComparison.Ordinal));

            var actual = testData.MapScriptBuilder.InitCustomTeamsInvokeCondition(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataInitCustomTeams()
        {
            foreach (var testData in _testData)
            {
                if (testData.DeclaredFunctions.ContainsKey("InitCustomTeams"))
                {
                    yield return new object[] { testData };
                }
            }
        }
    }
}