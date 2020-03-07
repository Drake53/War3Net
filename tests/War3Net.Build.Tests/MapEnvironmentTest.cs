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
using War3Net.Build.Info;
using War3Net.Build.Widget;
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
            var mapRegions = MapRegions.Parse(fileStream, true);
            using var memoryStream = new MemoryStream();
            mapRegions.SerializeTo(memoryStream, true);

            StreamAssert.AreEqual(fileStream, memoryStream, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetDefaultMapIconsFiles), DynamicDataSourceType.Method)]
        public void TestMapIcons(string iconsFilePath)
        {
            using var fileStream = File.OpenRead(iconsFilePath);
            var mapIcons = MapPreviewIcons.Parse(fileStream, true);
            using var memoryStream = new MemoryStream();
            mapIcons.SerializeTo(memoryStream, true);

            StreamAssert.AreEqual(fileStream, memoryStream, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapIconsMapFolders), DynamicDataSourceType.Method)]
        public void TestGenerateMapPreviewIcons(string inputFolder)
        {
            var mapInfo = MapInfo.Parse(File.OpenRead(Path.Combine(inputFolder, MapInfo.FileName)));
            var mapEnvironment = MapEnvironment.Parse(File.OpenRead(Path.Combine(inputFolder, MapEnvironment.FileName)));
            var mapUnits = MapUnits.Parse(File.OpenRead(Path.Combine(inputFolder, MapUnits.FileName)));

            var expected = MapPreviewIcons.Parse(File.OpenRead(Path.Combine(inputFolder, MapPreviewIcons.FileName)));
            var actual = new MapPreviewIcons(mapInfo, mapEnvironment, mapUnits);

            var expectedEnumerator = expected.GetEnumerator();
            var actualEnumerator = actual.GetEnumerator();
            while (true)
            {
                if (expectedEnumerator.MoveNext() & actualEnumerator.MoveNext())
                {
                    var expectedIcon = expectedEnumerator.Current;
                    var actualIcon = actualEnumerator.Current;

                    Assert.AreEqual(expectedIcon.IconType, actualIcon.IconType);
                    Assert.AreEqual(expectedIcon.X, actualIcon.X, 1);
                    Assert.AreEqual(expectedIcon.Y, actualIcon.Y, 1);
                    Assert.AreEqual(expectedIcon.Color.ToArgb(), actualIcon.Color.ToArgb());
                }
                else if (expectedEnumerator.Current != null || actualEnumerator.Current != null)
                {
                    Assert.Fail("Expected and actual icon count are not the same.");
                }
                else
                {
                    break;
                }
            }
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

        private static IEnumerable<object[]> GetDefaultMapIconsFiles()
        {
            foreach (var mapIcons in Directory.EnumerateFiles(@".\TestData\Icons"))
            {
                yield return new[] { mapIcons };
            }
        }

        private static IEnumerable<object[]> GetMapIconsMapFolders()
        {
            yield return new[] { @".\TestData\MapFiles\TestIcons1" };
            yield return new[] { @".\TestData\MapFiles\TestIcons2" };
        }
    }
}