// ------------------------------------------------------------------------------
// <copyright file="MapSoundsTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Audio;

namespace War3Net.Build.Core.Tests.Audio
{
    [TestClass]
    public class MapSoundsTests
    {
        [DataTestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetMapSoundsFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestParseMapSounds(string mapSoundsFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(
                mapSoundsFilePath,
                typeof(MapSounds));
        }
    }
}