// ------------------------------------------------------------------------------
// <copyright file="MapImportedFilesTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Extensions;
using War3Net.Build.Import;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests.Import
{
    [TestClass]
    public class MapImportedFilesTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapImportedFilesData), DynamicDataSourceType.Method)]
        public void TestParseMapImportedFiles(string mapImportedFilesFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(
                mapImportedFilesFilePath,
                typeof(MapImportedFiles),
                nameof(BinaryReaderExtensions.ReadImportedFiles),
                false);
        }

        private static IEnumerable<object[]> GetMapImportedFilesData()
        {
            return TestDataProvider.GetDynamicData(
                MapImportedFiles.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Import"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapImportedFiles.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }
    }
}