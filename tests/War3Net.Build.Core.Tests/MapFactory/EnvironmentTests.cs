// ------------------------------------------------------------------------------
// <copyright file="EnvironmentTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Extensions;
using War3Net.Common.Testing;

namespace War3Net.Build.Core.Tests.MapFactory
{
    [TestClass]
    public class EnvironmentTests
    {
        [TestMethod]
        public void TestCreateMapEnvironment()
        {
            var expectedMap = Map.Open(TestDataProvider.GetPath(@"Maps\NewLuaMap.w3m"), MapFiles.Environment);

            foreach (var terrainTile in expectedMap.Environment.TerrainTiles)
            {
                terrainTile.Variation = 0;
                terrainTile.CliffVariation = 0;
            }

            var expected = expectedMap.Environment;
            var actual = Build.MapFactory.Environment(Build.MapFactory.Info());

            using var expectedStream = new MemoryStream();
            using var expectedWriter = new BinaryWriter(expectedStream);
            expectedWriter.Write(expected);

            using var actualStream = new MemoryStream();
            using var actualWriter = new BinaryWriter(actualStream);
            actualWriter.Write(actual);

            StreamAssert.AreEqual(expectedStream, actualStream, true);
        }
    }
}