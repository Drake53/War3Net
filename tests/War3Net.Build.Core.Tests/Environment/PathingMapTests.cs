// ------------------------------------------------------------------------------
// <copyright file="PathingMapTests.cs" company="Drake53">
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
    public class PathingMapTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetPathingMapFiles), DynamicDataSourceType.Method)]
        public void TestParsePathingMap(string pathingMapFile)
        {
            using var original = FileProvider.GetFile(pathingMapFile);
            using var recreated = new MemoryStream();

            PathingMap.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true);
        }

        private static IEnumerable<object[]> GetPathingMapFiles()
        {
            return TestDataProvider.GetDynamicData(
                PathingMap.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Pathing"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                PathingMap.FileName,
                SearchOption.TopDirectoryOnly,
                "Maps"));
        }
    }
}