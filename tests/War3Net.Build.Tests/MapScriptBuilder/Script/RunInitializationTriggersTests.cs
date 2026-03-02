// ------------------------------------------------------------------------------
// <copyright file="RunInitializationTriggersTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Tests
{
    public partial class MapScriptBuilderTests
    {
        [FlakyTestMethod]
        [DynamicData(nameof(GetTestDataRunInitializationTriggers), DynamicDataSourceType.Method)]
        public void TestBodyRunInitializationTriggers(MapScriptBuilderTestData testData)
        {
            AssertFunctionGeneratedCorrectly(
                testData,
                MapScriptBuilder.GeneratedFunctionName.RunInitializationTriggers,
                writer => testData.MapScriptBuilder.GenerateRunInitializationTriggers(testData.Map, writer));
        }

        [FlakyTestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionRunInitializationTriggers(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.RunInitializationTriggers);
            var actual = testData.MapScriptBuilder.ShouldGenerateRunInitializationTriggers(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataRunInitializationTriggers()
        {
            foreach (var testData in _testData)
            {
                if (testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.RunInitializationTriggers))
                {
                    yield return new object[] { testData };
                }
            }
        }
    }
}