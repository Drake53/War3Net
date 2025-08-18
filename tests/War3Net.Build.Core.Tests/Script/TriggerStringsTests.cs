// ------------------------------------------------------------------------------
// <copyright file="TriggerStringsTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Extensions;
using War3Net.Build.Script;

namespace War3Net.Build.Core.Tests.Script
{
    [TestClass]
    public class TriggerStringsTests
    {
        [TestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetTriggerStringsFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestParseTriggerStrings(string triggerStringsFilePath)
        {
            ParseTestHelper.RunStreamRWTest(
                triggerStringsFilePath,
                typeof(TriggerStrings),
                nameof(StreamWriterExtensions.WriteTriggerStrings));
        }
    }
}