// ------------------------------------------------------------------------------
// <copyright file="BuffObjectDataTests.cs" company="Drake53">
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
    public class BuffObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetBuffObjectData), DynamicDataSourceType.Method)]
        public void TestParseBuffObjectData(string buffObjectDataFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(
                buffObjectDataFilePath,
                typeof(BuffObjectData),
                nameof(BinaryReaderExtensions.ReadBuffObjectData));
        }

        private static IEnumerable<object[]> GetBuffObjectData()
        {
            return TestDataProvider.GetDynamicData(
                $"*{BuffObjectData.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Object", "Buff"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                BuffObjectData.CampaignFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                BuffObjectData.CampaignSkinFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                BuffObjectData.MapFileName,
                SearchOption.AllDirectories,
                "Maps"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                BuffObjectData.MapSkinFileName,
                SearchOption.AllDirectories,
                "Maps"));
        }
    }
}