// ------------------------------------------------------------------------------
// <copyright file="BuildConfig.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;

using War3Net.IO.Casc.Structures;

namespace War3Net.IO.Casc.Cdn
{
    /// <summary>
    /// Represents a build configuration file.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The build config file contains hashes and metadata for all the critical data files needed to
    /// reconstruct a specific game build. It serves as the entry point for accessing game content
    /// in the TACT system used by <see cref="Storage.OnlineCascStorage"/>.
    /// </para>
    /// <para>
    /// The first value in paired fields is typically the <see cref="CascKey"/>, while the second
    /// (if present) is the <see cref="EKey"/>. If the second value is absent, the <see cref="EKey"/> must
    /// be looked up in the <see cref="Encoding.EncodingFile"/>.
    /// </para>
    /// <para>
    /// Key relationships in the build config:
    /// </para>
    /// <list type="bullet">
    /// <item><description><see cref="Root"/> - Points to the root file (e.g., <see cref="Root.TvfsRootHandler"/> for Warcraft III)</description></item>
    /// <item><description><see cref="Encoding"/> - Points to the <see cref="Encoding.EncodingFile"/> for <see cref="CascKey"/> to <see cref="EKey"/> resolution</description></item>
    /// <item><description><see cref="Install"/> and <see cref="Download"/> - Point to manifest files for installation and patching</description></item>
    /// <item><description><see cref="VfsRoot"/> - Specific to Warcraft III, points to the VFS root directory structure</description></item>
    /// </list>
    /// </remarks>
    public class BuildConfig : ConfigBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildConfig"/> class.
        /// </summary>
        public BuildConfig()
            : base()
        {
        }

        /// <summary>
        /// Gets the content hash of the decoded root file.
        /// </summary>
        /// <value>The <see cref="CascKey"/> of the root file.</value>
        /// <remarks>
        /// <para>
        /// Look this up in <see cref="Encoding.EncodingFile"/> to get the corresponding <see cref="EKey"/>.
        /// The root file contains the mapping from file paths to <see cref="CascKey"/>s, implemented
        /// by handlers like <see cref="Root.TvfsRootHandler"/> for Warcraft III.
        /// </para>
        /// </remarks>
        /// <exception cref="KeyNotFoundException">Thrown when the 'root' key is not found in the configuration.</exception>
        public CascKey Root => CascKey.Parse(GetRequiredValue("root"));

        /// <summary>
        /// Gets the install manifest hash pair.
        /// </summary>
        /// <value>The <see cref="CascKeyPair"/> for the install manifest.</value>
        /// <remarks>
        /// <para>
        /// First key is the <see cref="CascKey"/> of the decoded install file.
        /// Second key, if present, is the <see cref="EKey"/>; if absent, look up in <see cref="Encoding.EncodingFile"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="KeyNotFoundException">Thrown when the 'install' key is not found in the configuration.</exception>
        public CascKeyPair Install => CascKeyPair.Parse(GetRequiredValue("install"));

        /// <summary>
        /// Gets the install sizes corresponding to the install hash(es).
        /// </summary>
        /// <exception cref="KeyNotFoundException">Thrown when the 'install-size' key is not found in the configuration.</exception>
        public long[] InstallSizes => ParseSizes(GetRequiredValue("install-size"));

        /// <summary>
        /// Gets the download manifest hash pair.
        /// </summary>
        /// <value>The <see cref="CascKeyPair"/> for the download manifest.</value>
        /// <remarks>
        /// <para>
        /// First key is the <see cref="CascKey"/> of the decoded download file.
        /// Second key, if present, is the <see cref="EKey"/>; if absent, look up in <see cref="Encoding.EncodingFile"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="KeyNotFoundException">Thrown when the 'download' key is not found in the configuration.</exception>
        public CascKeyPair Download => CascKeyPair.Parse(GetRequiredValue("download"));

        /// <summary>
        /// Gets the download sizes corresponding to the download hash(es).
        /// </summary>
        /// <exception cref="KeyNotFoundException">Thrown when the 'download-size' key is not found in the configuration.</exception>
        public long[] DownloadSizes => ParseSizes(GetRequiredValue("download-size"));

        /// <summary>
        /// Gets the size file hash pair.
        /// </summary>
        /// <value>The <see cref="CascKeyPair"/> for the download size file, or <see cref="CascKeyPair.Empty"/> if not present.</value>
        /// <remarks>
        /// CKey and EKey of the download size file, respectively.
        /// Introduced in WoW build 27547.
        /// </remarks>
        public CascKeyPair Size => TryGetKeyPair("size");

        /// <summary>
        /// Gets the download size sizes corresponding to the download size keys.
        /// </summary>
        public long[] SizeSizes => ParseSizes(GetRequiredValue("size-size"));

        /// <summary>
        /// Gets the partial priority for builds.
        /// </summary>
        /// <value>Content hash:block size pairs, or <see langword="null"/> if not present.</value>
        /// <remarks>
        /// Content hash:block size pairs for priority non-archived files, ordered by priority.
        /// Optional field.
        /// </remarks>
        public string? BuildPartialPriority => GetOptionalValue("build-partial-priority");

        /// <summary>
        /// Gets the partial priority file hash.
        /// </summary>
        /// <value>The <see cref="CascKey"/> of the partial priority file, or <see cref="CascKey.Empty"/> if not present.</value>
        /// <remarks>
        /// Content hash of a partial download file containing priority files to download first.
        /// Optional field.
        /// </remarks>
        public CascKey PartialPriority => TryGetCKey("partial-priority");

        /// <summary>
        /// Gets the partial priority size.
        /// </summary>
        /// <remarks>
        /// Unknown: always 0 if present. Present if partial-priority is present.
        /// </remarks>
        public long PartialPrioritySize => ParseSize(GetOptionalValue("partial-priority-size"));

        /// <summary>
        /// Gets the encoding file hash pair.
        /// </summary>
        /// <value>The <see cref="CascKeyPair"/> for the encoding file.</value>
        /// <remarks>
        /// <para>
        /// First key is the <see cref="CascKey"/> of the decoded <see cref="Encoding.EncodingFile"/>.
        /// Second key is the <see cref="EKey"/> used to retrieve the file from CDN or <see cref="Index.IndexFile"/>.
        /// </para>
        /// <para>
        /// The encoding file is critical for <see cref="Storage.OnlineCascStorage"/> as it provides the mappings
        /// needed to resolve content keys to encoded keys for file retrieval.
        /// </para>
        /// </remarks>
        /// <exception cref="KeyNotFoundException">Thrown when the 'encoding' key is not found in the configuration.</exception>
        public CascKeyPair Encoding => CascKeyPair.Parse(GetRequiredValue("encoding"));

        /// <summary>
        /// Gets the encoding sizes corresponding to the encoding hashes.
        /// </summary>
        /// <exception cref="KeyNotFoundException">Thrown when the 'encoding-size' key is not found in the configuration.</exception>
        public long[] EncodingSizes => ParseSizes(GetRequiredValue("encoding-size"));

        /// <summary>
        /// Gets the patch manifest hash.
        /// </summary>
        /// <value>The <see cref="CascKey"/> of the patch manifest, or <see cref="CascKey.Empty"/> if not present.</value>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public CascKey Patch => TryGetCKey("patch");

        /// <summary>
        /// Gets the size of the patch manifest.
        /// </summary>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public long PatchSize => ParseSize(GetOptionalValue("patch-size"));

        /// <summary>
        /// Gets the content hash of non-encoded patch config.
        /// </summary>
        /// <value>The <see cref="CascKey"/> of the patch config, or <see cref="CascKey.Empty"/> if not present.</value>
        public CascKey PatchConfig => TryGetCKey("patch-config");

        /// <summary>
        /// Gets the patch index hash pair.
        /// </summary>
        /// <value>The <see cref="CascKeyPair"/> for the patch index file, or <see cref="CascKeyPair.Empty"/> if not present.</value>
        /// <remarks>
        /// The patch index contains mappings for patch data. First value is the <see cref="CascKey"/>,
        /// second value (if present) is the <see cref="EKey"/>.
        /// </remarks>
        public CascKeyPair PatchIndex => TryGetKeyPair("patch-index");

        /// <summary>
        /// Gets the patch index sizes.
        /// </summary>
        /// <remarks>
        /// Sizes corresponding to the patch index hashes.
        /// </remarks>
        public long[] PatchIndexSizes => ParseSizes(GetOptionalValue("patch-index-size"));

        /// <summary>
        /// Gets the build attributes.
        /// </summary>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public string? BuildAttributes => GetOptionalValue("build-attributes");

        /// <summary>
        /// Gets the build branch.
        /// </summary>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public string? BuildBranch => GetOptionalValue("build-branch");

        /// <summary>
        /// Gets the build comments.
        /// </summary>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public string? BuildComments => GetOptionalValue("build-comments");

        /// <summary>
        /// Gets the build creator.
        /// </summary>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public string? BuildCreator => GetOptionalValue("build-creator");

        /// <summary>
        /// Gets the build fixed hash.
        /// </summary>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public string? BuildFixedHash => GetOptionalValue("build-fixed-hash");

        /// <summary>
        /// Gets the build replay hash.
        /// </summary>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public string? BuildReplayHash => GetOptionalValue("build-replay-hash");

        /// <summary>
        /// Gets the build name.
        /// </summary>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public string? BuildName => GetOptionalValue("build-name");

        /// <summary>
        /// Gets the build playbuild installer.
        /// </summary>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public string? BuildPlaybuildInstaller => GetOptionalValue("build-playbuild-installer");

        /// <summary>
        /// Gets the product name.
        /// </summary>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public string? BuildProduct => GetOptionalValue("build-product");

        /// <summary>
        /// Gets the build T1 manifest version.
        /// </summary>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public string? BuildT1ManifestVersion => GetOptionalValue("build-t1-manifest-version");

        /// <summary>
        /// Gets the build UID (program code).
        /// </summary>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public string? BuildUid => GetOptionalValue("build-uid");

        /// <summary>
        /// Gets the build key (alias for BuildUid).
        /// </summary>
        public string? BuildKey => BuildUid;

        /// <summary>
        /// Gets the build status.
        /// </summary>
        /// <remarks>
        /// Build status indicator. Optional field.
        /// </remarks>
        public string? BuildStatus => GetOptionalValue("build-status");

        /// <summary>
        /// Gets the build source revision.
        /// </summary>
        /// <remarks>
        /// Source control revision hash. Optional field.
        /// </remarks>
        public string? BuildSourceRevision => GetOptionalValue("build-source-revision");

        /// <summary>
        /// Gets the build source branch.
        /// </summary>
        /// <remarks>
        /// Source control branch name. Optional field.
        /// </remarks>
        public string? BuildSourceBranch => GetOptionalValue("build-source-branch");

        /// <summary>
        /// Gets the build data revision.
        /// </summary>
        /// <remarks>
        /// Data revision number. Optional field.
        /// </remarks>
        public string? BuildDataRevision => GetOptionalValue("build-data-revision");

        /// <summary>
        /// Gets the build data branch.
        /// </summary>
        /// <remarks>
        /// Data branch name. Optional field.
        /// </remarks>
        public string? BuildDataBranch => GetOptionalValue("build-data-branch");

        /// <summary>
        /// Gets the VFS root hash pair (used by Warcraft III).
        /// </summary>
        /// <value>The <see cref="CascKeyPair"/> of the VFS root directory structure, or <see cref="CascKeyPair.Empty"/> if not present.</value>
        /// <remarks>
        /// <para>
        /// This is specific to Warcraft III and points to the Virtual File System root that contains
        /// the directory structure. This is different from the <see cref="Root"/> property which points
        /// to the <see cref="Root.TvfsRootHandler"/> file containing path-to-key mappings.
        /// </para>
        /// </remarks>
        public CascKeyPair VfsRoot => TryGetKeyPair("vfs-root");

        /// <summary>
        /// Gets the VFS root size.
        /// </summary>
        public long VfsRootSize => ParseSize(GetOptionalValue("vfs-root-size"));

        /// <summary>
        /// Gets a VFS manifest entry by index.
        /// </summary>
        /// <param name="index">The index of the VFS manifest to retrieve.</param>
        /// <returns>The <see cref="CascKeyPair"/> of the VFS manifest, or <see cref="CascKeyPair.Empty"/> if not found.</returns>
        /// <remarks>
        /// VFS manifests are numbered sequentially (vfs-0, vfs-1, etc.) and contain additional
        /// directory and file structure information used by Warcraft III's CASC implementation.
        /// </remarks>
        public CascKeyPair GetVfsManifest(int index) => TryGetKeyPair($"vfs-{index}");

        /// <summary>
        /// Gets a VFS manifest size by index.
        /// </summary>
        /// <param name="index">The index of the VFS manifest.</param>
        /// <returns>The VFS manifest size, or 0 if not found.</returns>
        public long GetVfsManifestSize(int index) => ParseSize(GetOptionalValue($"vfs-{index}-size"));

        /// <summary>
        /// Gets all VFS manifest entries.
        /// </summary>
        /// <returns>A dictionary mapping VFS manifest indices to their <see cref="CascKeyPair"/>s.</returns>
        /// <remarks>
        /// This method scans all configuration keys for VFS manifest entries (vfs-0, vfs-1, etc.)
        /// and returns them in a dictionary for easy access. Used by Warcraft III for comprehensive
        /// file system structure information.
        /// </remarks>
        public Dictionary<int, CascKeyPair> GetAllVfsManifests()
        {
            var result = new Dictionary<int, CascKeyPair>();
            foreach (var kvp in _data)
            {
                if (kvp.Key.StartsWith("vfs-", StringComparison.OrdinalIgnoreCase) &&
                    !kvp.Key.EndsWith("-size", StringComparison.OrdinalIgnoreCase) &&
                    kvp.Key != "vfs-root" && kvp.Key != "vfs-root-size")
                {
                    var indexStr = kvp.Key[4..];
                    if (int.TryParse(indexStr, out var index))
                    {
                        result[index] = CascKeyPair.Parse(kvp.Value);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Parses a build configuration from a stream.
        /// </summary>
        /// <param name="stream">The stream containing build configuration data.</param>
        /// <returns>A new <see cref="BuildConfig"/> instance with all configuration values loaded.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stream"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// The build configuration format is a simple key=value text format with comments starting with #.
        /// This file is typically downloaded by <see cref="Storage.OnlineCascStorage"/> from the CDN
        /// using hash values from version configuration files.
        /// </remarks>
        public static BuildConfig Parse(Stream stream)
        {
            return ParseConfig(stream, new BuildConfig());
        }

        private CascKey TryGetCKey(string key)
        {
            var value = GetOptionalValue(key);
            if (string.IsNullOrWhiteSpace(value))
            {
                return CascKey.Empty;
            }

            // If there are multiple values, take the first one (CKey)
            var parts = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length > 0 ? CascKey.Parse(parts[0]) : CascKey.Empty;
        }

        private CascKeyPair TryGetKeyPair(string key)
        {
            var value = GetOptionalValue(key);
            return string.IsNullOrWhiteSpace(value) ? CascKeyPair.Empty : CascKeyPair.Parse(value);
        }
    }
}