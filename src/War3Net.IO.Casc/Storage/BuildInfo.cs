// ------------------------------------------------------------------------------
// <copyright file="BuildInfo.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace War3Net.IO.Casc.Storage
{
    /// <summary>
    /// Represents build information from .build.info file.
    /// </summary>
    public class BuildInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildInfo"/> class.
        /// </summary>
        public BuildInfo()
        {
            Entries = new List<BuildInfoEntry>();
        }

        /// <summary>
        /// Gets the list of build entries.
        /// </summary>
        public List<BuildInfoEntry> Entries { get; }

        /// <summary>
        /// Gets the column headers.
        /// </summary>
        public string[]? Headers { get; private set; }

        /// <summary>
        /// Parses a .build.info file.
        /// </summary>
        /// <param name="filePath">The path to the .build.info file.</param>
        /// <returns>The parsed build info.</returns>
        public static BuildInfo ParseFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            return Parse(lines);
        }

        /// <summary>
        /// Parses build info from text lines.
        /// </summary>
        /// <param name="lines">The text lines.</param>
        /// <returns>The parsed build info.</returns>
        public static BuildInfo Parse(string[] lines)
        {
            var buildInfo = new BuildInfo();

            if (lines.Length == 0)
            {
                return buildInfo;
            }

            // First line contains headers
            buildInfo.Headers = lines[0].Split('|').Select(h => h.Trim('!')).ToArray();

            // Parse entries
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    continue;
                }

                var values = lines[i].Split('|');
                var entry = new BuildInfoEntry();

                for (int j = 0; j < Math.Min(values.Length, buildInfo.Headers.Length); j++)
                {
                    entry.Values[buildInfo.Headers[j]] = values[j];
                }

                buildInfo.Entries.Add(entry);
            }

            return buildInfo;
        }

        /// <summary>
        /// Gets the active build entry.
        /// </summary>
        /// <param name="region">The region to filter by (optional).</param>
        /// <returns>The active build entry, or null if not found.</returns>
        public BuildInfoEntry? GetActiveBuild(string? region = null)
        {
            // Try to find build for specific region
            if (!string.IsNullOrEmpty(region))
            {
                var regionalBuild = Entries.FirstOrDefault(e =>
                    e.GetValue("Region") == region &&
                    e.GetValue("Active") == "1");

                if (regionalBuild != null)
                {
                    return regionalBuild;
                }
            }

            // Fall back to any active build
            return Entries.FirstOrDefault(e => e.GetValue("Active") == "1");
        }
    }

    /// <summary>
    /// Represents a single entry in the build info.
    /// </summary>
    public class BuildInfoEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildInfoEntry"/> class.
        /// </summary>
        public BuildInfoEntry()
        {
            Values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the dictionary of values.
        /// </summary>
        public Dictionary<string, string> Values { get; }

        /// <summary>
        /// Gets a value by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value, or null if not found.</returns>
        public string? GetValue(string key)
        {
            return Values.TryGetValue(key, out var value) ? value : null;
        }

        /// <summary>
        /// Gets the build key.
        /// </summary>
        public string? BuildKey => GetValue("Build Key");

        /// <summary>
        /// Gets the CDN key.
        /// </summary>
        public string? CdnKey => GetValue("CDN Key");

        /// <summary>
        /// Gets the product.
        /// </summary>
        public string? Product => GetValue("Product");

        /// <summary>
        /// Gets the version.
        /// </summary>
        public string? Version => GetValue("Version");

        /// <summary>
        /// Gets the region.
        /// </summary>
        public string? Region => GetValue("Region");

        /// <summary>
        /// Gets the build ID.
        /// </summary>
        public string? BuildId => GetValue("BuildId");

        /// <summary>
        /// Gets the CDN hosts.
        /// </summary>
        public string? CdnHosts => GetValue("CDN Hosts");

        /// <summary>
        /// Gets the CDN path.
        /// </summary>
        public string? CdnPath => GetValue("CDN Path");
    }
}