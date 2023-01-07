// ------------------------------------------------------------------------------
// <copyright file="MapInfoTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests.Info
{
    [TestClass]
    public class MapInfoTests
    {
#if false
        [TestMethod]
        public void TestDefaultMapInfo()
        {
            // Get World Editor default info file.
            using var defaultInfoStream = File.OpenRead(TestDataProvider.GetPath(@"MapFiles\DefaultMapFiles\war3map.w3i"));
            using var defaultInfoReader = new BinaryReader(defaultInfoStream);
            var defaultMapInfo = defaultInfoReader.ReadMapInfo();
            defaultInfoStream.Position = 0;

            // Get War3Net default info file.
            var mapInfo = MapInfo.Default;

            // Update defaults that are different.
            mapInfo.EditorVersion = 6072;
            mapInfo.ScriptLanguage = ScriptLanguage.Jass;

            var player0 = mapInfo.GetPlayerData(0);
            player0.PlayerName = "TRIGSTR_001";
            player0.StartPosition = defaultMapInfo.Players[0].StartPosition;
            mapInfo.SetPlayerData(player0);

            var team0 = mapInfo.GetForceData(0);
            team0.ForceName = "TRIGSTR_002";
            team0.IncludeAllPlayers();
            mapInfo.SetForceData(team0);

            // Compare files.
            using var mapInfoStream = new MemoryStream();
            mapInfo.SerializeTo(mapInfoStream, true);
            mapInfoStream.Position = 0;

            StreamAssert.AreEqual(defaultInfoStream, mapInfoStream);
        }
#endif

        [DataTestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetMapInfoFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestParseMapInfo(string mapInfoFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(
                mapInfoFilePath,
                typeof(MapInfo));
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapInfoDataV8), DynamicDataSourceType.Method)]
        public void TestParseMapInfoV8(string mapInfoFilePath)
        {
            TestParseMapInfoInternal(mapInfoFilePath);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapInfoDataV10), DynamicDataSourceType.Method)]
        public void TestParseMapInfoV10(string mapInfoFilePath)
        {
            TestParseMapInfoInternal(mapInfoFilePath);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapInfoDataV11), DynamicDataSourceType.Method)]
        public void TestParseMapInfoV11(string mapInfoFilePath)
        {
            TestParseMapInfoInternal(mapInfoFilePath);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapInfoDataV15), DynamicDataSourceType.Method)]
        public void TestParseMapInfoV15(string mapInfoFilePath)
        {
            TestParseMapInfoInternal(mapInfoFilePath);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapInfoDataV23), DynamicDataSourceType.Method)]
        public void TestParseMapInfoV23(string mapInfoFilePath)
        {
            TestParseMapInfoInternal(mapInfoFilePath);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapInfoDataV24), DynamicDataSourceType.Method)]
        public void TestParseMapInfoV24(string mapInfoFilePath)
        {
            TestParseMapInfoInternal(mapInfoFilePath);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapInfoDataV26), DynamicDataSourceType.Method)]
        public void TestParseMapInfoV26(string mapInfoFilePath)
        {
            TestParseMapInfoInternal(mapInfoFilePath);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapInfoDataV27), DynamicDataSourceType.Method)]
        public void TestParseMapInfoV27(string mapInfoFilePath)
        {
            TestParseMapInfoInternal(mapInfoFilePath);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapInfoDataGameDataSet), DynamicDataSourceType.Method)]
        public void TestGameDataSet(string mapInfoFilePath, GameDataSet expectedDataSet)
        {
            using var mapInfoStream = File.OpenRead(mapInfoFilePath);
            using var mapInfoReader = new BinaryReader(mapInfoStream);
            var mapInfo = mapInfoReader.ReadMapInfo();

            Assert.AreEqual(expectedDataSet, mapInfo.GameDataSet);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetReforgedMapInfoData), DynamicDataSourceType.Method)]
        public void TestParseReforgedMapInfo(string mapInfoFilePath, bool expectCustomAbilitySkin, bool expectAccurateProbabilityForCalculations, SupportedModes expectSupportedModes, bool expectGameDataVersionTft)
        {
            using var mapInfoStream = File.OpenRead(mapInfoFilePath);
            using var mapInfoReader = new BinaryReader(mapInfoStream);
            var mapInfo = mapInfoReader.ReadMapInfo();

            if (mapInfo.GameDataVersion == GameDataVersion.Unset)
            {
                Assert.AreEqual(GameDataSet.Unset, mapInfo.GameDataSet);
            }
            else
            {
                Assert.AreEqual(GameDataSet.Default, mapInfo.GameDataSet);
                Assert.AreEqual(expectGameDataVersionTft, mapInfo.GameDataVersion == GameDataVersion.TFT);
            }

            Assert.AreEqual(expectCustomAbilitySkin, mapInfo.MapFlags.HasFlag(MapFlags.CustomAbilitySkin));
            Assert.AreEqual(expectAccurateProbabilityForCalculations, mapInfo.MapFlags.HasFlag(MapFlags.AccurateProbabilityForCalculations));
            Assert.AreEqual(expectSupportedModes, mapInfo.SupportedModes);
        }

        private static void TestParseMapInfoInternal(string mapInfoFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(
                mapInfoFilePath,
                typeof(MapInfo));
        }

        private static IEnumerable<object[]> GetMapInfoDataFolder(string versionFolder)
        {
            return TestDataProvider.GetDynamicArchiveData(
                MapInfo.FileName,
                SearchOption.TopDirectoryOnly,
                Path.Combine("Maps", versionFolder));
        }

        private static IEnumerable<object[]> GetMapInfoDataV8() => GetMapInfoDataFolder("8");

        private static IEnumerable<object[]> GetMapInfoDataV10() => GetMapInfoDataFolder("10");

        private static IEnumerable<object[]> GetMapInfoDataV11() => GetMapInfoDataFolder("11");

        private static IEnumerable<object[]> GetMapInfoDataV15() => GetMapInfoDataFolder("15");

        private static IEnumerable<object[]> GetMapInfoDataV23() => GetMapInfoDataFolder("23");

        private static IEnumerable<object[]> GetMapInfoDataV24() => GetMapInfoDataFolder("24");

        private static IEnumerable<object[]> GetMapInfoDataV26() => GetMapInfoDataFolder("26");

        private static IEnumerable<object[]> GetMapInfoDataV27() => GetMapInfoDataFolder("27");

        private static IEnumerable<object[]> GetMapInfoDataGameDataSet()
        {
            yield return new object[] { TestDataProvider.GetPath(@".\Info\GameDataSet\GameDataSetDontCare.w3i"), GameDataSet.Unset };
            yield return new object[] { TestDataProvider.GetPath(@".\Info\GameDataSet\GameDataSetDefault.w3i"), GameDataSet.Default };
            yield return new object[] { TestDataProvider.GetPath(@".\Info\GameDataSet\GameDataSetCustom.w3i"), GameDataSet.Custom };
            yield return new object[] { TestDataProvider.GetPath(@".\Info\GameDataSet\GameDataSetMelee.w3i"), GameDataSet.Melee };
        }

        private static IEnumerable<object[]> GetReforgedMapInfoData()
        {
            yield return new object[] { TestDataProvider.GetPath(@".\Info\Reforged\CustSkinFalse-AccProbFalse-HD-FrozenThrone.w3i"), false, false, SupportedModes.HD, true };
            yield return new object[] { TestDataProvider.GetPath(@".\Info\Reforged\CustSkinFalse-AccProbFalse-HDSD-FrozenThrone.w3i"), false, false, SupportedModes.HD | SupportedModes.SD, true };
            yield return new object[] { TestDataProvider.GetPath(@".\Info\Reforged\CustSkinFalse-AccProbFalse-HDSD-ReignOfChaos.w3i"), false, false, SupportedModes.HD | SupportedModes.SD, false };
            yield return new object[] { TestDataProvider.GetPath(@".\Info\Reforged\CustSkinFalse-AccProbFalse-SD-FrozenThrone.w3i"), false, false, SupportedModes.SD, true };
            yield return new object[] { TestDataProvider.GetPath(@".\Info\Reforged\CustSkinFalse-AccProbTrue-HDSD-FrozenThrone.w3i"), false, true, SupportedModes.HD | SupportedModes.SD, true };
            yield return new object[] { TestDataProvider.GetPath(@".\Info\Reforged\CustSkinTrue-AccProbFalse-HDSD-FrozenThrone.w3i"), true, false, SupportedModes.HD | SupportedModes.SD, true };
        }
    }
}