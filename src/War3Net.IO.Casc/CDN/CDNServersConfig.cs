// ------------------------------------------------------------------------------
// <copyright file="CDNServersConfig.cs" company="Drake53">
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
    /// Represents a CDN servers configuration entry.
    /// </summary>
    public class CDNServersEntry
    {
        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        public string Region { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the CDN path.
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the CDN hosts.
        /// </summary>
        public List<string> Hosts { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the servers.
        /// </summary>
        public List<string> Servers { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the config path.
        /// </summary>
        public string ConfigPath { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents a CDN servers configuration file.
    /// </summary>
    public class CDNServersConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CDNServersConfig"/> class.
        /// </summary>
        public CDNServersConfig()
        {
            Entries = new List<CDNServersEntry>();
        }

        /// <summary>
        /// Gets the CDN server entries.
        /// </summary>
        public List<CDNServersEntry> Entries { get; }

        /// <summary>
        /// Gets the column headers.
        /// </summary>
        public List<string> Headers { get; private set; } = new List<string>();

        /// <summary>
        /// Parses a CDN servers config from a stream.
        /// </summary>
        /// <param name="stream">The stream to parse.</param>
        /// <returns>The parsed CDN servers config.</returns>
        public static CDNServersConfig Parse(Stream stream)
        {
            var config = new CDNServersConfig();
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
                    var entry = new CDNServersEntry();

                    if (columnIndices.TryGetValue("Name", out var idx))
                        entry.Region = parts[idx];
                    if (columnIndices.TryGetValue("Path", out idx))
                        entry.Path = parts[idx];
                    if (columnIndices.TryGetValue("Hosts", out idx))
                    {
                        var hosts = parts[idx].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        entry.Hosts.AddRange(hosts.Select(h => $"http://{h}"));
                    }
                    if (columnIndices.TryGetValue("Servers", out idx) && idx < parts.Length && !string.IsNullOrWhiteSpace(parts[idx]))
                    {
                        var servers = parts[idx].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        entry.Servers.AddRange(servers);
                    }
                    if (columnIndices.TryGetValue("ConfigPath", out idx) && idx < parts.Length)
                        entry.ConfigPath = parts[idx];

                    config.Entries.Add(entry);
                }
            }

            return config;
        }

        /// <summary>
        /// Gets the entry for a specific region.
        /// </summary>
        /// <param name="region">The region code.</param>
        /// <returns>The CDN servers entry, or null if not found.</returns>
        public CDNServersEntry? GetEntry(string region)
        {
            return Entries.FirstOrDefault(e => 
                string.Equals(e.Region, region, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets the first available entry.
        /// </summary>
        /// <returns>The first CDN servers entry, or null if empty.</returns>
        public CDNServersEntry? GetFirstEntry()
        {
            return Entries.FirstOrDefault();
        }
    }
}