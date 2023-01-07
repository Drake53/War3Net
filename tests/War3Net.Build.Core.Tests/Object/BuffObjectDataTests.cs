// ------------------------------------------------------------------------------
// <copyright file="BuffObjectDataTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Object;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class BuffObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetBuffObjectDataFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestParseBuffObjectData(string buffObjectDataFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(
                buffObjectDataFilePath,
                typeof(BuffObjectData));
        }
    }
}