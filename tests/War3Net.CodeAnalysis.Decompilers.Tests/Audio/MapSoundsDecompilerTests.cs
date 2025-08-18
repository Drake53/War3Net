// ------------------------------------------------------------------------------
// <copyright file="MapSoundsDecompilerTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build;
using War3Net.Build.Audio;
using War3Net.Build.Info;
using War3Net.TestTools.UnitTesting;

namespace War3Net.CodeAnalysis.Decompilers.Tests.Audio
{
    [TestClass]
    public class MapSoundsDecompilerTests
    {
        private const MapFiles FilesToOpen = MapFiles.Info | MapFiles.Script | MapFiles.Sounds;

        [TestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestDecompileMapSounds(string mapFilePath)
        {
            var map = Map.Open(mapFilePath, FilesToOpen);

            Assert.IsTrue(new JassScriptDecompiler(map).TryDecompileMapSounds(map.Sounds.FormatVersion, out var decompiledMapSounds), "Failed to decompile map sounds.");

            Assert.AreEqual(map.Sounds.Sounds.Count, decompiledMapSounds.Sounds.Count);
            for (var i = 0; i < decompiledMapSounds.Sounds.Count; i++)
            {
                var expectedSound = map.Sounds.Sounds[i];
                var actualSound = decompiledMapSounds.Sounds[i];

                Assert.AreEqual(expectedSound.Name, actualSound.Name, ignoreCase: false, CultureInfo.InvariantCulture);
                Assert.AreEqual(expectedSound.FilePath, actualSound.FilePath, ignoreCase: false, CultureInfo.InvariantCulture);
                Assert.AreEqual(expectedSound.EaxSetting, actualSound.EaxSetting, ignoreCase: false, CultureInfo.InvariantCulture);
                Assert.AreEqual(expectedSound.Flags, actualSound.Flags);
                Assert.AreEqual(expectedSound.FadeInRate, actualSound.FadeInRate);
                Assert.AreEqual(expectedSound.FadeOutRate, actualSound.FadeOutRate);

                if (map.Sounds.FormatVersion >= MapSoundsFormatVersion.v2)
                {
                    Assert.AreEqual(expectedSound.DialogueTextKey, actualSound.DialogueTextKey);
                    Assert.AreEqual(expectedSound.DialogueSpeakerNameKey, actualSound.DialogueSpeakerNameKey);
                    Assert.AreEqual(expectedSound.FacialAnimationLabel, actualSound.FacialAnimationLabel, ignoreCase: false, CultureInfo.InvariantCulture);
                    Assert.AreEqual(expectedSound.FacialAnimationGroupLabel, actualSound.FacialAnimationGroupLabel, ignoreCase: false, CultureInfo.InvariantCulture);
                    Assert.AreEqual(expectedSound.FacialAnimationSetFilepath, actualSound.FacialAnimationSetFilepath, ignoreCase: false, CultureInfo.InvariantCulture);
                }
            }
        }

        private static IEnumerable<object[]> GetTestData()
        {
            foreach (var data in TestDataProvider.GetDynamicData("*", SearchOption.AllDirectories, "Maps"))
            {
                if (Map.TryOpen((string)data[0], out var map, FilesToOpen) &&
                    map.Info is not null &&
                    map.Sounds is not null &&
                    map.Sounds.Sounds.Count > 0 &&
                    map.Info.ScriptLanguage == ScriptLanguage.Jass &&
                    !string.IsNullOrEmpty(map.Script))
                {
                    yield return data;
                }
            }
        }
    }
}