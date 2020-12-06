// ------------------------------------------------------------------------------
// <copyright file="MapTriggersTests.cs" company="Drake53">
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
    public class MapTriggersTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapTriggersDataRoC), DynamicDataSourceType.Method)]
        public void TestParseMapTriggersRoC(string mapTriggersFilePath)
        {
            TestParseMapTriggers(mapTriggersFilePath);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapTriggersDataTft), DynamicDataSourceType.Method)]
        public void TestParseMapTriggersTft(string mapTriggersFilePath)
        {
            TestParseMapTriggers(mapTriggersFilePath);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapTriggersDataNew), DynamicDataSourceType.Method)]
        public void TestParseMapTriggersNew(string mapTriggersFilePath)
        {
            TestParseMapTriggers(mapTriggersFilePath);
        }

        private static void TestParseMapTriggers(string mapTriggersFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(mapTriggersFilePath, typeof(MapTriggers), additionalReadParameters: TriggerData.Default);
        }

        private static IEnumerable<object[]> GetMapTriggersDataRoC() => GetMapTriggersDataSpecificFormatVersion(MapTriggersFormatVersion.RoC);

        private static IEnumerable<object[]> GetMapTriggersDataTft() => GetMapTriggersDataSpecificFormatVersion(MapTriggersFormatVersion.Tft);

        private static IEnumerable<object[]> GetMapTriggersDataNew() => GetMapTriggersDataSpecificFormatVersion(null);

        private static IEnumerable<object[]> GetMapTriggersDataSpecificFormatVersion(MapTriggersFormatVersion? formatVersion)
        {
            foreach (var testData in GetMapTriggersData())
            {
                using var original = FileProvider.GetFile((string)testData[0]);
                using var reader = new BinaryReader(original);

                if (original.Length >= 8)
                {
                    if (reader.ReadInt32() == MapTriggers.FileFormatSignature)
                    {
                        var actualVersion = (MapTriggersFormatVersion?)reader.ReadInt32();
                        if (!Enum.IsDefined(typeof(MapTriggersFormatVersion), actualVersion))
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
        }

        private static IEnumerable<object[]> GetMapTriggersData()
        {
            return TestDataProvider.GetDynamicData(
                MapTriggers.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Triggers"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapTriggers.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }
    }
}