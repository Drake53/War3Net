// ------------------------------------------------------------------------------
// <copyright file="CdnConfigTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.IO.Casc.Cdn;
using War3Net.TestTools.UnitTesting;

namespace War3Net.IO.Casc.Tests
{
    [TestClass]
    public class CdnConfigTests
    {
        [TestMethod]
        public void TestParseCdnConfig()
        {
            // Arrange
            var testDataPath = TestDataProvider.GetPath("Casc/CdnConfig.ini");

            // Act
            CdnConfig config;
            using (var stream = File.OpenRead(testDataPath))
            {
                config = CdnConfig.Parse(stream);
            }

            // Assert - Test basic properties
            Assert.IsNotNull(config);

            // Test archives list
            var archives = config.Archives;
            Assert.IsNotNull(archives);
            Assert.AreEqual(142, archives.Count, "Should have 142 archives based on the test data");

            // Test first few archive hashes
            Assert.AreEqual("0335958aca9f4a22a2dc5a2e99117a57", archives[0]);
            Assert.AreEqual("08a698412385246273a7acbb30a1874b", archives[1]);
            Assert.AreEqual("08fa8495eb9b1ae30a4a3fb84dc550b2", archives[2]);

            // Test last archive hash
            Assert.AreEqual("ffade4a94362a96be38e5da159c053a5", archives[archives.Count - 1]);

            // Note: CdnConfig doesn't expose archive index sizes as a property
            // We would need to parse the raw data to get these values

            // Test archive index sizes
            var archiveIndexSizes = config.ArchiveIndexSizes;
            Assert.IsNotNull(archiveIndexSizes);
            Assert.AreEqual(142, archiveIndexSizes.Length, "Should have 142 archive index sizes");
            Assert.AreEqual(127748, archiveIndexSizes[0]);
            Assert.AreEqual(144228, archiveIndexSizes[1]);

            // Test archive group
            var archiveGroup = config.ArchiveGroup;
            Assert.IsFalse(string.IsNullOrEmpty(archiveGroup));
            Assert.AreEqual("def07feb3aa97dbc3bf17544b464196c", archiveGroup);

            // Test patch archives
            var patchArchives = config.PatchArchives;
            Assert.IsNotNull(patchArchives);
            Assert.AreEqual(1, patchArchives.Count, "Should have 1 patch archive");
            Assert.AreEqual("b8abfc0fa101b10f5f591aa440123426", patchArchives[0]);

            // Test patch archive index sizes
            var patchArchiveIndexSizes = config.PatchArchiveIndexSizes;
            Assert.IsNotNull(patchArchiveIndexSizes);
            Assert.AreEqual(1, patchArchiveIndexSizes.Length, "Should have 1 patch archive index size");
            Assert.AreEqual(313148, patchArchiveIndexSizes[0]);

            // Test patch archive group
            var patchArchiveGroup = config.PatchArchiveGroup;
            Assert.IsFalse(string.IsNullOrEmpty(patchArchiveGroup));
            Assert.AreEqual("32207983d368c53aa0dd667459c90f30", patchArchiveGroup);

            // Test file index
            var fileIndex = config.FileIndex;
            Assert.IsFalse(string.IsNullOrEmpty(fileIndex));
            Assert.AreEqual("ffa8a61a063080cd99e925856dc7aa8b", fileIndex);

            // Test file index size
            var fileIndexSize = config.FileIndexSize;
            Assert.AreEqual(173068, fileIndexSize);

            // Test patch file index
            var patchFileIndex = config.PatchFileIndex;
            Assert.IsFalse(string.IsNullOrEmpty(patchFileIndex));
            Assert.AreEqual("63275e1270fcdf4346cbef10c3770d70", patchFileIndex);

            // Test patch file index size
            var patchFileIndexSize = config.PatchFileIndexSize;
            Assert.AreEqual(16508, patchFileIndexSize);
        }

        [TestMethod]
        public void TestCdnConfigTryGetValue()
        {
            // Arrange
            var testDataPath = TestDataProvider.GetPath("Casc/CdnConfig.ini");

            // Act
            CdnConfig config;
            using (var stream = File.OpenRead(testDataPath))
            {
                config = CdnConfig.Parse(stream);
            }

            // Assert - Test TryGetValue method
            Assert.IsTrue(config.TryGetValue("archive-group", out var archiveGroupValue));
            Assert.AreEqual("def07feb3aa97dbc3bf17544b464196c", archiveGroupValue);

            Assert.IsTrue(config.TryGetValue("file-index", out var fileIndexValue));
            Assert.AreEqual("ffa8a61a063080cd99e925856dc7aa8b", fileIndexValue);

            Assert.IsTrue(config.TryGetValue("archives", out var archivesValue));
            Assert.IsNotNull(archivesValue);
            Assert.IsTrue(archivesValue.StartsWith("0335958aca9f4a22a2dc5a2e99117a57", StringComparison.OrdinalIgnoreCase));

            Assert.IsFalse(config.TryGetValue("non-existent-key", out _));
        }

        [TestMethod]
        public void TestCdnConfigAllKeys()
        {
            // Arrange
            var testDataPath = TestDataProvider.GetPath("Casc/CdnConfig.ini");

            // Act
            CdnConfig config;
            using (var stream = File.OpenRead(testDataPath))
            {
                config = CdnConfig.Parse(stream);
            }

            // Assert - Test that we can access various properties
            Assert.IsNotNull(config.ArchiveGroup);
            Assert.IsNotNull(config.Archives);
            Assert.IsTrue(config.Archives.Count > 0);
            Assert.IsNotNull(config.FileIndex);
            Assert.IsTrue(config.FileIndexSize > 0);
            Assert.IsNotNull(config.PatchFileIndex);
            Assert.IsTrue(config.PatchFileIndexSize > 0);
            Assert.IsNotNull(config.PatchArchives);
            Assert.IsNotNull(config.PatchArchiveGroup);
            Assert.IsTrue(config.ContainsKey("archives"));
            Assert.IsTrue(config.ContainsKey("file-index"));
            Assert.IsFalse(config.ContainsKey("non-existent-key"));
        }

        [TestMethod]
        public void TestCdnConfigArchiveValidation()
        {
            // Arrange
            var testDataPath = TestDataProvider.GetPath("Casc/CdnConfig.ini");

            // Act
            CdnConfig config;
            using (var stream = File.OpenRead(testDataPath))
            {
                config = CdnConfig.Parse(stream);
            }

            // Assert - Validate archives
            var archives = config.Archives;

            // Validate all archive hashes are 32 characters (MD5 hash in hex)
            foreach (var archive in archives)
            {
                Assert.AreEqual(32, archive.Length, $"Archive hash '{archive}' should be 32 characters (MD5)");
                Assert.IsTrue(IsValidHexString(archive), $"Archive hash '{archive}' should be valid hex");
            }

            // Validate archive group is valid hash
            var archiveGroup = config.ArchiveGroup;
            Assert.IsNotNull(archiveGroup);
            Assert.AreEqual(32, archiveGroup.Length, "Archive group should be 32 characters (MD5)");
            Assert.IsTrue(IsValidHexString(archiveGroup), "Archive group should be valid hex");

            // Validate file index is valid hash
            var fileIndex = config.FileIndex;
            Assert.IsNotNull(fileIndex);
            Assert.AreEqual(32, fileIndex.Length, "File index should be 32 characters (MD5)");
            Assert.IsTrue(IsValidHexString(fileIndex), "File index should be valid hex");
        }

        [TestMethod]
        public void TestCdnConfigSizeValues()
        {
            // Arrange
            var testDataPath = TestDataProvider.GetPath("Casc/CdnConfig.ini");

            // Act
            CdnConfig config;
            using (var stream = File.OpenRead(testDataPath))
            {
                config = CdnConfig.Parse(stream);
            }

            // Assert - Validate size values
            Assert.IsTrue(config.FileIndexSize > 0, "File index size should be positive");
            Assert.IsTrue(config.PatchFileIndexSize > 0, "Patch file index size should be positive");

            // Validate that file index size matches expected value from test data
            Assert.AreEqual(173068, config.FileIndexSize, "File index size should match test data");
            Assert.AreEqual(16508, config.PatchFileIndexSize, "Patch file index size should match test data");
        }

        private static bool IsValidHexString(string hex)
        {
            return hex.All(c => (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F'));
        }
    }
}