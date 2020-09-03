// ------------------------------------------------------------------------------
// <copyright file="MapEnvironmentTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Environment;
using War3Net.Common.Testing;
using War3Net.IO.Mpq;

namespace War3Net.Build.Core.Tests.Environment
{
    [TestClass]
    public class MapEnvironmentTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetEnvironmentFiles), DynamicDataSourceType.Method)]
        public void TestParseMapEnvironment(string environmentFilePath)
        {
            using var original = FileProvider.GetFile(environmentFilePath);
            using var recreated = new MemoryStream();

            MapEnvironment.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true);
        }

        private static IEnumerable<object[]> GetEnvironmentFiles()
        {
            return TestDataProvider.GetDynamicData(
                MapEnvironment.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Environment"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapEnvironment.FileName,
                SearchOption.TopDirectoryOnly,
                "Maps"));
        }
    }
}