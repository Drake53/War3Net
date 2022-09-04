// ------------------------------------------------------------------------------
// <copyright file="GameConfigurationTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Configuration;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests.Configuration
{
    [TestClass]
    public class GameConfigurationTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetGameConfigurationFiles), DynamicDataSourceType.Method)]
        public void TestParseGameConfiguration(string gameConfigurationFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(gameConfigurationFilePath, typeof(GameConfiguration));
        }

        private static IEnumerable<object[]> GetGameConfigurationFiles()
        {
            return TestDataProvider.GetDynamicData(
                "*.wgc",
                SearchOption.AllDirectories,
                Path.Combine("Configuration"));
        }
    }
}