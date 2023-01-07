// ------------------------------------------------------------------------------
// <copyright file="DestructableObjectDataTests.cs" company="Drake53">
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
    public class DestructableObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetDestructableObjectData), DynamicDataSourceType.Method)]
        public void TestParseDestructableObjectData(string destructableObjectDataFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(
                destructableObjectDataFilePath,
                typeof(DestructableObjectData),
                nameof(BinaryReaderExtensions.ReadDestructableObjectData));
        }

        private static IEnumerable<object[]> GetDestructableObjectData()
        {
            return TestDataProvider.GetDynamicData(
                $"*{DestructableObjectData.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Object", "Destructable"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                DestructableObjectData.CampaignFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                DestructableObjectData.CampaignSkinFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                DestructableObjectData.MapFileName,
                SearchOption.AllDirectories,
                "Maps"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                DestructableObjectData.MapSkinFileName,
                SearchOption.AllDirectories,
                "Maps"));
        }
    }
}