// ------------------------------------------------------------------------------
// <copyright file="BuildConfig.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.IO.Casc.Cdn
{
    /// <summary>
    /// Represents a build configuration file.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The build config file contains hashes and metadata for all the critical data files needed to
    /// reconstruct a specific game build. It serves as the entry point for accessing game content.
    /// </para>
    /// <para>
    /// The first value in paired fields is typically the <see cref="Structures.CascKey"/>, while the second
    /// (if present) is the <see cref="Structures.EKey"/>. If the second value is absent, the <see cref="Structures.EKey"/> must
    /// be looked up in the <see cref="Encoding.EncodingFile"/>.
    /// </para>
    /// </remarks>
    public class BuildConfig
    {
        private readonly Dictionary<string, string> _data;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildConfig"/> class.
        /// </summary>
        public BuildConfig()
        {
            _data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the content hash (<see cref="Structures.CascKey"/>) of the decoded root file.
        /// </summary>
        /// <remarks>
        /// Look this up in encoding to get the encoded hash.
        /// </remarks>
        public string? Root => GetValue("root");

        /// <summary>
        /// Gets the install manifest hash.
        /// </summary>
        /// <remarks>
        /// First key is the content hash of the decoded install file.
        /// Second key, if present, is the encoded hash; if absent, look up in encoding.
        /// </remarks>
        public string? Install => GetValue("install");

        /// <summary>
        /// Gets the install size(s) corresponding to the install hash(es).
        /// </summary>
        /// <remarks>
        /// Absent in older WoW builds.
        /// </remarks>
        public string? InstallSize => GetValue("install-size");

        /// <summary>
        /// Gets the download manifest hash.
        /// </summary>
        /// <remarks>
        /// First key is the content hash of the decoded download file.
        /// Second key, if present, is the encoded hash; if absent, look up in encoding.
        /// </remarks>
        public string? Download => GetValue("download");

        /// <summary>
        /// Gets the download size(s) corresponding to the download hash(es).
        /// </summary>
        /// <remarks>
        /// Absent in older WoW builds.
        /// </remarks>
        public string? DownloadSize => GetValue("download-size");

        /// <summary>
        /// Gets the size file hash.
        /// </summary>
        /// <remarks>
        /// CKey and EKey of the download size file, respectively.
        /// Introduced in WoW build 27547.
        /// </remarks>
        public string? Size => GetValue("size");

        /// <summary>
        /// Gets the download size sizes corresponding to the download size keys.
        /// </summary>
        public string? SizeSize => GetValue("size-size");

        /// <summary>
        /// Gets the partial priority for builds.
        /// </summary>
        /// <remarks>
        /// Content hash:block size pairs for priority non-archived files, ordered by priority.
        /// Optional field.
        /// </remarks>
        public string? BuildPartialPriority => GetValue("build-partial-priority");

        /// <summary>
        /// Gets the partial priority file hash.
        /// </summary>
        /// <remarks>
        /// Content hash of a partial download file containing priority files to download first.
        /// Optional field.
        /// </remarks>
        public string? PartialPriority => GetValue("partial-priority");

        /// <summary>
        /// Gets the partial priority size.
        /// </summary>
        /// <remarks>
        /// Unknown: always 0 if present. Present if partial-priority is present.
        /// </remarks>
        public string? PartialPrioritySize => GetValue("partial-priority-size");

        /// <summary>
        /// Gets the encoding file hash.
        /// </summary>
        /// <remarks>
        /// First key is the content hash of the decoded encoding file.
        /// Second key is the encoded hash.
        /// </remarks>
        public string? Encoding => GetValue("encoding");

        /// <summary>
        /// Gets the encoding sizes corresponding to the encoding hashes.
        /// </summary>
        public string? EncodingSize => GetValue("encoding-size");

        /// <summary>
        /// Gets the patch manifest of patchable data files.
        /// </summary>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public string? Patch => GetValue("patch");

        /// <summary>
        /// Gets the size of the patch manifest, if any.
        /// </summary>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public string? PatchSize => GetValue("patch-size");

        /// <summary>
        /// Gets the content hash of non-encoded patch config.
        /// </summary>
        public string? PatchConfig => GetValue("patch-config");

        /// <summary>
        /// Gets the build attributes.
        /// </summary>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public string? BuildAttributes => GetValue("build-attributes");

        /// <summary>
        /// Gets the build branch.
        /// </summary>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public string? BuildBranch => GetValue("build-branch");

        /// <summary>
        /// Gets the build comments.
        /// </summary>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public string? BuildComments => GetValue("build-comments");

        /// <summary>
        /// Gets the build creator.
        /// </summary>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public string? BuildCreator => GetValue("build-creator");

        /// <summary>
        /// Gets the build fixed hash.
        /// </summary>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public string? BuildFixedHash => GetValue("build-fixed-hash");

        /// <summary>
        /// Gets the build replay hash.
        /// </summary>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public string? BuildReplayHash => GetValue("build-replay-hash");

        /// <summary>
        /// Gets the build name.
        /// </summary>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public string? BuildName => GetValue("build-name");

        /// <summary>
        /// Gets the build playbuild installer.
        /// </summary>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public string? BuildPlaybuildInstaller => GetValue("build-playbuild-installer");

        /// <summary>
        /// Gets the product name.
        /// </summary>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public string? BuildProduct => GetValue("build-product");

        /// <summary>
        /// Gets the build T1 manifest version.
        /// </summary>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public string? BuildT1ManifestVersion => GetValue("build-t1-manifest-version");

        /// <summary>
        /// Gets the build UID (program code).
        /// </summary>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public string? BuildUid => GetValue("build-uid");

        /// <summary>
        /// Gets the build key (alias for BuildUid).
        /// </summary>
        public string? BuildKey => BuildUid;

        /// <summary>
        /// Gets the VFS root content hash (used by Warcraft III).
        /// </summary>
        public string? VfsRoot => GetValue("vfs-root");

        /// <summary>
        /// Gets the VFS root size.
        /// </summary>
        public string? VfsRootSize => GetValue("vfs-root-size");

        /// <summary>
        /// Gets a VFS manifest entry by index.
        /// </summary>
        /// <param name="index">The index of the VFS manifest.</param>
        /// <returns>The VFS manifest hash, or null if not found.</returns>
        public string? GetVfsManifest(int index) => GetValue($"vfs-{index}");

        /// <summary>
        /// Gets a VFS manifest size by index.
        /// </summary>
        /// <param name="index">The index of the VFS manifest.</param>
        /// <returns>The VFS manifest size, or null if not found.</returns>
        public string? GetVfsManifestSize(int index) => GetValue($"vfs-{index}-size");

        /// <summary>
        /// Gets all VFS manifest entries.
        /// </summary>
        /// <returns>A dictionary of index to VFS manifest hash.</returns>
        public Dictionary<int, string> GetAllVfsManifests()
        {
            var result = new Dictionary<int, string>();
            foreach (var kvp in _data)
            {
                if (kvp.Key.StartsWith("vfs-", StringComparison.OrdinalIgnoreCase) &&
                    !kvp.Key.EndsWith("-size", StringComparison.OrdinalIgnoreCase) &&
                    kvp.Key != "vfs-root" && kvp.Key != "vfs-root-size")
                {
                    var indexStr = kvp.Key[4..];
                    if (int.TryParse(indexStr, out var index))
                    {
                        result[index] = kvp.Value;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Parses a build config from a stream.
        /// </summary>
        /// <param name="stream">The stream to parse.</param>
        /// <returns>The parsed build config.</returns>
        public static BuildConfig Parse(Stream stream)
        {
            var config = new BuildConfig();
            using var reader = new StreamReader(stream);

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#", StringComparison.Ordinal))
                {
                    continue;
                }

                var equalIndex = line.IndexOf('=', StringComparison.Ordinal);
                if (equalIndex < 0)
                {
                    continue;
                }

                var key = line[..equalIndex].Trim();
                var value = line[(equalIndex + 1)..].Trim();

                config._data[key] = value;
            }

            return config;
        }

        /// <summary>
        /// Gets the value corresponding to the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value, or <see langword="null"/>  if not found.</returns>
        public string? GetValue(string key)
        {
            return _data.TryGetValue(key, out var value) ? value : null;
        }

        /// <summary>
        /// Tries to get the value corresponding to the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key to look up.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, null.</param>
        /// <returns><see langword="true"/> if the key was found; otherwise, <see langword="false"/>.</returns>
        public bool TryGetValue(string key, [NotNullWhen(true)] out string? value)
        {
            return _data.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets all key-value pairs.
        /// </summary>
        /// <returns>A dictionary of all key-value pairs.</returns>
        public IReadOnlyDictionary<string, string> GetAll()
        {
            return _data;
        }
    }
}