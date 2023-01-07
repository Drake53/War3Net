// ------------------------------------------------------------------------------
// <copyright file="ImportedFilesTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Import;

namespace War3Net.Build.Core.Tests.Import
{
    [TestClass]
    public class ImportedFilesTests
    {
        [DataTestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetImportedFilesFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestParseImportedFiles(string importedFilesFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(
                importedFilesFilePath,
                typeof(ImportedFiles));
        }
    }
}