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
        [FlakyDynamicTestData(
            TestDataFileType.MapUnits,
            "Jurassic Park Survival EE v6.4.w3x/war3mapUnits.doo",
            "Units/war3mapUnits.doo")]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<MapUnits>.RunBinaryRWTest(filePath);
        }

        [TestMethod]
        [FlakyDynamicTestData(
            TestDataFileType.MapUnits,
            "Jurassic Park Survival EE v6.4.w3x/war3mapUnits.doo",
            "Units/war3mapUnits.doo")]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<MapUnits>.RunJsonRWTest(filePath, false);
        }

        [TestMethod]
        [FlakyDynamicTestData(
            TestDataFileType.MapUnits,
            "Jurassic Park Survival EE v6.4.w3x/war3mapUnits.doo",
            "Units/war3mapUnits.doo")]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<MapUnits>.RunJsonRWTest(filePath, true);
        }
    }
}