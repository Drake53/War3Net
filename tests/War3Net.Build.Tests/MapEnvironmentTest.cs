// ------------------------------------------------------------------------------
// <copyright file="MapEnvironmentTest.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Environment;

namespace War3Net.Build.Tests
{
    [TestClass]
    public class MapEnvironmentTest
    {
        [DataTestMethod]
        [DynamicData(nameof(GetDefaultEnvironmentFiles), DynamicDataSourceType.Method)]
        public void TestDefaultTileset(string environmentFilePath)
        {
            using var fileStream = File.OpenRead(environmentFilePath);
            var environment = MapEnvironment.Parse(fileStream);
            Assert.IsTrue(environment.IsDefaultTileset());
        }

        private static IEnumerable<object[]> GetDefaultEnvironmentFiles()
        {
            foreach (var env in Directory.EnumerateFiles(@".\TestData\Environment\Default"))
            {
                yield return new[] { env };
            }
        }
    }
}