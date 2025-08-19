// ------------------------------------------------------------------------------
// <copyright file="CdnServersEntry.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.IO.Casc.Cdn
{
    /// <summary>
    /// Represents a CDN servers configuration entry.
    /// </summary>
    public class CdnServersEntry
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

        /// <inheritdoc/>
        public override string ToString() => $"{Path} ({Region})";
    }
}