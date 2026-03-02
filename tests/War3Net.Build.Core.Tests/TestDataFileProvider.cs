using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests
{
    public static class TestDataFileProvider
    {
        public static IEnumerable<object[]> GetFilePathsForTestDataType(TestDataFileType testDataFileType)
        {
            var parameters = TestDataFileParams.Get(testDataFileType);

            var result = TestDataProvider.GetDynamicData(
                parameters.SearchPattern,
                SearchOption.AllDirectories,
                parameters.FolderName);

            foreach (var knownFileName in parameters.KnownFileNames)
            {
                var folderName = knownFileName.StartsWith("war3campaign", StringComparison.Ordinal)
                    ? "Campaigns"
                    : "Maps";

                result = result.Concat(TestDataProvider.GetDynamicArchiveData(
                    knownFileName,
                    SearchOption.AllDirectories,
                    folderName));
            }

            return result;
        }
    }
}