// ------------------------------------------------------------------------------
// <copyright file="PreviewIconsTests.cs" company="Drake53">
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
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests.MapFactory
{
    [TestClass]
    public class PreviewIconsTests
    {
        private const MapFiles FilesToOpen = MapFiles.Info | MapFiles.Environment | MapFiles.Units | MapFiles.PreviewIcons;

        [FlakyTestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestCreateMapPreviewIcons(string mapFilePath)
        {
            var map = Map.Open(mapFilePath, FilesToOpen);

            var expected = map.PreviewIcons;
            var actual = Build.MapFactory.PreviewIcons(map.Info, map.Environment, map.Units);

            Assert.AreEqual(expected.FormatVersion, actual.FormatVersion);
            Assert.AreEqual(expected.Icons.Count, actual.Icons.Count);

            var expectedIcons = expected.Icons
                .OrderBy(icon => icon.IconType)
                .ThenBy(icon => icon.X)
                .ThenBy(icon => icon.Y)
                .ThenBy(icon => icon.Color.R)
                .ThenBy(icon => icon.Color.G)
                .ThenBy(icon => icon.Color.B)
                .ThenBy(icon => icon.Color.A)
                .ToList();

            var actualIcons = actual.Icons
                .OrderBy(icon => icon.IconType)
                .ThenBy(icon => icon.X)
                .ThenBy(icon => icon.Y)
                .ThenBy(icon => icon.Color.R)
                .ThenBy(icon => icon.Color.G)
                .ThenBy(icon => icon.Color.B)
                .ThenBy(icon => icon.Color.A)
                .ToList();

            for (var i = 0; i < expectedIcons.Count; i++)
            {
                var expectedIcon = expectedIcons[i];
                var actualIcon = actualIcons[i];

                const int delta = 1;

                Assert.AreEqual(expectedIcon.IconType, actualIcon.IconType);
                Assert.AreEqual(expectedIcon.X, actualIcon.X, delta);
                Assert.AreEqual(expectedIcon.Y, actualIcon.Y, delta);

                if (expectedIcon.IconType == PreviewIconType.PlayerStartLocation)
                {
                    Assert.IsTrue(expectedIcon.Color.TryGetKnownPlayerColor(out var expectedPlayerColor));
                    Assert.IsTrue(actualIcon.Color.TryGetKnownPlayerColor(out var actualPlayerColor));
                    Assert.AreEqual(expectedPlayerColor, actualPlayerColor);
                }
                else
                {
                    Assert.AreEqual(expectedIcon.Color, actualIcon.Color);
                }
            }
        }

        private static IEnumerable<object[]> GetTestData()
        {
            foreach (var data in TestDataProvider.GetDynamicData("*", SearchOption.AllDirectories, "Maps"))
            {
                if (Map.TryOpen((string)data[0], out var map, FilesToOpen) &&
                    map.Info is not null &&
                    map.Environment is not null &&
                    map.Units is not null &&
                    map.PreviewIcons is not null &&
                    map.Info.CameraBoundsComplements is not null)
                {
                    yield return data;
                }
            }
        }
    }
}