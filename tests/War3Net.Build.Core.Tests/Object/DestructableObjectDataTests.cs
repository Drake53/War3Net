// ------------------------------------------------------------------------------
// <copyright file="DestructableObjectDataTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Object;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class DestructableObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetDestructableObjectDataFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestParseDestructableObjectData(string destructableObjectDataFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(
                destructableObjectDataFilePath,
                typeof(DestructableObjectData));
        }
    }
}