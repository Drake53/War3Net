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
using War3Net.IO.Casc.Enums;
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
                var versionsUrl = "http://eu.patch.battle.net:1119/w3/versions";
                var data = await httpClient.GetByteArrayAsync(versionsUrl);

                Assert.IsNotNull(data);
                Assert.IsTrue(data.Length > 0);

                using var stream = new MemoryStream(data);
                var config = VersionConfig.Parse(stream);

                Assert.IsNotNull(config);
                Assert.IsTrue(config.Entries.Count > 0);

                var euEntry = config.GetEntry("eu");
                Assert.IsNotNull(euEntry);
                Assert.IsFalse(string.IsNullOrEmpty(euEntry.BuildConfig));
                Assert.IsFalse(string.IsNullOrEmpty(euEntry.CdnConfig));
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
                var cdnsUrl = "http://eu.patch.battle.net:1119/w3/cdns";
                var data = await httpClient.GetByteArrayAsync(cdnsUrl);

                Assert.IsNotNull(data);
                Assert.IsTrue(data.Length > 0);

                using var stream = new MemoryStream(data);
                var config = CdnServersConfig.Parse(stream);

                Assert.IsNotNull(config);
                Assert.IsTrue(config.Entries.Count > 0);

                var euEntry = config.GetEntry("eu");
                Assert.IsNotNull(euEntry);
                Assert.IsTrue(euEntry.Hosts.Count > 0);
                Assert.IsFalse(string.IsNullOrEmpty(euEntry.Path));
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
                // First get the versions config to obtain valid build/CDN config hashes
                using var httpClient = new System.Net.Http.HttpClient();
                var versionsUrl = "http://eu.patch.battle.net:1119/w3/versions";
                var versionStream = await httpClient.GetStreamAsync(versionsUrl);
                var versions = VersionConfig.Parse(versionStream);

                var euEntry = versions.GetEntry("eu");
                Assert.IsNotNull(euEntry, "EU version entry should exist");

                // Get the CDN configuration
                var cdnsUrl = "http://eu.patch.battle.net:1119/w3/cdns";
                var cdnStream = await httpClient.GetStreamAsync(cdnsUrl);
                var cdns = CdnServersConfig.Parse(cdnStream);

                var cdnEntry = cdns.GetEntry("eu");
                Assert.IsNotNull(cdnEntry, "EU CDN entry should exist");

                // Create CDN client with actual CDN servers and path
                using var client = new CdnClient(cdnEntry.Hosts, cdnEntry.Path);

                // Test downloading the build config using the hash from versions
                var buildConfigHash = euEntry.BuildConfig;
                Assert.IsFalse(string.IsNullOrEmpty(buildConfigHash), "Build config hash should not be empty");

                // Download the build config file
                // The CdnClient.DownloadConfigAsync constructs the path as: config/{xx}/{yy}/{hash}
                // where xx and yy are the first two pairs of hex digits from the hash
                var buildConfigData = await client.DownloadConfigAsync(Structures.EKey.Parse(buildConfigHash));

                // Verify we actually downloaded something
                Assert.IsNotNull(buildConfigData);
                Assert.IsTrue(buildConfigData.Length > 0, "Downloaded build config should have content");

                // Verify it's a valid config file (should contain configuration data)
                var configText = System.Text.Encoding.UTF8.GetString(buildConfigData);
                Assert.IsTrue(configText.Length > 0, "Config text should not be empty");

                // Also test downloading the CDN config
                var cdnConfigHash = euEntry.CdnConfig;
                Assert.IsFalse(string.IsNullOrEmpty(cdnConfigHash), "CDN config hash should not be empty");

                var cdnConfigData = await client.DownloadConfigAsync(Structures.EKey.Parse(cdnConfigHash));
                Assert.IsNotNull(cdnConfigData);
                Assert.IsTrue(cdnConfigData.Length > 0, "Downloaded CDN config should have content");
            }
            catch (System.Net.Http.HttpRequestException)
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
                var tempPath = Path.Combine(Path.GetTempPath(), "CascTest");

                // Clean up previous test data
                Directory.Delete(tempPath, true);

                var progressReporter = new TestProgressReporter();
                using var storage = await OnlineCascStorage.OpenWar3Async("eu", tempPath, progressReporter);

                Assert.IsNotNull(storage);
                Assert.AreEqual("w3", storage.Product);
                Assert.AreEqual("eu", storage.Region);

                // Verify some files were downloaded
                Assert.IsTrue(Directory.Exists(tempPath));
                Assert.IsTrue(Directory.GetFiles(tempPath, "*", SearchOption.AllDirectories).Length > 0);
            }
            catch (System.Net.Http.HttpRequestException)
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
                var tempPath = Path.Combine(Path.GetTempPath(), "CascTest");

                // Clean up previous test data
                Directory.Delete(tempPath, true);

                using var storage = await OnlineCascStorage.OpenWar3Async("eu", tempPath);

                Assert.IsNotNull(storage, "Storage should be opened successfully");

                // Try to open a known file from the storage
                // Note: War3 uses a binary TVFS root format which we don't fully support yet
                // File name resolution may not work until we implement TVFS parsing
                const string testFileName = "war3.w3mod/ui/triggerdata.txt";

                using var fileStream = storage.OpenFileByName(testFileName);
                Assert.IsNotNull(fileStream, "File stream should not be null");

                // Read some data from the file to verify it opened correctly
                var buffer = new byte[1024];
                var bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length);

                Assert.IsTrue(bytesRead > 0, "Should be able to read data from the file");

                // Optionally verify the content starts with expected text
                // TriggerData files typically start with comments or section headers
                var content = System.Text.Encoding.UTF8.GetString(buffer, 0, Math.Min(bytesRead, 100));
                Console.WriteLine($"First 100 bytes of {testFileName}: {content}");
            }
            catch (System.Net.Http.HttpRequestException)
            {
                Assert.Inconclusive("Test requires internet connection to Blizzard CDN");
            }
        }

        private class TestProgressReporter : IProgressReporter
        {
            public bool ReportProgress(CascProgressMessage message, string? objectName, int current, int total)
            {
                Console.WriteLine($"[{current}/{total}] {message}: {objectName}");
                return true;
            }

            public void ReportStatus(string message)
            {
                Console.WriteLine($"Status: {message}");
            }

            public void ReportError(string message)
            {
                Console.WriteLine($"Error: {message}");
            }

            public bool IsCancelled()
            {
                return false;
            }
        }
    }
}