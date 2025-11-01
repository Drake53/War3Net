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

using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.Build.Providers;
using War3Net.IO.Mpq;

namespace War3Net.Build.Core.Tests.Providers
{
    [TestClass]
    public class GameBuildsProviderTests
    {
        [TestMethod]
        [DynamicData(nameof(GetMappings), DynamicDataSourceType.Method)]
        public void TestVersionAndEditorVersionMatch(Version version, EditorVersion? editorVersion)
        {
            Assert.IsNotNull(editorVersion);

            var gameBuilds = GameBuildsProvider.GetGameBuilds(version);

            Assert.AreNotEqual(0, gameBuilds.Count, $"GameBuilds.json is missing an entry for version '{version}'.");

            foreach (var gameBuild in gameBuilds)
            {
                Assert.AreEqual(editorVersion, gameBuild.EditorVersion, gameBuild.ToString());
            }
        }

        private static IEnumerable<object?[]> GetMappings()
        {
            var mappings = new Dictionary<Version, EditorVersion?>();

            var mapInfoData = TestDataFileProvider.GetFilePathsForTestDataType(TestDataFileType.MapInfo);
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
                        var existingValue = mappings[mapInfo.GameVersion];
                        if (mapInfo.EditorVersion != existingValue)
                        {
                            mappings[mapInfo.GameVersion] = null;
                        }
                    }
                }
            }

            return mappings
#if !ENABLE_FLAKY_TESTS
                .Where(kvp => GameBuildsProvider.GetGameBuilds(kvp.Key).Any())
#endif
                .Select(kvp => new object?[] { kvp.Key, kvp.Value });
        }
    }
}