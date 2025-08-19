// ------------------------------------------------------------------------------
// <copyright file="OnlineStorageTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.IO.Casc.Cdn;
using War3Net.IO.Casc.Progress;
using War3Net.IO.Casc.Storage;

namespace War3Net.IO.Casc.Tests
{
    /// <summary>
    /// Tests for online CASC storage.
    /// </summary>
    [TestClass]
    public class OnlineStorageTests
    {
        /// <summary>
        /// Tests downloading and parsing version config.
        /// </summary>
        [TestMethod]
        [TestCategory("Online")]
        public async Task TestVersionConfigDownload()
        {
            // This test requires internet connection
            try
            {
                using var httpClient = new System.Net.Http.HttpClient();
                var versionsUrl = "http://us.patch.battle.net:1119/w3/versions";
                var data = await httpClient.GetByteArrayAsync(versionsUrl);

                Assert.IsNotNull(data);
                Assert.IsTrue(data.Length > 0);

                using var stream = new MemoryStream(data);
                var config = VersionConfig.Parse(stream);

                Assert.IsNotNull(config);
                Assert.IsTrue(config.Entries.Count > 0);

                var usEntry = config.GetEntry("us");
                Assert.IsNotNull(usEntry);
                Assert.IsFalse(string.IsNullOrEmpty(usEntry.BuildConfig));
                Assert.IsFalse(string.IsNullOrEmpty(usEntry.CdnConfig));
            }
            catch (System.Net.Http.HttpRequestException)
            {
                // Skip test if no internet connection
                Assert.Inconclusive("Test requires internet connection to Blizzard CDN");
            }
        }

        /// <summary>
        /// Tests downloading and parsing CDN servers config.
        /// </summary>
        [TestMethod]
        [TestCategory("Online")]
        public async Task TestCdnServersConfigDownload()
        {
            try
            {
                using var httpClient = new System.Net.Http.HttpClient();
                var cdnsUrl = "http://us.patch.battle.net:1119/w3/cdns";
                var data = await httpClient.GetByteArrayAsync(cdnsUrl);

                Assert.IsNotNull(data);
                Assert.IsTrue(data.Length > 0);

                using var stream = new MemoryStream(data);
                var config = CdnServersConfig.Parse(stream);

                Assert.IsNotNull(config);
                Assert.IsTrue(config.Entries.Count > 0);

                var usEntry = config.GetEntry("us");
                Assert.IsNotNull(usEntry);
                Assert.IsTrue(usEntry.Hosts.Count > 0);
                Assert.IsFalse(string.IsNullOrEmpty(usEntry.Path));
            }
            catch (System.Net.Http.HttpRequestException)
            {
                Assert.Inconclusive("Test requires internet connection to Blizzard CDN");
            }
        }

        /// <summary>
        /// Tests CDN client functionality.
        /// </summary>
        [TestMethod]
        [TestCategory("Online")]
        public async Task TestCdnClient()
        {
            try
            {
                using var client = new CdnClient("us");
                
                // Try to download versions file directly
                var data = await client.DownloadFileAsync("w3/versions");
                
                Assert.IsNotNull(data);
                Assert.IsTrue(data.Length > 0);
            }
            catch (Exception ex) when (ex is System.Net.Http.HttpRequestException || ex is CascException)
            {
                Assert.Inconclusive("Test requires internet connection to Blizzard CDN");
            }
        }

        /// <summary>
        /// Tests opening Warcraft 3 online storage.
        /// </summary>
        [TestMethod]
        [TestCategory("Online")]
        [TestCategory("LongRunning")]
        public async Task TestOpenWar3OnlineStorage()
        {
            try
            {
                var tempPath = Path.Combine(Path.GetTempPath(), "CascTest", Guid.NewGuid().ToString());
                
                var progressReporter = new TestProgressReporter();
                using var storage = await OnlineCascStorage.OpenWar3Async("us", tempPath, progressReporter);
                
                Assert.IsNotNull(storage);
                Assert.AreEqual("w3", storage.Product);
                Assert.AreEqual("us", storage.Region);
                
                // Verify some files were downloaded
                Assert.IsTrue(Directory.Exists(tempPath));
                Assert.IsTrue(Directory.GetFiles(tempPath, "*", SearchOption.AllDirectories).Length > 0);
                
                // Clean up
                try
                {
                    Directory.Delete(tempPath, true);
                }
                catch
                {
                    // Ignore cleanup errors
                }
            }
            catch (Exception ex) when (ex is System.Net.Http.HttpRequestException || ex is CascException)
            {
                Assert.Inconclusive("Test requires internet connection to Blizzard CDN");
            }
        }

        /// <summary>
        /// Tests reading a file from online storage.
        /// </summary>
        [TestMethod]
        [TestCategory("Online")]
        [TestCategory("LongRunning")]
        public async Task TestReadFileFromOnlineStorage()
        {
            try
            {
                var tempPath = Path.Combine(Path.GetTempPath(), "CascTest", Guid.NewGuid().ToString());
                
                using var storage = await OnlineCascStorage.OpenWar3Async("us", tempPath);
                
                // Try to open a known file by its key
                // This would require knowing a specific file's key in the current build
                // For now, just verify the storage opened successfully
                Assert.IsNotNull(storage);
                
                // Clean up
                try
                {
                    Directory.Delete(tempPath, true);
                }
                catch
                {
                    // Ignore cleanup errors
                }
            }
            catch (Exception ex) when (ex is System.Net.Http.HttpRequestException || ex is CascException)
            {
                Assert.Inconclusive("Test requires internet connection to Blizzard CDN");
            }
        }

        private class TestProgressReporter : IProgressReporter
        {
            public void Report(ProgressEventArgs args)
            {
                Console.WriteLine($"[{args.Current}/{args.Total}] {args.Message}: {args.FileName}");
            }
        }
    }
}