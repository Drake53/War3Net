// ------------------------------------------------------------------------------
// <copyright file="InitAllyPrioritiesTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace War3Net.Build.Tests
{
    public partial class MapScriptBuilderTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionInitAllyPriorities(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey("InitAllyPriorities");
            var actual = testData.MapScriptBuilder.InitAllyPrioritiesCondition(testData.Map);

            Assert.AreEqual(expected, actual);
        }
    }
}