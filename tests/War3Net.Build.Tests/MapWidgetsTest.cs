// ------------------------------------------------------------------------------
// <copyright file="MapWidgetsTest.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Widget;
using War3Net.Common.Testing;

namespace War3Net.Build.Tests
{
    [TestClass]
    public class MapWidgetsTest
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapDoodadsData), DynamicDataSourceType.Method)]
        public void TestParseMapDoodads(string mapDoodadsFilePath)
        {
            using var recreated = new MemoryStream();
            using var original = File.OpenRead(mapDoodadsFilePath);

            var data = MapDoodads.Parse(original, true);
            Assert.AreEqual(original.Length, original.Position);

            data.SerializeTo(recreated, true);

            // TODO: skip header, since it's ignored in SerializeTo method (so always saves as TFT version)
            //if (data.)

            StreamAssert.AreEqual(original, recreated, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapUnitsData), DynamicDataSourceType.Method)]
        public void TestParseMapUnits(string mapUnitsFilePath)
        {
            using var recreated = new MemoryStream();
            using var original = File.OpenRead(mapUnitsFilePath);

            var data = MapUnits.Parse(original, true);
            Assert.AreEqual(original.Length, original.Position);

            data.SerializeTo(recreated, true);

            original.Position = 4;
            using (var reader = new BinaryReader(original, new UTF8Encoding(false, true), true))
            {
                var version = (MapWidgetsVersion)reader.ReadUInt32();
                if (version != MapWidgetsVersion.TFT)
                {
                    // Can't compare streams, because serialize always saves as TFT, which sets header version to 8 and has additional data per unit.
                    return;
                }
            }

            StreamAssert.AreEqual(original, recreated, true);
        }

        private static IEnumerable<object[]> GetMapDoodadsData()
        {
            foreach (var unitData in Directory.EnumerateFiles(@".\TestData\Widget\Doodads"))
            {
                yield return new[] { unitData };
            }
        }

        private static IEnumerable<object[]> GetMapUnitsData()
        {
            foreach (var unitData in Directory.EnumerateFiles(@".\TestData\Widget\Units"))
            {
                yield return new[] { unitData };
            }
        }
    }
}