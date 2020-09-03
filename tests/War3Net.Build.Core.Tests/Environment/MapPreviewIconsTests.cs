// ------------------------------------------------------------------------------
// <copyright file="MapPreviewIconsTests.cs" company="Drake53">
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
using War3Net.Build.Widget;
using War3Net.Common.Testing;
using War3Net.IO.Mpq;

namespace War3Net.Build.Core.Tests.Environment
{
    [TestClass]
    public class MapPreviewIconsTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapPreviewIconsFiles), DynamicDataSourceType.Method)]
        public void TestParseMapPreviewIcons(string iconsFilePath)
        {
            using var original = FileProvider.GetFile(iconsFilePath);
            using var recreated = new MemoryStream();

            MapPreviewIcons.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapPreviewIconsMapFolders), DynamicDataSourceType.Method)]
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

        private static IEnumerable<object[]> GetMapPreviewIconsFiles()
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

        private static IEnumerable<object[]> GetMapPreviewIconsMapFolders()
        {
            yield return new[] { @".\TestData\MapFiles\TestIcons1" };
            yield return new[] { @".\TestData\MapFiles\TestIcons2" };
        }
    }
}