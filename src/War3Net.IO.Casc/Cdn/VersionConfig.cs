// ------------------------------------------------------------------------------
// <copyright file="VersionConfig.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace War3Net.IO.Casc.Cdn
{
    /// <summary>
    /// Represents a version configuration entry.
    /// </summary>
    public class VersionEntry
    {
        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        public string Region { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the build config hash.
        /// </summary>
        public string BuildConfig { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the CDN config hash.
        /// </summary>
        public string CdnConfig { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the build ID.
        /// </summary>
        public string BuildId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the version name.
        /// </summary>
        public string VersionsName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the product config.
        /// </summary>
        public string ProductConfig { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the key ring.
        /// </summary>
        public string KeyRing { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents a versions configuration file.
    /// </summary>
    public class VersionConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VersionConfig"/> class.
        /// </summary>
        public VersionConfig()
        {
            Entries = new List<VersionEntry>();
        }

        /// <summary>
        /// Gets the version entries.
        /// </summary>
        public List<VersionEntry> Entries { get; }

        /// <summary>
        /// Gets the column headers.
        /// </summary>
        public List<string> Headers { get; private set; } = new List<string>();

        /// <summary>
        /// Parses a version config from a stream.
        /// </summary>
        /// <param name="stream">The stream to parse.</param>
        /// <returns>The parsed version config.</returns>
        public static VersionConfig Parse(Stream stream)
        {
            var config = new VersionConfig();
            using var reader = new StreamReader(stream);

            string? line;
            var columnIndices = new Dictionary<string, int>();

            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                {
                    continue;
                }

                var parts = line.Split('|').Select(p => p.Trim()).ToArray();

                // First non-comment line contains headers
                if (config.Headers.Count == 0)
                {
                    config.Headers.AddRange(parts.Select(p => p.TrimEnd('!')));
                    for (int i = 0; i < config.Headers.Count; i++)
                    {
                        columnIndices[config.Headers[i]] = i;
                    }
                    continue;
                }

                // Parse data rows
                if (parts.Length >= config.Headers.Count)
                {
                    var entry = new VersionEntry();

                    if (columnIndices.TryGetValue("Region", out var idx))
                        entry.Region = parts[idx];
                    if (columnIndices.TryGetValue("BuildConfig", out idx))
                        entry.BuildConfig = parts[idx];
                    if (columnIndices.TryGetValue("CDNConfig", out idx))
                        entry.CdnConfig = parts[idx];
                    if (columnIndices.TryGetValue("BuildId", out idx))
                        entry.BuildId = parts[idx];
                    if (columnIndices.TryGetValue("VersionsName", out idx))
                        entry.VersionsName = parts[idx];
                    if (columnIndices.TryGetValue("ProductConfig", out idx))
                        entry.ProductConfig = parts[idx];
                    if (columnIndices.TryGetValue("KeyRing", out idx) && idx < parts.Length)
                        entry.KeyRing = parts[idx];

                    config.Entries.Add(entry);
                }
            }

            return config;
        }

        /// <summary>
        /// Gets the entry for a specific region.
        /// </summary>
        /// <param name="region">The region code.</param>
        /// <returns>The version entry, or null if not found.</returns>
        public VersionEntry? GetEntry(string region)
        {
            return Entries.FirstOrDefault(e => 
                string.Equals(e.Region, region, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets the first available entry.
        /// </summary>
        /// <returns>The first version entry, or null if empty.</returns>
        public VersionEntry? GetFirstEntry()
        {
            return Entries.FirstOrDefault();
        }
    }
}