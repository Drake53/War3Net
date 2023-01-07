// ------------------------------------------------------------------------------
// <copyright file="UpgradeObjectDataTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Object;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class UpgradeObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetUpgradeObjectDataFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestParseUpgradeObjectData(string upgradeObjectDataFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(
                upgradeObjectDataFilePath,
                typeof(UpgradeObjectData));
        }
    }
}