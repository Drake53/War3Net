// ------------------------------------------------------------------------------
// <copyright file="DoodadObjectDataTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Object;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class DoodadObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetDoodadObjectDataFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestParseDoodadObjectData(string doodadObjectDataFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(
                doodadObjectDataFilePath,
                typeof(DoodadObjectData));
        }
    }
}