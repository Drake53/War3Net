// ------------------------------------------------------------------------------
// <copyright file="MapCustomTextTriggersTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Script;

namespace War3Net.Build.Core.Tests.Script
{
    [TestClass]
    public class MapCustomTextTriggersTests
    {
        [TestMethod]
        [DynamicTestData(TestDataFileType.MapCustomTextTriggers)]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<MapCustomTextTriggers>.RunBinaryRWTest(filePath);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.MapCustomTextTriggers)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<MapCustomTextTriggers>.RunJsonRWTest(filePath, false);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.MapCustomTextTriggers)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<MapCustomTextTriggers>.RunJsonRWTest(filePath, true);
        }
    }
}