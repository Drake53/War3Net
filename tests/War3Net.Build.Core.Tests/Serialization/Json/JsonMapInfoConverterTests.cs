// ------------------------------------------------------------------------------
// <copyright file="JsonMapInfoConverterTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.Build.Serialization.Json;
using War3Net.IO.Mpq;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests.Serialization.Json
{
    [TestClass]
    public class JsonMapInfoConverterTests
    {
        [TestMethod]
        public void TestSerializeDefaultMapInfo()
        {
            TestSerializeMapInfoInternal(Build.MapFactory.Info(), false);
        }

        [TestMethod]
        public void TestSerializeDefaultMapInfoStringEnums()
        {
            TestSerializeMapInfoInternal(Build.MapFactory.Info(), true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapInfoData), DynamicDataSourceType.Method)]
        public void TestSerializeMapInfo(string mapInfoFilePath)
        {
            using var mapInfoStream = MpqFile.OpenRead(mapInfoFilePath);
            using var binaryReader = new BinaryReader(mapInfoStream);

            TestSerializeMapInfoInternal(binaryReader.ReadMapInfo(), false);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapInfoData), DynamicDataSourceType.Method)]
        public void TestSerializeMapInfoStringEnums(string mapInfoFilePath)
        {
            using var mapInfoStream = MpqFile.OpenRead(mapInfoFilePath);
            using var binaryReader = new BinaryReader(mapInfoStream);

            TestSerializeMapInfoInternal(binaryReader.ReadMapInfo(), true);
        }

        private static void TestSerializeMapInfoInternal(MapInfo mapInfo, bool useStringEnumConverter)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            options.Converters.Add(new JsonStringVersionConverter());
            if (useStringEnumConverter)
            {
                options.Converters.Add(new JsonStringEnumConverter());
            }

            var json = JsonSerializer.Serialize(mapInfo, options);
            var deserializedMapInfo = JsonSerializer.Deserialize<MapInfo>(json, options);

            using var expectedStream = new MemoryStream();
            using var expectedWriter = new BinaryWriter(expectedStream);
            expectedWriter.Write(mapInfo);

            using var actualStream = new MemoryStream();
            using var actualWriter = new BinaryWriter(actualStream);
            actualWriter.Write(deserializedMapInfo);

            StreamAssert.AreEqual(expectedStream, actualStream, true, false);
        }

        private static IEnumerable<object[]> GetMapInfoData()
        {
            return TestDataProvider.GetDynamicData(
                MapInfo.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Info"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapInfo.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }
    }
}