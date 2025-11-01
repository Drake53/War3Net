// ------------------------------------------------------------------------------
// <copyright file="TriggerDataTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Extensions;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests.Script
{
    [TestClass]
    public class TriggerDataTests
    {
        [TestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestParseTriggerData(string triggerDataPath)
        {
            var triggerDataText = File.ReadAllText(triggerDataPath);
            var reader = new StringReader(triggerDataText);

            reader.ReadTriggerData();
        }

        private static IEnumerable<object[]> GetTestData()
        {
            yield return new object[] { TestDataProvider.GetPath("Script/TriggerData/triggerdata_1_31_1_PTR.txt") };
            yield return new object[] { TestDataProvider.GetPath("Script/TriggerData/triggerdata_1_32_9.txt") };
        }
    }
}