// ------------------------------------------------------------------------------
// <copyright file="DoodadObjectDataTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Extensions;
using War3Net.Build.Object;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class DoodadObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetDoodadObjectData), DynamicDataSourceType.Method)]
        public void TestParseDoodadObjectData(string doodadObjectDataFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(
                doodadObjectDataFilePath,
                typeof(DoodadObjectData),
                nameof(BinaryReaderExtensions.ReadDoodadObjectData));
        }

        private static IEnumerable<object[]> GetDoodadObjectData()
        {
            return TestDataProvider.GetDynamicData(
                $"*{DoodadObjectData.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Object", "Doodad"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                DoodadObjectData.CampaignFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                DoodadObjectData.CampaignSkinFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                DoodadObjectData.MapFileName,
                SearchOption.AllDirectories,
                "Maps"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                DoodadObjectData.MapSkinFileName,
                SearchOption.AllDirectories,
                "Maps"));
        }
    }
}