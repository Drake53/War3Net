// ------------------------------------------------------------------------------
// <copyright file="MapUnitsTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Widget;

namespace War3Net.Build.Core.Tests.Widget
{
    [TestClass]
    public class MapUnitsTests
    {
        [TestMethod]
        [DynamicTestData(TestDataFileType.MapUnits)]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<MapUnits>.RunBinaryRWTest(filePath);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.MapUnits)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<MapUnits>.RunJsonRWTest(filePath, false);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.MapUnits)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<MapUnits>.RunJsonRWTest(filePath, true);
        }
    }
}