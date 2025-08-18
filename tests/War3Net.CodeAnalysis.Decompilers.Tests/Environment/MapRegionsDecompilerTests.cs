// ------------------------------------------------------------------------------
// <copyright file="MapRegionsDecompilerTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build;
using War3Net.Build.Info;
using War3Net.TestTools.UnitTesting;

namespace War3Net.CodeAnalysis.Decompilers.Tests.Environment
{
    [TestClass]
    public class MapRegionsDecompilerTests
    {
        [TestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestDecompileMapRegions(Map map)
        {
            Assert.IsTrue(new JassScriptDecompiler(map).TryDecompileMapRegions(map.Regions.FormatVersion, out var decompiledMapRegions), "Failed to decompile map regions.");

            Assert.AreEqual(map.Regions.Regions.Count, decompiledMapRegions.Regions.Count);
            for (var i = 0; i < decompiledMapRegions.Regions.Count; i++)
            {
                var expectedRegion = map.Regions.Regions[i];
                var actualRegion = decompiledMapRegions.Regions[i];

                Assert.AreEqual(expectedRegion.Name.Replace('_', ' '), actualRegion.Name, ignoreCase: false, CultureInfo.InvariantCulture);
                Assert.AreEqual(expectedRegion.Left, actualRegion.Left);
                Assert.AreEqual(expectedRegion.Bottom, actualRegion.Bottom);
                Assert.AreEqual(expectedRegion.Right, actualRegion.Right);
                Assert.AreEqual(expectedRegion.Top, actualRegion.Top);
                Assert.AreEqual(expectedRegion.WeatherType, actualRegion.WeatherType);
                Assert.AreEqual(expectedRegion.AmbientSound, actualRegion.AmbientSound);
            }
        }

        private static IEnumerable<object[]> GetTestData()
        {
            foreach (var mapPath in TestDataProvider.GetDynamicData("*", SearchOption.AllDirectories, "Maps"))
            {
                if (Map.TryOpen((string)mapPath[0], out var map, MapFiles.Info | MapFiles.Script | MapFiles.Regions) &&
                    map.Info is not null &&
                    map.Regions is not null &&
                    map.Regions.Regions.Count > 0 &&
                    map.Info.ScriptLanguage == ScriptLanguage.Jass &&
                    !string.IsNullOrEmpty(map.Script))
                {
                    yield return new[] { map };
                }
            }
        }
    }
}