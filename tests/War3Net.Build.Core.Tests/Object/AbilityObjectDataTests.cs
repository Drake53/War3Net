// ------------------------------------------------------------------------------
// <copyright file="AbilityObjectDataTests.cs" company="Drake53">
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
    public class AbilityObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetAbilityObjectData), DynamicDataSourceType.Method)]
        public void TestParseAbilityObjectData(string abilityObjectDataFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(
                abilityObjectDataFilePath,
                typeof(AbilityObjectData),
                nameof(BinaryReaderExtensions.ReadAbilityObjectData));
        }

        private static IEnumerable<object[]> GetAbilityObjectData()
        {
            return TestDataProvider.GetDynamicData(
                $"*{AbilityObjectData.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Object", "Ability"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                AbilityObjectData.CampaignFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                AbilityObjectData.CampaignSkinFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                AbilityObjectData.MapFileName,
                SearchOption.AllDirectories,
                "Maps"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                AbilityObjectData.MapSkinFileName,
                SearchOption.AllDirectories,
                "Maps"));
        }
    }
}