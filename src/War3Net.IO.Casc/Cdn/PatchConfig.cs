// ------------------------------------------------------------------------------
// <copyright file="PatchConfig.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using War3Net.IO.Casc.Enums;

using War3Net.IO.Casc.Structures;

namespace War3Net.IO.Casc.Cdn
{
    /// <summary>
    /// Represents a patch configuration file.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The patch config file directs the system to download patch files to apply and update
    /// previously downloaded material, reducing redundant downloads.
    /// </para>
    /// <para>
    /// This configuration file was added after all of the others. It first appeared in CASC v1
    /// for Heroes of the Storm in August 2014. It then appeared in WoW for CASC v2 in build 19027
    /// (October 10th, 2014).
    /// </para>
    /// <para>
    /// Each patch-entry contains information about patches for the install, download, encoding,
    /// and size files. The format includes the target file hash, size, encoding key, and a list
    /// of patches from older versions to bring them up to date.
    /// </para>
    /// </remarks>
    public class PatchConfig : ConfigBase
    {
        private readonly List<string> _patchEntries = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PatchConfig"/> class.
        /// </summary>
        public PatchConfig()
            : base()
        {
        }

        /// <summary>
        /// Gets the patch manifest file hash.
        /// </summary>
        /// <value>The <see cref="CascKey"/> of the patch manifest file, or <see cref="CascKey.Empty"/> if not present.</value>
        /// <remarks>
        /// The patch manifest file contains information about patchable data files.
        /// </remarks>
        public CascKey Patch => TryGetCKey("patch");

        /// <summary>
        /// Gets the size of the patch manifest file.
        /// </summary>
        /// <remarks>
        /// Optional field.
        /// </remarks>
        public long PatchSize => ParseSize(GetOptionalValue("patch-size"));

        /// <summary>
        /// Gets the patch entries for the install file.
        /// </summary>
        /// <returns>A <see cref="PatchEntry"/> for the install file, or <see langword="null"/> if not present.</returns>
        /// <remarks>
        /// Contains patch information to update the install manifest from older versions.
        /// </remarks>
        public PatchEntry? GetInstallPatchEntry()
        {
            // Find install entry from the stored patch entries list
            var installEntry = _patchEntries.FirstOrDefault(e => e.StartsWith("install ", StringComparison.OrdinalIgnoreCase));
            return installEntry != null ? ParsePatchEntry(installEntry, PatchEntryType.Install) : null;
        }

        /// <summary>
        /// Gets the patch entries for the download file.
        /// </summary>
        /// <returns>A <see cref="PatchEntry"/> for the download file, or <see langword="null"/> if not present.</returns>
        /// <remarks>
        /// Contains patch information to update the download manifest from older versions.
        /// </remarks>
        public PatchEntry? GetDownloadPatchEntry()
        {
            // Find download entry from the stored patch entries list
            var downloadEntry = _patchEntries.FirstOrDefault(e => e.StartsWith("download ", StringComparison.OrdinalIgnoreCase));
            return downloadEntry != null ? ParsePatchEntry(downloadEntry, PatchEntryType.Download) : null;
        }

        /// <summary>
        /// Gets the patch entries for the encoding file.
        /// </summary>
        /// <returns>A <see cref="PatchEntry"/> for the encoding file, or <see langword="null"/> if not present.</returns>
        /// <remarks>
        /// Contains patch information to update the encoding file from older versions.
        /// </remarks>
        public PatchEntry? GetEncodingPatchEntry()
        {
            // Find encoding entry from the stored patch entries list
            var encodingEntry = _patchEntries.FirstOrDefault(e => e.StartsWith("encoding ", StringComparison.OrdinalIgnoreCase));
            return encodingEntry != null ? ParsePatchEntry(encodingEntry, PatchEntryType.Encoding) : null;
        }

        /// <summary>
        /// Gets the patch entries for the size file.
        /// </summary>
        /// <returns>A <see cref="PatchEntry"/> for the size file, or <see langword="null"/> if not present.</returns>
        /// <remarks>
        /// Contains patch information to update the size file from older versions.
        /// </remarks>
        public PatchEntry? GetSizePatchEntry()
        {
            // Find size entry from the stored patch entries list
            var sizeEntry = _patchEntries.FirstOrDefault(e => e.StartsWith("size ", StringComparison.OrdinalIgnoreCase));
            return sizeEntry != null ? ParsePatchEntry(sizeEntry, PatchEntryType.Size) : null;
        }

        /// <summary>
        /// Gets all patch entries.
        /// </summary>
        /// <returns>A dictionary mapping <see cref="PatchEntryType"/> to <see cref="PatchEntry"/>.</returns>
        public Dictionary<PatchEntryType, PatchEntry> GetAllPatchEntries()
        {
            var result = new Dictionary<PatchEntryType, PatchEntry>();

            var install = GetInstallPatchEntry();
            if (install.HasValue)
            {
                result[PatchEntryType.Install] = install.Value;
            }

            var download = GetDownloadPatchEntry();
            if (download.HasValue)
            {
                result[PatchEntryType.Download] = download.Value;
            }

            var encoding = GetEncodingPatchEntry();
            if (encoding.HasValue)
            {
                result[PatchEntryType.Encoding] = encoding.Value;
            }

            var size = GetSizePatchEntry();
            if (size.HasValue)
            {
                result[PatchEntryType.Size] = size.Value;
            }

            return result;
        }

        /// <summary>
        /// Parses a patch configuration from a stream.
        /// </summary>
        /// <param name="stream">The stream containing patch configuration data.</param>
        /// <returns>A new <see cref="PatchConfig"/> instance with all configuration values loaded.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stream"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// The patch configuration format is a simple key=value text format with comments starting with #.
        /// This file is typically downloaded from the CDN using the patch-config hash from the build config.
        /// </remarks>
        public static PatchConfig Parse(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var config = new PatchConfig();
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

                // Special handling for patch-entry keys - store all of them
                if (key.Equals("patch-entry", StringComparison.OrdinalIgnoreCase))
                {
                    config._patchEntries.Add(value);
                }
                else
                {
                    config._data[key] = value;
                }
            }

            return config;
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

        private static PatchEntry ParsePatchEntry(string value, PatchEntryType type)
        {
            var parts = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 6)
            {
                throw new InvalidOperationException($"Invalid patch-entry format for {type}. Expected at least 6 parts.");
            }

            // Format: <type> <content hash> <content size> <BLTE-encoding key> <BLTE-encoded size> <encoding string>
            // followed by sets of: <old BLTE-encoding key> <old content size> <patch hash> <patch size>
            var entry = new PatchEntry
            {
                Type = type,
                ContentHash = CascKey.Parse(parts[1]),
                ContentSize = long.Parse(parts[2], CultureInfo.InvariantCulture),
                EncodingKey = EKey.Parse(parts[3]),
                EncodedSize = long.Parse(parts[4], CultureInfo.InvariantCulture),
                EncodingString = parts[5],
                OldVersionPatches = new List<OldVersionPatch>(),
            };

            // Parse old version patches (sets of 4 values)
            for (var i = 6; i + 3 < parts.Length; i += 4)
            {
                entry.OldVersionPatches.Add(new OldVersionPatch
                {
                    OldEncodingKey = EKey.Parse(parts[i]),
                    OldContentSize = long.Parse(parts[i + 1], CultureInfo.InvariantCulture),
                    PatchHash = CascKey.Parse(parts[i + 2]),
                    PatchSize = long.Parse(parts[i + 3], CultureInfo.InvariantCulture),
                });
            }

            return entry;
        }
    }
}