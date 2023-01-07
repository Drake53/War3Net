// ------------------------------------------------------------------------------
// <copyright file="MapUnitsTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Widget;

namespace War3Net.Build.Core.Tests.Widget
{
    [TestClass]
    public class MapUnitsTests
    {
        [DataTestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetMapUnitsFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestParseMapUnits(string mapUnitsFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(
                mapUnitsFilePath,
                typeof(MapUnits));
        }
    }
}