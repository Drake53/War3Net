// ------------------------------------------------------------------------------
// <copyright file="CDNConfig.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace War3Net.IO.Casc.CDN
{
    /// <summary>
    /// Represents a CDN configuration file.
    /// </summary>
    public class CDNConfig
    {
        private readonly Dictionary<string, List<string>> _data;

        /// <summary>
        /// Initializes a new instance of the <see cref="CDNConfig"/> class.
        /// </summary>
        public CDNConfig()
        {
            _data = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the archive group.
        /// </summary>
        public string? ArchiveGroup => GetSingleValue("archive-group");

        /// <summary>
        /// Gets the archives.
        /// </summary>
        public List<string> Archives => GetValues("archives") ?? new List<string>();

        /// <summary>
        /// Gets the patch archives.
        /// </summary>
        public List<string> PatchArchives => GetValues("patch-archives") ?? new List<string>();

        /// <summary>
        /// Gets the file index.
        /// </summary>
        public string? FileIndex => GetSingleValue("file-index");

        /// <summary>
        /// Gets the file index size.
        /// </summary>
        public long FileIndexSize => GetSingleValueAsLong("file-index-size");

        /// <summary>
        /// Gets the patch file index.
        /// </summary>
        public string? PatchFileIndex => GetSingleValue("patch-file-index");

        /// <summary>
        /// Gets the patch file index size.
        /// </summary>
        public long PatchFileIndexSize => GetSingleValueAsLong("patch-file-index-size");

        /// <summary>
        /// Parses a CDN config from a stream.
        /// </summary>
        /// <param name="stream">The stream to parse.</param>
        /// <returns>The parsed CDN config.</returns>
        public static CDNConfig Parse(Stream stream)
        {
            var config = new CDNConfig();
            using var reader = new StreamReader(stream);

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                {
                    continue;
                }

                var equalIndex = line.IndexOf('=');
                if (equalIndex < 0)
                {
                    continue;
                }

                var key = line.Substring(0, equalIndex).Trim();
                var value = line.Substring(equalIndex + 1).Trim();

                if (!config._data.ContainsKey(key))
                {
                    config._data[key] = new List<string>();
                }

                // Handle multi-value fields (space-separated)
                if (key == "archives" || key == "patch-archives")
                {
                    var values = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    config._data[key].AddRange(values);
                }
                else
                {
                    config._data[key].Add(value);
                }
            }

            return config;
        }

        /// <summary>
        /// Gets a single value for a key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value, or null if not found.</returns>
        public string? GetSingleValue(string key)
        {
            return _data.TryGetValue(key, out var values) && values.Count > 0 ? values[0] : null;
        }

        /// <summary>
        /// Gets all values for a key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The values, or null if not found.</returns>
        public List<string>? GetValues(string key)
        {
            return _data.TryGetValue(key, out var values) ? values : null;
        }

        /// <summary>
        /// Gets a single value as a long.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value as a long, or 0 if not found or invalid.</returns>
        public long GetSingleValueAsLong(string key)
        {
            var value = GetSingleValue(key);
            return !string.IsNullOrEmpty(value) && long.TryParse(value, out var result) ? result : 0;
        }
    }
}