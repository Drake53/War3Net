// ------------------------------------------------------------------------------
// <copyright file="ItemObjectDataTests.cs" company="Drake53">
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
    public class ItemObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetItemObjectData), DynamicDataSourceType.Method)]
        public void TestParseItemObjectData(string itemObjectDataFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(
                itemObjectDataFilePath,
                typeof(ItemObjectData),
                nameof(BinaryReaderExtensions.ReadItemObjectData));
        }

        private static IEnumerable<object[]> GetItemObjectData()
        {
            return TestDataProvider.GetDynamicData(
                $"*{ItemObjectData.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Object", "Item"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                ItemObjectData.CampaignFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                ItemObjectData.CampaignSkinFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                ItemObjectData.MapFileName,
                SearchOption.AllDirectories,
                "Maps"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                ItemObjectData.MapSkinFileName,
                SearchOption.AllDirectories,
                "Maps"));
        }
    }
}