// ------------------------------------------------------------------------------
// <copyright file="CampaignInfoTest.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Info;
using War3Net.Build.Providers;
using War3Net.Common.Testing;

namespace War3Net.Build.Tests
{
    [TestClass]
    public class CampaignInfoTest
    {
        [DataTestMethod]
        [DynamicData(nameof(GetCampaignInfoData), DynamicDataSourceType.Method)]
        public void TestParseCampaignInfo(string campaignInfoFilePath)
        {
            using var original = FileProvider.GetFile(campaignInfoFilePath);
            using var recreated = new MemoryStream();

            CampaignInfo.Parse(original, true).SerializeTo(recreated, true);
            Assert.AreEqual(original.Length, original.Position);
            StreamAssert.AreEqual(original, recreated, true);
        }

        private static IEnumerable<object[]> GetCampaignInfoData()
        {
            if (Directory.Exists(@".\TestData\Info\Campaign"))
            {
                foreach (var file in Directory.EnumerateFiles(@".\TestData\Info\Campaign", "*.w3f", SearchOption.AllDirectories))
                {
                    yield return new[] { file };
                }
            }

            if (Directory.Exists(@".\TestData\Local\Info\Campaign"))
            {
                foreach (var file in Directory.EnumerateFiles(@".\TestData\Local\Info\Campaign", "*.w3f", SearchOption.AllDirectories))
                {
                    yield return new[] { file };
                }
            }

            if (Directory.Exists(@".\TestData\Campaigns"))
            {
                foreach (var campaignPath in Directory.EnumerateFiles(@".\TestData\Campaigns", "*.w3n", SearchOption.TopDirectoryOnly))
                {
                    var file = Path.Combine(campaignPath, CampaignInfo.FileName);
                    if (FileProvider.FileExists(file))
                    {
                        yield return new[] { file };
                    }
                }
            }

            if (Directory.Exists(@".\TestData\Local\Campaigns"))
            {
                foreach (var campaignPath in Directory.EnumerateFiles(@".\TestData\Local\Campaigns", "*.w3n", SearchOption.TopDirectoryOnly))
                {
                    var file = Path.Combine(campaignPath, CampaignInfo.FileName);
                    if (FileProvider.FileExists(file))
                    {
                        yield return new[] { file };
                    }
                }
            }
        }
    }
}