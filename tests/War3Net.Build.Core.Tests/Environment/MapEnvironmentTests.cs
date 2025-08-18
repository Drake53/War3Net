// ------------------------------------------------------------------------------
// <copyright file="MapEnvironmentTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Environment;
using War3Net.Build.Extensions;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests.Environment
{
    [TestClass]
    public class MapEnvironmentTests
    {
#if false
        [TestMethod]
        public void TestDefaultMapEnvironment()
        {
            // Get World Editor default environment file.
            using var defaultEnvironmentStream = File.OpenRead(TestDataProvider.GetPath("MapFiles/DefaultMapFiles/war3map.w3e"));
            using var defaultEnvironmentReader = new BinaryReader(defaultEnvironmentStream);
            var defaultMapEnvironment = defaultEnvironmentReader.ReadMapEnvironment();
            defaultEnvironmentStream.Position = 0;

            // Get War3Net default environment file.
            var mapEnvironment = MapEnvironment.Default;

            // Update defaults that are different.
            var tileEnumerator = defaultMapEnvironment.TerrainTiles.GetEnumerator();
            foreach (var tile in mapEnvironment)
            {
                tileEnumerator.MoveNext();
                tile.Variation = tileEnumerator.Current.Variation;
                tile.CliffVariation = tileEnumerator.Current.CliffVariation;
            }

            // Compare files.
            using var mapEnvironmentStream = new MemoryStream();
            mapEnvironment.SerializeTo(mapEnvironmentStream, true);
            mapEnvironmentStream.Position = 0;

            StreamAssert.AreEqual(defaultEnvironmentStream, mapEnvironmentStream);
        }
#endif

        [DataTestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetMapEnvironmentFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<MapEnvironment>.RunBinaryRWTest(filePath);
        }

        [DataTestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetMapEnvironmentFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<MapEnvironment>.RunJsonRWTest(filePath, false);
        }

        [DataTestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetMapEnvironmentFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<MapEnvironment>.RunJsonRWTest(filePath, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetDefaultEnvironmentFiles), DynamicDataSourceType.Method)]
        public void TestDefaultTileset(string environmentFilePath)
        {
            using var fileStream = File.OpenRead(environmentFilePath);
            using var reader = new BinaryReader(fileStream);
            var environment = reader.ReadMapEnvironment();
            Assert.IsTrue(environment.IsDefaultTileset());
        }

        private static IEnumerable<object[]> GetDefaultEnvironmentFiles()
        {
            return TestDataProvider.GetDynamicData(
                $"*{MapEnvironment.FileExtension}",
                SearchOption.TopDirectoryOnly,
                Path.Combine("Environment", "Default"));
        }
    }
}