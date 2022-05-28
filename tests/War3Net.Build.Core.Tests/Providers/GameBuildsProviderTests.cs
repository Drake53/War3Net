// ------------------------------------------------------------------------------
// <copyright file="GameBuildsProviderTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Common;
using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.Build.Providers;
using War3Net.IO.Mpq;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests.Providers
{
    [TestClass]
    public class GameBuildsProviderTests
    {
        [TestMethod]
        public void TestVersionAndEditorVersionMatch()
        {
            var mappings = new Dictionary<Version, EditorVersion>();

            var mapInfoData = GetMapInfoData();
            foreach (var data in mapInfoData)
            {
                var mapInfoPath = (string)data[0];

                using var fileStream = MpqFile.OpenRead(mapInfoPath);
                using var reader = new BinaryReader(fileStream);
                var mapInfo = reader.ReadMapInfo();

                if (mapInfo.GameVersion is not null)
                {
                    if (!mappings.TryAdd(mapInfo.GameVersion, mapInfo.EditorVersion))
                    {
                        Assert.AreEqual(mappings[mapInfo.GameVersion], mapInfo.EditorVersion);
                    }
                }
            }

            var errors = new List<string>();

            var gameBuilds = GameBuildsProvider.GetGameBuilds(GameExpansion.Reforged);
            foreach (var gameBuild in gameBuilds)
            {
                if (mappings.TryGetValue(gameBuild.Version, out var expected))
                {
                    Assert.AreEqual(expected, gameBuild.EditorVersion, gameBuild.ToString());
                }
                else if (!gameBuild.EditorVersion.HasValue)
                {
                    errors.Add($"\r\nNo .w3i found for v{gameBuild.Version} to get the expected editor version. ({gameBuild.ReleaseDate:dd MMM yyyy})");
                }
            }

            if (errors.Any())
            {
                Assert.Fail(string.Concat(errors));
            }
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