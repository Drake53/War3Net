// ------------------------------------------------------------------------------
// <copyright file="SylkParserTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.TestTools.UnitTesting;

namespace War3Net.IO.Slk.Tests
{
    [TestClass]
    public class SylkParserTests
    {
        [TestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestParseSlkFile(string filePath)
        {
            using var stream = File.OpenRead(filePath);

            new SylkParser().Parse(stream);
        }

        private static IEnumerable<object[]> GetTestData()
        {
            return TestDataProvider.GetDynamicData("*.slk", SearchOption.AllDirectories, "Slk");
        }
    }
}