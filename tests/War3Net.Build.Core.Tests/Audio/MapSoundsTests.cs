// ------------------------------------------------------------------------------
// <copyright file="MapSoundsTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Audio;
using War3Net.Common.Testing;

namespace War3Net.Build.Core.Tests.Audio
{
    [TestClass]
    public class MapSoundsTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetDefaultAudioFiles), DynamicDataSourceType.Method)]
        public void TestParseMapAudio(string mapSoundsFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(mapSoundsFilePath, typeof(MapSounds));
        }

        private static IEnumerable<object[]> GetDefaultAudioFiles()
        {
            return TestDataProvider.GetDynamicData(
                MapSounds.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Audio"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapSounds.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }
    }
}