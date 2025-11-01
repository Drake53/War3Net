// ------------------------------------------------------------------------------
// <copyright file="MapSoundsDecompilerTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build;
using War3Net.Build.Audio;
using War3Net.CodeAnalysis.Jass;

namespace War3Net.CodeAnalysis.Decompilers.Tests.Audio
{
    [TestClass]
    public class MapSoundsDecompilerTests
    {
        private const MapFiles FilesToOpen = MapFiles.Info | MapFiles.Script | MapFiles.Sounds;

        [TestMethod]
        [FlakyDynamicTestData(FilesToOpen, "ExampleMap133.w3x")]
        public void TestDecompileMapSounds(string mapFilePath)
        {
            var map = Map.Open(mapFilePath, FilesToOpen);

            var decompiledMap = new JassScriptDecompiler(JassSyntaxFactory.ParseCompilationUnit(map.Script), new DecompileOptions() { mapSoundsFormatVersion = map.Sounds.FormatVersion }, map.Info).DecompileObjectManagerData();
            var decompiledMapSounds = decompiledMap.Sounds;

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
    }
}