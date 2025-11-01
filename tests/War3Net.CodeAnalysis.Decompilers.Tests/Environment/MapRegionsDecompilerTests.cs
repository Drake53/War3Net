// ------------------------------------------------------------------------------
// <copyright file="MapRegionsDecompilerTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build;
using War3Net.CodeAnalysis.Jass;

namespace War3Net.CodeAnalysis.Decompilers.Tests.Environment
{
    [TestClass]
    public class MapRegionsDecompilerTests
    {
        private const MapFiles FilesToOpen = MapFiles.Info | MapFiles.Script | MapFiles.Regions;

        [TestMethod]
        [DynamicTestData(FilesToOpen)]
        public void TestDecompileMapRegions(string mapFilePath)
        {
            var map = Map.Open(mapFilePath, FilesToOpen);

            var decompiledMap = new JassScriptDecompiler(JassSyntaxFactory.ParseCompilationUnit(map.Script), new DecompileOptions() { mapRegionsFormatVersion = map.Regions.FormatVersion }, map.Info).DecompileObjectManagerData();
            var decompiledMapRegions = decompiledMap.Regions;

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
    }
}