// ------------------------------------------------------------------------------
// <copyright file="MapEnvironmentTest.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Environment;
using War3Net.Common.Testing;

namespace War3Net.Build.Tests
{
    [TestClass]
    public class MapEnvironmentTest
    {
        [DataTestMethod]
        [DynamicData(nameof(GetDefaultEnvironmentFiles), DynamicDataSourceType.Method)]
        public void TestDefaultTileset(string environmentFilePath)
        {
            using var fileStream = File.OpenRead(environmentFilePath);
            var environment = MapEnvironment.Parse(fileStream);
            Assert.IsTrue(environment.IsDefaultTileset());
        }

        [DataTestMethod]
        [DynamicData(nameof(GetDefaultPathingFiles), DynamicDataSourceType.Method)]
        public void TestPathingMap(string pathingMapFile)
        {
            using var fileStream = File.OpenRead(pathingMapFile);
            var pathingMap = PathingMap.Parse(fileStream, true);
            using var memoryStream = new MemoryStream();
            pathingMap.SerializeTo(memoryStream, true);

            StreamAssert.AreEqual(fileStream, memoryStream, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetDefaultRegionFiles), DynamicDataSourceType.Method)]
        public void TestMapRegions(string regionsFilePath)
        {
            using var fileStream = File.OpenRead(regionsFilePath);
            var mapRegions = MapRegions.Parse(fileStream);
        }

        private static IEnumerable<object[]> GetDefaultEnvironmentFiles()
        {
            foreach (var env in Directory.EnumerateFiles(@".\TestData\Environment\Default"))
            {
                yield return new[] { env };
            }
        }

        private static IEnumerable<object[]> GetDefaultPathingFiles()
        {
            foreach (var pathingMapFile in Directory.EnumerateFiles(@".\TestData\Pathing"))
            {
                yield return new[] { pathingMapFile };
            }
        }

        private static IEnumerable<object[]> GetDefaultRegionFiles()
        {
            foreach (var mapRegions in Directory.EnumerateFiles(@".\TestData\Region"))
            {
                yield return new[] { mapRegions };
            }
        }
    }
}