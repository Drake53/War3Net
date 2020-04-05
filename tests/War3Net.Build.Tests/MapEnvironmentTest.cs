// ------------------------------------------------------------------------------
// <copyright file="MapEnvironmentTest.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Environment;
using War3Net.Build.Info;
using War3Net.Build.Providers;
using War3Net.Build.Widget;
using War3Net.Common.Testing;

namespace War3Net.Build.Tests
{
    [TestClass]
    public class MapEnvironmentTest
    {
        [DataTestMethod]
        [DynamicData(nameof(GetEnvironmentFiles), DynamicDataSourceType.Method)]
        public void TestParseMapEnvironment(string environmentFilePath)
        {
            using var original = FileProvider.GetFile(environmentFilePath);
            using var recreated = new MemoryStream();

            MapEnvironment.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPathingFiles), DynamicDataSourceType.Method)]
        public void TestParsePathingMap(string pathingMapFile)
        {
            using var original = FileProvider.GetFile(pathingMapFile);
            using var recreated = new MemoryStream();

            PathingMap.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetRegionFiles), DynamicDataSourceType.Method)]
        public void TestParseMapRegions(string regionsFilePath)
        {
            using var original = FileProvider.GetFile(regionsFilePath);
            using var recreated = new MemoryStream();

            MapRegions.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapIconsFiles), DynamicDataSourceType.Method)]
        public void TestParseMapIcons(string iconsFilePath)
        {
            using var original = FileProvider.GetFile(iconsFilePath);
            using var recreated = new MemoryStream();

            MapPreviewIcons.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetDefaultEnvironmentFiles), DynamicDataSourceType.Method)]
        public void TestDefaultTileset(string environmentFilePath)
        {
            using var fileStream = File.OpenRead(environmentFilePath);
            var environment = MapEnvironment.Parse(fileStream);
            Assert.IsTrue(environment.IsDefaultTileset());
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

                    const int delta = 1;
                    Assert.AreEqual(expectedIcon.IconType, actualIcon.IconType);
                    Assert.AreEqual(expectedIcon.X, actualIcon.X, delta);
                    Assert.AreEqual(expectedIcon.Y, actualIcon.Y, delta);
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

        private static IEnumerable<object[]> GetEnvironmentFiles()
        {
            return TestDataProvider.GetDynamicData(
                MapEnvironment.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Environment"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapEnvironment.FileName,
                SearchOption.TopDirectoryOnly,
                "Maps"));
        }

        private static IEnumerable<object[]> GetPathingFiles()
        {
            return TestDataProvider.GetDynamicData(
                PathingMap.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Pathing"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                PathingMap.FileName,
                SearchOption.TopDirectoryOnly,
                "Maps"));
        }

        private static IEnumerable<object[]> GetRegionFiles()
        {
            return TestDataProvider.GetDynamicData(
                MapRegions.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Region"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapRegions.FileName,
                SearchOption.TopDirectoryOnly,
                "Maps"));
        }

        private static IEnumerable<object[]> GetMapIconsFiles()
        {
            return TestDataProvider.GetDynamicData(
                MapPreviewIcons.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Icons"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapPreviewIcons.FileName,
                SearchOption.TopDirectoryOnly,
                "Maps"));
        }

        private static IEnumerable<object[]> GetDefaultEnvironmentFiles()
        {
            return TestDataProvider.GetDynamicData(
                MapEnvironment.FileName.GetSearchPattern(),
                SearchOption.TopDirectoryOnly,
                Path.Combine("Environment", "Default"));
        }

        private static IEnumerable<object[]> GetMapIconsMapFolders()
        {
            yield return new[] { @".\TestData\MapFiles\TestIcons1" };
            yield return new[] { @".\TestData\MapFiles\TestIcons2" };
        }
    }
}