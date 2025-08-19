// ------------------------------------------------------------------------------
// <copyright file="BuildConfigTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.IO.Casc.Cdn;
using War3Net.TestTools.UnitTesting;

namespace War3Net.IO.Casc.Tests
{
    [TestClass]
    public class BuildConfigTests
    {
        [TestMethod]
        public void TestParseBuildConfig()
        {
            // Arrange
            var testDataPath = TestDataProvider.GetPath("Casc/BuildConfig.ini");

            // Act
            BuildConfig config;
            using (var stream = File.OpenRead(testDataPath))
            {
                config = BuildConfig.Parse(stream);
            }

            // Assert - Test required fields
            Assert.IsNotNull(config);

            // Test root hash
            var root = config.Root;
            Assert.IsFalse(root.IsEmpty, "Root should not be empty");
            Assert.AreEqual("07b62d28f0a0ed93a91c967bc789fa52", root.ToString().ToLowerInvariant());

            // Test install hash pair
            var install = config.Install;
            Assert.IsFalse(install.CKey.IsEmpty, "Install CKey should not be empty");
            Assert.IsTrue(install.HasEKey, "Install should have EKey");
            Assert.AreEqual("38d6c6d4d2ab370c6320f3774b945729", install.CKey.ToString().ToLowerInvariant());
            Assert.AreEqual("3e86f448f5429026022a91168ecac9e6", install.EKey.ToString().ToLowerInvariant());

            // Test install sizes
            var installSizes = config.InstallSizes;
            Assert.IsNotNull(installSizes);
            Assert.AreEqual(2, installSizes.Length);
            Assert.AreEqual(20518, installSizes[0]);
            Assert.AreEqual(20145, installSizes[1]);

            // Test download hash pair
            var download = config.Download;
            Assert.IsFalse(download.CKey.IsEmpty, "Download CKey should not be empty");
            Assert.IsTrue(download.HasEKey, "Download should have EKey");
            Assert.AreEqual("efa0ec8d0ec37c51e4e043f6ad991254", download.CKey.ToString().ToLowerInvariant());
            Assert.AreEqual("b4a1c6a0f5c56b964711fa859baf5c41", download.EKey.ToString().ToLowerInvariant());

            // Test download sizes
            var downloadSizes = config.DownloadSizes;
            Assert.IsNotNull(downloadSizes);
            Assert.AreEqual(2, downloadSizes.Length);
            Assert.AreEqual(2900720, downloadSizes[0]);
            Assert.AreEqual(2632351, downloadSizes[1]);

            // Test encoding hash pair
            var encoding = config.Encoding;
            Assert.IsFalse(encoding.CKey.IsEmpty, "Encoding CKey should not be empty");
            Assert.IsTrue(encoding.HasEKey, "Encoding should have EKey");
            Assert.AreEqual("e2491854f24f3fde84bc7bb78a8f26a5", encoding.CKey.ToString().ToLowerInvariant());
            Assert.AreEqual("112ccf5ca2eb7bf0458a6baa9f97bbd8", encoding.EKey.ToString().ToLowerInvariant());

            // Test encoding sizes
            var encodingSizes = config.EncodingSizes;
            Assert.IsNotNull(encodingSizes);
            Assert.AreEqual(2, encodingSizes.Length);
            Assert.AreEqual(7657606, encodingSizes[0]);
            Assert.AreEqual(7657768, encodingSizes[1]);

            // Test build metadata
            Assert.AreEqual("2.0.3.22988-retail", config.BuildName);
            Assert.AreEqual("w3", config.BuildUid);
            Assert.AreEqual("w3", config.BuildKey); // Alias for BuildUid
            Assert.AreEqual("War3", config.BuildProduct);
            Assert.AreEqual("RC 1 HOTFIX 1 PROD", config.BuildComments);
            Assert.AreEqual("ngdptool_casc2", config.BuildPlaybuildInstaller);

            // Test VFS root
            var vfsRoot = config.VfsRoot;
            Assert.IsFalse(vfsRoot.IsEmpty, "VFS root should not be empty");
            Assert.IsFalse(vfsRoot.CKey.IsEmpty, "VFS root CKey should not be empty");
            Assert.AreEqual("70d46a929f53caf0796c0575542a7e7a", vfsRoot.CKey.ToString().ToLowerInvariant());

            // Test patch fields (optional)
            var patch = config.Patch;
            Assert.IsFalse(patch.IsEmpty, "Patch should be present in this config");
            Assert.AreEqual("8dfa8cf743536bdba9a8f0eac167fa3a", patch.ToString().ToLowerInvariant());
            Assert.AreEqual(780489, config.PatchSize);

            var patchConfig = config.PatchConfig;
            Assert.IsFalse(patchConfig.IsEmpty, "Patch config should be present");
            Assert.AreEqual("ab3cf421fa37ccf91936ba08e5589798", patchConfig.ToString().ToLowerInvariant());

            // Test patch index fields
            var patchIndex = config.PatchIndex;
            Assert.IsFalse(patchIndex.IsEmpty, "Patch index should be present");
            Assert.IsFalse(patchIndex.CKey.IsEmpty, "Patch index CKey should not be empty");
            Assert.IsTrue(patchIndex.HasEKey, "Patch index should have EKey");
            Assert.AreEqual("6c01db8e4148602d548d4a1d88ea1c74", patchIndex.CKey.ToString().ToLowerInvariant());
            Assert.AreEqual("03f9d4885ede6a6425a791aa03389759", patchIndex.EKey.ToString().ToLowerInvariant());

            var patchIndexSizes = config.PatchIndexSizes;
            Assert.IsNotNull(patchIndexSizes);
            Assert.AreEqual(2, patchIndexSizes.Length);
            Assert.AreEqual(1150651, patchIndexSizes[0]);
            Assert.AreEqual(969034, patchIndexSizes[1]);

            // Test TryGetValue method
            Assert.IsTrue(config.TryGetValue("build-name", out var buildName));
            Assert.AreEqual("2.0.3.22988-retail", buildName);

            Assert.IsTrue(config.TryGetValue("build-source-revision", out var sourceRevision));
            Assert.AreEqual("b72c6dde8afe08c0485d1d9afea6b0f98fa54f7f", sourceRevision);

            Assert.IsFalse(config.TryGetValue("non-existent-key", out _));

            // Test ContainsKey method
            Assert.IsTrue(config.ContainsKey("root"));
            Assert.IsTrue(config.ContainsKey("encoding"));
            Assert.IsTrue(config.ContainsKey("vfs-root"));
            Assert.IsFalse(config.ContainsKey("non-existent-key"));
        }

        [TestMethod]
        public void TestParseVfsManifests()
        {
            // Arrange
            var testDataPath = TestDataProvider.GetPath("Casc/BuildConfig.ini");

            // Act
            BuildConfig config;
            using (var stream = File.OpenRead(testDataPath))
            {
                config = BuildConfig.Parse(stream);
            }

            // Assert - Test VFS manifests
            var vfsManifests = config.GetAllVfsManifests();
            Assert.IsNotNull(vfsManifests);

            // The test file has vfs-1 through vfs-121
            Assert.IsTrue(vfsManifests.Count > 100, $"Expected over 100 VFS manifests, got {vfsManifests.Count}");

            // Test specific VFS entries
            var vfs1 = config.GetVfsManifest(1);
            Assert.IsFalse(vfs1.IsEmpty, "VFS-1 should exist");
            Assert.IsFalse(vfs1.CKey.IsEmpty, "VFS-1 CKey should not be empty");
            Assert.AreEqual("70d46a929f53caf0796c0575542a7e7a", vfs1.CKey.ToString().ToLowerInvariant());

            var vfs1Size = config.GetVfsManifestSize(1);
            Assert.AreEqual(14000, vfs1Size);

            // Test that vfs-1 matches vfs-root (they have the same hash in the test data)
            Assert.AreEqual(config.VfsRoot.CKey.ToString(), vfs1.CKey.ToString());

            // Test a high-numbered VFS manifest
            var vfs121 = config.GetVfsManifest(121);
            Assert.IsFalse(vfs121.IsEmpty, "VFS-121 should exist");
            Assert.IsFalse(vfs121.CKey.IsEmpty, "VFS-121 CKey should not be empty");
            Assert.AreEqual("06c75b987989a5397101923042c73f71", vfs121.CKey.ToString().ToLowerInvariant());

            var vfs121Size = config.GetVfsManifestSize(121);
            Assert.AreEqual(3006, vfs121Size);

            // Test non-existent VFS manifest
            var vfs999 = config.GetVfsManifest(999);
            Assert.IsTrue(vfs999.IsEmpty, "VFS-999 should not exist");

            var vfs999Size = config.GetVfsManifestSize(999);
            Assert.AreEqual(0, vfs999Size, "Non-existent VFS size should be 0");
        }

        [TestMethod]
        public void TestBuildConfigAllKeys()
        {
            // Arrange
            var testDataPath = TestDataProvider.GetPath("Casc/BuildConfig.ini");

            // Act
            BuildConfig config;
            using (var stream = File.OpenRead(testDataPath))
            {
                config = BuildConfig.Parse(stream);
            }

            // Assert - Test that all keys in the file are accessible
            var allKeys = config.GetAll();
            Assert.IsNotNull(allKeys);

            // The test file should have many keys
            Assert.IsTrue(allKeys.Count > 50, $"Expected many keys, got {allKeys.Count}");

            // Verify some known keys exist
            Assert.IsTrue(allKeys.ContainsKey("root"));
            Assert.IsTrue(allKeys.ContainsKey("install"));
            Assert.IsTrue(allKeys.ContainsKey("download"));
            Assert.IsTrue(allKeys.ContainsKey("encoding"));
            Assert.IsTrue(allKeys.ContainsKey("build-name"));
            Assert.IsTrue(allKeys.ContainsKey("build-uid"));
            Assert.IsTrue(allKeys.ContainsKey("vfs-root"));

            // Verify the values match what we expect
            Assert.AreEqual("07b62d28f0a0ed93a91c967bc789fa52", allKeys["root"]);
            Assert.AreEqual("2.0.3.22988-retail", allKeys["build-name"]);
            Assert.AreEqual("w3", allKeys["build-uid"]);
        }

        [TestMethod]
        public void TestOptionalFields()
        {
            // Arrange
            var testDataPath = TestDataProvider.GetPath("Casc/BuildConfig.ini");

            // Act
            BuildConfig config;
            using (var stream = File.OpenRead(testDataPath))
            {
                config = BuildConfig.Parse(stream);
            }

            // Test size file (optional)
            var size = config.Size;
            Assert.IsTrue(size.HasEKey, "Size should have both keys in this config");
            Assert.AreEqual("9da27123f5404c6f340eb268ddc5e513", size.CKey.ToString().ToLowerInvariant());
            Assert.AreEqual("bf6803f6a66e96ea0c497298842e8118", size.EKey.ToString().ToLowerInvariant());

            var sizeSizes = config.SizeSizes;
            Assert.IsNotNull(sizeSizes);
            Assert.AreEqual(2, sizeSizes.Length);
            Assert.AreEqual(1824234, sizeSizes[0]);
            Assert.AreEqual(1684008, sizeSizes[1]);

            // Test build attributes (not present in this file)
            var buildAttributes = config.BuildAttributes;
            Assert.IsNull(buildAttributes, "Build attributes should not be present in this config");

            // Test build status
            var buildStatus = config.BuildStatus;
            Assert.IsNotNull(buildStatus);
            Assert.AreEqual("0", buildStatus);

            // Test build source revision
            var buildSourceRevision = config.BuildSourceRevision;
            Assert.IsNotNull(buildSourceRevision);
            Assert.AreEqual("b72c6dde8afe08c0485d1d9afea6b0f98fa54f7f", buildSourceRevision);

            // Test build source branch
            var buildSourceBranch = config.BuildSourceBranch;
            Assert.IsNotNull(buildSourceBranch);
            Assert.AreEqual("v2.0.3", buildSourceBranch);

            // Test build data revision
            var buildDataRevision = config.BuildDataRevision;
            Assert.IsNotNull(buildDataRevision);
            Assert.AreEqual("188572", buildDataRevision);

            // Test build data branch
            var buildDataBranch = config.BuildDataBranch;
            Assert.IsNotNull(buildDataBranch);
            Assert.AreEqual("v2.0.3", buildDataBranch);

            // The BuildBranch property looks for "build-branch" which is not in this file
            var buildBranch = config.BuildBranch;
            Assert.IsNull(buildBranch, "build-branch key is not present in this config");
        }
    }
}