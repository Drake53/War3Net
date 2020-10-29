// ------------------------------------------------------------------------------
// <copyright file="MapCustomTextTriggersTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Script;
using War3Net.Common.Testing;
using War3Net.IO.Mpq;

namespace War3Net.Build.Core.Tests.Script
{
    [TestClass]
    public class MapCustomTextTriggersTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapCustomTextTriggersDataRoC), DynamicDataSourceType.Method)]
        public void TestParseMapCustomTextTriggersRoC(string mapCustomTextTriggersFilePath)
        {
            TestParseMapCustomTextTriggers(mapCustomTextTriggersFilePath);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapCustomTextTriggersDataTft), DynamicDataSourceType.Method)]
        public void TestParseMapCustomTextTriggersTft(string mapCustomTextTriggersFilePath)
        {
            TestParseMapCustomTextTriggers(mapCustomTextTriggersFilePath);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapCustomTextTriggersDataNew), DynamicDataSourceType.Method)]
        public void TestParseMapCustomTextTriggersNew(string mapCustomTextTriggersFilePath)
        {
            TestParseMapCustomTextTriggers(mapCustomTextTriggersFilePath);
        }

        private static void TestParseMapCustomTextTriggers(string mapCustomTextTriggersFilePath)
        {
            using var original = FileProvider.GetFile(mapCustomTextTriggersFilePath);
            using var recreated = new MemoryStream();

            MapCustomTextTriggers.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true);
        }

        private static IEnumerable<object[]> GetMapCustomTextTriggersDataRoC() => GetMapCustomTextTriggersDataSpecificFormatVersion(MapCustomTextTriggersFormatVersion.RoC);

        private static IEnumerable<object[]> GetMapCustomTextTriggersDataTft() => GetMapCustomTextTriggersDataSpecificFormatVersion(MapCustomTextTriggersFormatVersion.Tft);

        private static IEnumerable<object[]> GetMapCustomTextTriggersDataNew() => GetMapCustomTextTriggersDataSpecificFormatVersion(null);

        private static IEnumerable<object[]> GetMapCustomTextTriggersDataSpecificFormatVersion(MapCustomTextTriggersFormatVersion? formatVersion)
        {
            foreach (var testData in GetMapCustomTextTriggersData())
            {
                using var original = FileProvider.GetFile((string)testData[0]);
                using var reader = new BinaryReader(original);

                if (original.Length >= 4)
                {
                    var actualVersion = (MapCustomTextTriggersFormatVersion?)reader.ReadInt32();
                    if (!Enum.IsDefined(typeof(MapCustomTextTriggersFormatVersion), actualVersion))
                    {
                        actualVersion = null;
                    }

                    if (formatVersion == actualVersion)
                    {
                        yield return testData;
                    }
                }
            }
        }

        private static IEnumerable<object[]> GetMapCustomTextTriggersData()
        {
            return TestDataProvider.GetDynamicData(
                MapCustomTextTriggers.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Triggers"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapCustomTextTriggers.FileName,
                SearchOption.TopDirectoryOnly,
                "Maps"));
        }
    }
}