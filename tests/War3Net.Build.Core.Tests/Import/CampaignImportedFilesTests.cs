// ------------------------------------------------------------------------------
// <copyright file="CampaignImportedFilesTests.cs" company="Drake53">
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
using War3Net.Common.Testing;

namespace War3Net.Build.Core.Tests.Import
{
    [TestClass]
    public class CampaignImportedFilesTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetCampaignImportedFilesData), DynamicDataSourceType.Method)]
        public void TestParseCampaignImportedFiles(string campaignImportedFilesFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(
                campaignImportedFilesFilePath,
                typeof(CampaignImportedFiles),
                nameof(BinaryReaderExtensions.ReadImportedFiles),
                true);
        }

        private static IEnumerable<object[]> GetCampaignImportedFilesData()
        {
            return TestDataProvider.GetDynamicData(
                CampaignImportedFiles.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Import"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                CampaignImportedFiles.FileName,
                SearchOption.AllDirectories,
                "Campaigns"));
        }
    }
}