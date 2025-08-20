// ------------------------------------------------------------------------------
// <copyright file="PatchConfigTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.IO.Casc.Cdn;
using War3Net.IO.Casc.Enums;
using War3Net.TestTools.UnitTesting;

namespace War3Net.IO.Casc.Tests
{
    [TestClass]
    public class PatchConfigTests
    {
        [TestMethod]
        public void TestParsePatchConfig()
        {
            // Arrange
            var testDataPath = TestDataProvider.GetPath("Casc/PatchConfig.ini");

            // Act
            PatchConfig config;
            using (var stream = File.OpenRead(testDataPath))
            {
                config = PatchConfig.Parse(stream);
            }

            // Assert - Test basic properties
            Assert.IsNotNull(config);

            // Test patch manifest
            var patch = config.Patch;
            Assert.IsFalse(patch.IsEmpty);
            Assert.AreEqual("8dfa8cf743536bdba9a8f0eac167fa3a", patch.ToString().ToLowerInvariant());

            // Test patch size
            var patchSize = config.PatchSize;
            Assert.AreEqual(780489, patchSize);
        }

        [TestMethod]
        public void TestPatchConfigInstallEntry()
        {
            // Arrange
            var testDataPath = TestDataProvider.GetPath("Casc/PatchConfig.ini");

            // Act
            PatchConfig config;
            using (var stream = File.OpenRead(testDataPath))
            {
                config = PatchConfig.Parse(stream);
            }

            // Assert - Test install patch entry
            var installEntry = config.GetInstallPatchEntry();
            Assert.IsNotNull(installEntry);
            Assert.IsTrue(installEntry.HasValue);

            var entry = installEntry.Value;
            Assert.AreEqual(PatchEntryType.Install, entry.Type);
            Assert.AreEqual("38d6c6d4d2ab370c6320f3774b945729", entry.ContentHash.ToString().ToLowerInvariant());
            Assert.AreEqual(20518, entry.ContentSize);
            Assert.AreEqual("3e86f448f5429026022a91168ecac9e6", entry.EncodingKey.ToString().ToLowerInvariant());
            Assert.AreEqual(20145, entry.EncodedSize);
            Assert.AreEqual("b:{595=z,19923=n}", entry.EncodingString);

            // Test old version patches
            Assert.IsNotNull(entry.OldVersionPatches);
            Assert.AreEqual(4, entry.OldVersionPatches.Count);

            // Test first old version patch
            var firstPatch = entry.OldVersionPatches[0];
            Assert.AreEqual("7b7b7e60f8c019f9cbada2e8413551bc", firstPatch.OldEncodingKey.ToString().ToLowerInvariant());
            Assert.AreEqual(20518, firstPatch.OldContentSize);
            Assert.AreEqual("2f3885f62572cae102cd10d9fb7fe794", firstPatch.PatchHash.ToString().ToLowerInvariant());
            Assert.AreEqual(295, firstPatch.PatchSize);
        }

        [TestMethod]
        public void TestPatchConfigDownloadEntry()
        {
            // Arrange
            var testDataPath = TestDataProvider.GetPath("Casc/PatchConfig.ini");

            // Act
            PatchConfig config;
            using (var stream = File.OpenRead(testDataPath))
            {
                config = PatchConfig.Parse(stream);
            }

            // Assert - Test download patch entry
            var downloadEntry = config.GetDownloadPatchEntry();
            Assert.IsNotNull(downloadEntry);
            Assert.IsTrue(downloadEntry.HasValue);

            var entry = downloadEntry.Value;
            Assert.AreEqual(PatchEntryType.Download, entry.Type);
            Assert.AreEqual("efa0ec8d0ec37c51e4e043f6ad991254", entry.ContentHash.ToString().ToLowerInvariant());
            Assert.AreEqual(2900720, entry.ContentSize);
            Assert.AreEqual("b4a1c6a0f5c56b964711fa859baf5c41", entry.EncodingKey.ToString().ToLowerInvariant());
            Assert.AreEqual(2632351, entry.EncodedSize);
            Assert.AreEqual("b:{11=n,2631420=n,269289=z}", entry.EncodingString);

            // Test old version patches
            Assert.IsNotNull(entry.OldVersionPatches);
            Assert.AreEqual(4, entry.OldVersionPatches.Count);
        }

        [TestMethod]
        public void TestPatchConfigEncodingEntry()
        {
            // Arrange
            var testDataPath = TestDataProvider.GetPath("Casc/PatchConfig.ini");

            // Act
            PatchConfig config;
            using (var stream = File.OpenRead(testDataPath))
            {
                config = PatchConfig.Parse(stream);
            }

            // Assert - Test encoding patch entry
            var encodingEntry = config.GetEncodingPatchEntry();
            Assert.IsNotNull(encodingEntry);
            Assert.IsTrue(encodingEntry.HasValue);

            var entry = encodingEntry.Value;
            Assert.AreEqual(PatchEntryType.Encoding, entry.Type);
            Assert.AreEqual("e2491854f24f3fde84bc7bb78a8f26a5", entry.ContentHash.ToString().ToLowerInvariant());
            Assert.AreEqual(7657606, entry.ContentSize);
            Assert.AreEqual("112ccf5ca2eb7bf0458a6baa9f97bbd8", entry.EncodingKey.ToString().ToLowerInvariant());
            Assert.AreEqual(7657768, entry.EncodedSize);
            Assert.AreEqual("b:{22=n,91=z,35840=n,4587520=n,23520=n,3010560=n,*=z}", entry.EncodingString);

            // Test old version patches
            Assert.IsNotNull(entry.OldVersionPatches);
            Assert.AreEqual(4, entry.OldVersionPatches.Count);
        }

        [TestMethod]
        public void TestPatchConfigVfsEntries()
        {
            // Arrange
            var testDataPath = TestDataProvider.GetPath("Casc/PatchConfig.ini");

            // Act
            PatchConfig config;
            using (var stream = File.OpenRead(testDataPath))
            {
                config = PatchConfig.Parse(stream);
            }

            // Assert - Test that we can read VFS patch entries
            Assert.IsTrue(config.TryGetValue("patch-entry", out var firstEntry));
            Assert.IsNotNull(firstEntry);
            Assert.IsTrue(firstEntry.StartsWith("download", StringComparison.OrdinalIgnoreCase) ||
                          firstEntry.StartsWith("install", StringComparison.OrdinalIgnoreCase) ||
                          firstEntry.StartsWith("encoding", StringComparison.OrdinalIgnoreCase) ||
                          firstEntry.StartsWith("vfs", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void TestPatchConfigGetAllEntries()
        {
            // Arrange
            var testDataPath = TestDataProvider.GetPath("Casc/PatchConfig.ini");

            // Act
            PatchConfig config;
            using (var stream = File.OpenRead(testDataPath))
            {
                config = PatchConfig.Parse(stream);
            }

            // Assert - Test GetAllPatchEntries
            var allEntries = config.GetAllPatchEntries();
            Assert.IsNotNull(allEntries);

            // The test file should have at least download and encoding entries
            Assert.IsTrue(allEntries.ContainsKey(PatchEntryType.Download));
            Assert.IsTrue(allEntries.ContainsKey(PatchEntryType.Encoding));
            Assert.IsTrue(allEntries.ContainsKey(PatchEntryType.Install));

            // Verify each entry type
            var downloadEntry = allEntries[PatchEntryType.Download];
            Assert.AreEqual(PatchEntryType.Download, downloadEntry.Type);

            var encodingEntry = allEntries[PatchEntryType.Encoding];
            Assert.AreEqual(PatchEntryType.Encoding, encodingEntry.Type);

            var installEntry = allEntries[PatchEntryType.Install];
            Assert.AreEqual(PatchEntryType.Install, installEntry.Type);
        }

        [TestMethod]
        public void TestPatchConfigTryGetValue()
        {
            // Arrange
            var testDataPath = TestDataProvider.GetPath("Casc/PatchConfig.ini");

            // Act
            PatchConfig config;
            using (var stream = File.OpenRead(testDataPath))
            {
                config = PatchConfig.Parse(stream);
            }

            // Assert - Test TryGetValue method
            Assert.IsTrue(config.TryGetValue("patch", out var patchValue));
            Assert.AreEqual("8dfa8cf743536bdba9a8f0eac167fa3a", patchValue);

            Assert.IsTrue(config.TryGetValue("patch-size", out var patchSizeValue));
            Assert.AreEqual("780489", patchSizeValue);

            Assert.IsTrue(config.TryGetValue("patch-entry", out var patchEntryValue));
            Assert.IsNotNull(patchEntryValue);
            // The last patch-entry in the file should be the one in the dictionary (due to overwriting)
            // Looking at the file, the last patch-entry is for vfs:war3.w3mod:_locales/zhtw.w3mod
            Assert.IsTrue(patchEntryValue.StartsWith("vfs:", StringComparison.OrdinalIgnoreCase));

            Assert.IsFalse(config.TryGetValue("non-existent-key", out _));
        }

        [TestMethod]
        public void TestPatchConfigContainsKey()
        {
            // Arrange
            var testDataPath = TestDataProvider.GetPath("Casc/PatchConfig.ini");

            // Act
            PatchConfig config;
            using (var stream = File.OpenRead(testDataPath))
            {
                config = PatchConfig.Parse(stream);
            }

            // Assert - Test ContainsKey method
            Assert.IsTrue(config.ContainsKey("patch"));
            Assert.IsTrue(config.ContainsKey("patch-size"));
            Assert.IsTrue(config.ContainsKey("patch-entry"));
            // Note: patch-entry-1, patch-entry-2, etc. don't exist in this file - all are just "patch-entry"
            Assert.IsFalse(config.ContainsKey("patch-entry-1"));
            Assert.IsFalse(config.ContainsKey("patch-entry-2"));
            Assert.IsFalse(config.ContainsKey("non-existent-key"));
        }

        [TestMethod]
        public void TestPatchConfigOptionalValues()
        {
            // Arrange
            var testDataPath = TestDataProvider.GetPath("Casc/PatchConfig.ini");

            // Act
            PatchConfig config;
            using (var stream = File.OpenRead(testDataPath))
            {
                config = PatchConfig.Parse(stream);
            }

            // Assert - Test optional values
            // Note: patch-entry-3 doesn't exist - all patch entries use the same key "patch-entry"
            var patchEntry3 = config.GetOptionalValue("patch-entry-3");
            Assert.IsNull(patchEntry3); // Should be null since this key doesn't exist

            // Size entry might be present
            var sizeEntry = config.GetSizePatchEntry();
            if (sizeEntry.HasValue)
            {
                var entry = sizeEntry.Value;
                Assert.AreEqual(PatchEntryType.Size, entry.Type);
                Assert.IsNotNull(entry.ContentHash);
                Assert.IsTrue(entry.ContentSize > 0);
            }
        }

        [TestMethod]
        public void TestPatchConfigHashValidation()
        {
            // Arrange
            var testDataPath = TestDataProvider.GetPath("Casc/PatchConfig.ini");

            // Act
            PatchConfig config;
            using (var stream = File.OpenRead(testDataPath))
            {
                config = PatchConfig.Parse(stream);
            }

            // Assert - Validate hashes are valid MD5 format
            var patch = config.Patch;
            Assert.AreEqual(32, patch.ToString().ToLowerInvariant().Length, "Patch hash should be 32 characters (MD5)");

            var installEntry = config.GetInstallPatchEntry();
            if (installEntry.HasValue)
            {
                var entry = installEntry.Value;
                Assert.AreEqual(32, entry.ContentHash.ToString().ToLowerInvariant().Length, "Content hash should be 32 characters");
                Assert.AreEqual(32, entry.EncodingKey.ToString().ToLowerInvariant().Length, "Encoding key should be 32 characters");

                foreach (var oldPatch in entry.OldVersionPatches)
                {
                    Assert.AreEqual(32, oldPatch.OldEncodingKey.ToString().ToLowerInvariant().Length, "Old encoding key should be 32 characters");
                    Assert.AreEqual(32, oldPatch.PatchHash.ToString().ToLowerInvariant().Length, "Patch hash should be 32 characters");
                }
            }
        }

        [TestMethod]
        public void TestPatchConfigEmptyInstance()
        {
            // Arrange & Act
            var config = new PatchConfig();

            // Assert - Empty config should return empty/null values
            Assert.IsTrue(config.Patch.IsEmpty);
            Assert.AreEqual(0, config.PatchSize);
            Assert.IsNull(config.GetInstallPatchEntry());
            Assert.IsNull(config.GetDownloadPatchEntry());
            Assert.IsNull(config.GetEncodingPatchEntry());
            Assert.IsNull(config.GetSizePatchEntry());

            var allEntries = config.GetAllPatchEntries();
            Assert.IsNotNull(allEntries);
            Assert.AreEqual(0, allEntries.Count);
        }
    }
}