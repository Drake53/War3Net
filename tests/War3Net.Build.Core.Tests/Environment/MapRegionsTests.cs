// ------------------------------------------------------------------------------
// <copyright file="MapRegionsTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Environment;

namespace War3Net.Build.Core.Tests.Environment
{
    [TestClass]
    public class MapRegionsTests
    {
        [TestMethod]
        [DynamicTestData(TestDataFileType.MapRegions)]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<MapRegions>.RunBinaryRWTest(filePath);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.MapRegions)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<MapRegions>.RunJsonRWTest(filePath, false);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.MapRegions)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<MapRegions>.RunJsonRWTest(filePath, true);
        }
    }
}