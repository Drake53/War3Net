// ------------------------------------------------------------------------------
// <copyright file="UnitObjectDataTests.cs" company="Drake53">
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
    public class UnitObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetUnitObjectData), DynamicDataSourceType.Method)]
        public void TestParseUnitObjectData(string unitObjectDataFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(
                unitObjectDataFilePath,
                typeof(UnitObjectData),
                nameof(BinaryReaderExtensions.ReadUnitObjectData));
        }

        private static IEnumerable<object[]> GetUnitObjectData()
        {
            return TestDataProvider.GetDynamicData(
                $"*{UnitObjectData.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Object", "Unit"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                UnitObjectData.CampaignFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                UnitObjectData.CampaignSkinFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                UnitObjectData.MapFileName,
                SearchOption.AllDirectories,
                "Maps"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                UnitObjectData.MapSkinFileName,
                SearchOption.AllDirectories,
                "Maps"));
        }
    }
}