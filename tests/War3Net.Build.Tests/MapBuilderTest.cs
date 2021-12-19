// ------------------------------------------------------------------------------
// <copyright file="MapBuilderTest.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Common;
using War3Net.Build.Environment;
using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.Common.Testing;

namespace War3Net.Build.Tests
{
    [TestClass]
    public class MapBuilderTest
    {
        private const string Warcraft3ExecutableFilePath = null;

        [TestMethod]
        public void TestGenerateJassScriptWithUnitData()
        {
            const string OutputMapName = "TestOutput.w3x";
            const string InputPath = @".\TestData\MapFiles\TestGenerateUnitData";

            var scriptCompilerOptions = new ScriptCompilerOptions();
            scriptCompilerOptions.ForceCompile = true;
            scriptCompilerOptions.SourceDirectory = null;
            scriptCompilerOptions.OutputDirectory = @".\TestOutput\TestGenerateUnitData";

            var mapBuilder = new LegacyMapBuilder(OutputMapName);
            if (mapBuilder.Build(scriptCompilerOptions, InputPath))
            {
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestGenerateLuaScriptWithUnitData()
        {
            const string OutputMapName = "TestOutput.w3x";
            const string InputPath = @".\TestData\MapFiles\TestGenerateUnitData";

            using var mapInfoStream = File.OpenRead(Path.Combine(InputPath, MapInfo.FileName));
            using var mapInfoReader = new BinaryReader(mapInfoStream);
            var mapInfo = mapInfoReader.ReadMapInfo();
            mapInfo.ScriptLanguage = ScriptLanguage.Lua;

            var scriptCompilerOptions = new ScriptCompilerOptions();
            scriptCompilerOptions.MapInfo = mapInfo;
            scriptCompilerOptions.ForceCompile = true;
            scriptCompilerOptions.SourceDirectory = null;
            scriptCompilerOptions.OutputDirectory = @".\TestOutput\TestGenerateUnitData";

            var mapBuilder = new LegacyMapBuilder(OutputMapName);
            if (mapBuilder.Build(scriptCompilerOptions, InputPath))
            {
                var mapPath = Path.Combine(scriptCompilerOptions.OutputDirectory, OutputMapName);
                var absoluteMapPath = new FileInfo(mapPath).FullName;

                Assert.IsNotNull(Warcraft3ExecutableFilePath, "Path to Warcraft III.exe is not set.");
                Process.Start(Warcraft3ExecutableFilePath, $"-loadfile \"{absoluteMapPath}\"");
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestCreateNewTemplateMap()
        {
            const string OutputMapName = "Template.w3x";

            var scriptCompilerOptions = new ScriptCompilerOptions(CSharpLua.CoreSystem.CoreSystemProvider.GetCoreSystemFiles());

            // scriptCompilerOptions.MapInfo = MapInfo.Default;
            // scriptCompilerOptions.MapEnvironment = MapEnvironment.Default;
            throw new NotImplementedException();

            scriptCompilerOptions.SourceDirectory = @".\TestData\Script\Template";
            scriptCompilerOptions.OutputDirectory = @".\TestOutput\Template";

#if DEBUG
            scriptCompilerOptions.Debug = true;
#endif

            // Build and launch
            var mapBuilder = new LegacyMapBuilder(OutputMapName);
            if (mapBuilder.Build(scriptCompilerOptions))
            {
                var mapPath = Path.Combine(scriptCompilerOptions.OutputDirectory, OutputMapName);
                var absoluteMapPath = new FileInfo(mapPath).FullName;

                Assert.IsNotNull(Warcraft3ExecutableFilePath, "Path to Warcraft III.exe is not set.");
                Process.Start(Warcraft3ExecutableFilePath, $"-loadfile \"{absoluteMapPath}\"");
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestDefaultMapInfo()
        {
            // Get World Editor default info file.
            using var defaultInfoStream = File.OpenRead(@".\TestData\MapFiles\DefaultMapFiles\war3map.w3i");
            using var defaultInfoReader = new BinaryReader(defaultInfoStream);
            var defaultMapInfo = defaultInfoReader.ReadMapInfo();
            defaultInfoStream.Position = 0;

            // Get War3Net default info file.
            MapInfo mapInfo; // = MapInfo.Default;
            throw new NotImplementedException();

            // Update defaults that are different.
            mapInfo.EditorVersion = 6072;
            mapInfo.ScriptLanguage = ScriptLanguage.Jass;

            var player0 = mapInfo.Players[0];
            player0.Name = "TRIGSTR_001";
            player0.StartPosition = defaultMapInfo.Players[0].StartPosition;
            mapInfo.Players.Clear();
            mapInfo.Players.Add(player0);

            var team0 = mapInfo.Forces[0];
            team0.Name = "TRIGSTR_002";
            team0.Players = new Bitmask32();
            mapInfo.Forces.Clear();
            mapInfo.Forces.Add(team0);

            // Compare files.
            using var mapInfoStream = new MemoryStream();
            using var mapInfoWriter = new BinaryWriter(mapInfoStream);
            mapInfoWriter.Write(mapInfo);

            StreamAssert.AreEqual(defaultInfoStream, mapInfoStream, true);
        }

        [TestMethod]
        public void TestDefaultMapEnvironment()
        {
            // Get World Editor default environment file.
            using var defaultEnvironmentStream = File.OpenRead(@".\TestData\MapFiles\DefaultMapFiles\war3map.w3e");
            using var defaultEnvironmentReader = new BinaryReader(defaultEnvironmentStream);
            var defaultMapEnvironment = defaultEnvironmentReader.ReadMapEnvironment();
            defaultEnvironmentStream.Position = 0;

            // Get War3Net default environment file.
            MapEnvironment mapEnvironment; // = MapEnvironment.Default;
            throw new NotImplementedException();

            // Update defaults that are different.
            var tileEnumerator = defaultMapEnvironment.TerrainTiles.GetEnumerator();
            foreach (var tile in mapEnvironment.TerrainTiles)
            {
                tileEnumerator.MoveNext();
                tile.Variation = tileEnumerator.Current.Variation;
                tile.CliffVariation = tileEnumerator.Current.CliffVariation;
            }

            // Compare files.
            using var mapEnvironmentStream = new MemoryStream();
            using var mapEnvironmentWriter = new BinaryWriter(mapEnvironmentStream);
            mapEnvironmentWriter.Write(mapEnvironment);

            StreamAssert.AreEqual(defaultEnvironmentStream, mapEnvironmentStream, true);
        }
    }
}