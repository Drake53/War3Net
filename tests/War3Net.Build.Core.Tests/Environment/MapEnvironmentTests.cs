// ------------------------------------------------------------------------------
// <copyright file="MapEnvironmentTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Environment;
using War3Net.Build.Extensions;
using War3Net.Common.Testing;

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
            using var defaultEnvironmentStream = File.OpenRead(TestDataProvider.GetPath(@"MapFiles\DefaultMapFiles\war3map.w3e"));
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
        [DynamicData(nameof(GetEnvironmentFiles), DynamicDataSourceType.Method)]
        public void TestParseMapEnvironment(string environmentFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(environmentFilePath, typeof(MapEnvironment));
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

        private static IEnumerable<object[]> GetEnvironmentFiles()
        {
            return TestDataProvider.GetDynamicData(
                MapEnvironment.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Environment"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapEnvironment.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }

        private static IEnumerable<object[]> GetDefaultEnvironmentFiles()
        {
            return TestDataProvider.GetDynamicData(
                MapEnvironment.FileName.GetSearchPattern(),
                SearchOption.TopDirectoryOnly,
                Path.Combine("Environment", "Default"));
        }
    }
}