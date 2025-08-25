// ------------------------------------------------------------------------------
// <copyright file="MapCustomTextTriggersTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Script;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests.Script
{
    [TestClass]
    public class MapCustomTextTriggersTests
    {
        [TestMethod]
        [FlakyDynamicTestData(
            TestDataFileType.MapCustomTextTriggers,
            "Warcraft 3 TFT-TFT multi-TFT 1.31.0.12071 multi-(8)GoldRush.w3x/war3map.wct")]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<MapCustomTextTriggers>.RunBinaryRWTest(filePath);
        }

        [FlakyTestMethod]
        [DynamicTestData(TestDataFileType.MapCustomTextTriggers)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<MapCustomTextTriggers>.RunJsonRWTest(filePath, false);
        }

        [FlakyTestMethod]
        [DynamicTestData(TestDataFileType.MapCustomTextTriggers)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<MapCustomTextTriggers>.RunJsonRWTest(filePath, true);
        }
    }
}