// ------------------------------------------------------------------------------
// <copyright file="MapBuilderTest.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Environment;
using War3Net.Build.Info;
using War3Net.Build.Script;

namespace War3Net.Build.Tests
{
    [TestClass]
    public class MapBuilderTest
    {
        [TestMethod]
        public void TestCreateNewTemplateMap()
        {
            const string OutputMapName = "Template.w3x";

            var scriptCompilerOptions = new ScriptCompilerOptions(CSharpLua.CoreSystem.CoreSystemProvider.GetCoreSystemFiles());

            scriptCompilerOptions.MapInfo = MapInfo.Default;
            scriptCompilerOptions.MapEnvironment = MapEnvironment.Default;
            scriptCompilerOptions.SourceDirectory = @".\TestData\Script\Template";
            scriptCompilerOptions.OutputDirectory = @".\TestOutput\Template";

#if DEBUG
            scriptCompilerOptions.Debug = true;
#endif

            // Build and launch
            var mapBuilder = new MapBuilder(OutputMapName);
            if (mapBuilder.Build(scriptCompilerOptions))
            {
                var mapPath = Path.Combine(scriptCompilerOptions.OutputDirectory, OutputMapName);
                var absoluteMapPath = new FileInfo(mapPath).FullName;

                const string Warcraft3ExecutableFilePath = null;
                Assert.IsNotNull(Warcraft3ExecutableFilePath, "Path to Warcraft III.exe is not set.");
                Process.Start(Warcraft3ExecutableFilePath, $"-loadfile \"{absoluteMapPath}\"");
            }
        }

        [TestMethod]
        public void TestDefaultMapInfo()
        {
            // Get World Editor default info file.
            using var defaultInfoStream = File.OpenRead(@".\TestData\DefaultMapFiles\war3map.w3i");
            var defaultMapInfo = MapInfo.Parse(defaultInfoStream, true);
            defaultInfoStream.Position = 0;

            // Get War3Net default info file.
            var mapInfo = MapInfo.Default;

            // Update defaults that are different.
            mapInfo.EditorVersion = 6072;
            mapInfo.ScriptLanguage = ScriptLanguage.Jass;

            var player0 = mapInfo.GetPlayerData(0);
            player0.PlayerName = "TRIGSTR_001";
            player0.StartPosition = defaultMapInfo.GetPlayerData(0).StartPosition;
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

        [TestMethod]
        public void TestDefaultMapEnvironment()
        {
            // Get World Editor default environment file.
            using var defaultEnvironmentStream = File.OpenRead(@".\TestData\DefaultMapFiles\war3map.w3e");
            var defaultMapEnvironment = MapEnvironment.Parse(defaultEnvironmentStream, true);
            defaultEnvironmentStream.Position = 0;

            // Get War3Net default environment file.
            var mapEnvironment = MapEnvironment.Default;

            // Update defaults that are different.
            var tileEnumerator = defaultMapEnvironment.GetEnumerator();
            foreach (var tile in mapEnvironment)
            {
                tileEnumerator.MoveNext();
                tile.Variation = tileEnumerator.Current.Variation;
                tile.CliffVariation = tileEnumerator.Current.CliffVariation;
            }

            // Compare files.
            using var mapEnvironmentStream = new MemoryStream();
            mapEnvironment.SerializeTo(mapEnvironmentStream, true);
            mapEnvironmentStream.Position = 0;

            StreamAssert.AreEqual(defaultEnvironmentStream, mapEnvironmentStream);
        }
    }
}