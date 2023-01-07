// ------------------------------------------------------------------------------
// <copyright file="ImportedFilesTests.cs" company="Drake53">
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
    public class ImportedFilesTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetImportedFilesData), DynamicDataSourceType.Method)]
        public void TestParseImportedFiles(string importedFilesFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(
                importedFilesFilePath,
                typeof(ImportedFiles),
                nameof(BinaryReaderExtensions.ReadImportedFiles));
        }

        private static IEnumerable<object[]> GetImportedFilesData()
        {
            return TestDataProvider.GetDynamicData(
                $"*{ImportedFiles.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Import"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                ImportedFiles.CampaignFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                ImportedFiles.MapFileName,
                SearchOption.AllDirectories,
                "Maps"));
        }
    }
}