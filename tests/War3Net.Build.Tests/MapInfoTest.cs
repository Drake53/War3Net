// ------------------------------------------------------------------------------
// <copyright file="MapInfoTest.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Info;
using War3Net.Common.Testing;

namespace War3Net.Build.Tests
{
    [TestClass]
    public class MapInfoTest
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapInfoData), DynamicDataSourceType.Method)]
        public void TestParseMapInfo(string mapInfoFilePath)
        {
            using var recreated = new MemoryStream();
            using var original = File.OpenRead(mapInfoFilePath);

            MapInfo.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapInfoDataGameDataSet), DynamicDataSourceType.Method)]
        public void TestGameDataSet(string mapInfoFilePath, GameDataSet expectedDataSet)
        {
            using var mapInfoStream = File.OpenRead(mapInfoFilePath);
            var mapInfo = MapInfo.Parse(mapInfoStream);

            Assert.AreEqual(expectedDataSet, mapInfo.GameDataSet);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetReforgedMapInfoData), DynamicDataSourceType.Method)]
        public void TestParseReforgedMapInfo(string mapInfoFilePath, bool expectCustomAbilitySkin, bool expectAccurateProbabilityForCalculations, SupportedModes expectSupportedModes, bool expectGameDataVersionTft)
        {
            using var mapInfoStream = File.OpenRead(mapInfoFilePath);
            var mapInfo = MapInfo.Parse(mapInfoStream);

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

        private static IEnumerable<object[]> GetMapInfoData()
        {
            foreach (var info in Directory.EnumerateFiles(@".\TestData\Info", "*.w3i", SearchOption.AllDirectories))
            {
                yield return new[] { info };
            }
        }

        private static IEnumerable<object[]> GetMapInfoDataGameDataSet()
        {
            yield return new object[] { @".\TestData\Info\GameDataSet\GameDataSetDontCare.w3i", GameDataSet.Unset };
            yield return new object[] { @".\TestData\Info\GameDataSet\GameDataSetDefault.w3i", GameDataSet.Default };
            yield return new object[] { @".\TestData\Info\GameDataSet\GameDataSetCustom.w3i", GameDataSet.Custom };
            yield return new object[] { @".\TestData\Info\GameDataSet\GameDataSetMelee.w3i", GameDataSet.Melee };
        }

        private static IEnumerable<object[]> GetReforgedMapInfoData()
        {
            yield return new object[] { @".\TestData\Info\Reforged\CustSkinFalse-AccProbFalse-HD-FrozenThrone.w3i", false, false, SupportedModes.HD, true };
            yield return new object[] { @".\TestData\Info\Reforged\CustSkinFalse-AccProbFalse-HDSD-FrozenThrone.w3i", false, false, SupportedModes.HD | SupportedModes.SD, true };
            yield return new object[] { @".\TestData\Info\Reforged\CustSkinFalse-AccProbFalse-HDSD-ReignOfChaos.w3i", false, false, SupportedModes.HD | SupportedModes.SD, false };
            yield return new object[] { @".\TestData\Info\Reforged\CustSkinFalse-AccProbFalse-SD-FrozenThrone.w3i", false, false, SupportedModes.SD, true };
            yield return new object[] { @".\TestData\Info\Reforged\CustSkinFalse-AccProbTrue-HDSD-FrozenThrone.w3i", false, true, SupportedModes.HD | SupportedModes.SD, true };
            yield return new object[] { @".\TestData\Info\Reforged\CustSkinTrue-AccProbFalse-HDSD-FrozenThrone.w3i", true, false, SupportedModes.HD | SupportedModes.SD, true };
        }
    }
}