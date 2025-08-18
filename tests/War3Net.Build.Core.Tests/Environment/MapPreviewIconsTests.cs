// ------------------------------------------------------------------------------
// <copyright file="MapPreviewIconsTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Environment;

namespace War3Net.Build.Core.Tests.Environment
{
    [TestClass]
    public class MapPreviewIconsTests
    {
        [TestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetMapPreviewIconsFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<MapPreviewIcons>.RunBinaryRWTest(filePath);
        }

        [TestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetMapPreviewIconsFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<MapPreviewIcons>.RunJsonRWTest(filePath, false);
        }

        [TestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetMapPreviewIconsFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<MapPreviewIcons>.RunJsonRWTest(filePath, true);
        }

#if false
        [TestMethod]
        [DynamicData(nameof(GetMapPreviewIconsMapFolders), DynamicDataSourceType.Method)]
        public void TestGenerateMapPreviewIcons(string mapFolder)
        {
            var map = Map.Open(mapFolder);

            var expected = map.PreviewIcons;
            var actual = MapFileFactory.PreviewIcons(map);

            Assert.AreEqual(expected.Icons.Count, actual.Icons.Count);

            for (var i = 0; i < expected.Icons.Count; i++)
            {
                var expectedIcon = expected.Icons[i];
                var actualIcon = actual.Icons[i];

                const int delta = 1;
                Assert.AreEqual(expectedIcon.IconType, actualIcon.IconType);
                Assert.AreEqual(expectedIcon.X, actualIcon.X, delta);
                Assert.AreEqual(expectedIcon.Y, actualIcon.Y, delta);
                Assert.AreEqual(expectedIcon.Color.ToArgb(), actualIcon.Color.ToArgb());
            }
        }

        private static IEnumerable<object[]> GetMapPreviewIconsMapFolders()
        {
            yield return new[] { TestDataProvider.GetPath("MapFiles/TestIcons1") };
            yield return new[] { TestDataProvider.GetPath("MapFiles/TestIcons2") };
        }
#endif
    }
}