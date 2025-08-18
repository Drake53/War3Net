// ------------------------------------------------------------------------------
// <copyright file="MapSoundsTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Audio;

namespace War3Net.Build.Core.Tests.Audio
{
    [TestClass]
    public class MapSoundsTests
    {
        [TestMethod]
        [DynamicTestData(TestDataFileType.MapSounds)]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<MapSounds>.RunBinaryRWTest(filePath);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.MapSounds)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<MapSounds>.RunJsonRWTest(filePath, false);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.MapSounds)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<MapSounds>.RunJsonRWTest(filePath, true);
        }
    }
}