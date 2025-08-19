// ------------------------------------------------------------------------------
// <copyright file="VersionEntry.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

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
}