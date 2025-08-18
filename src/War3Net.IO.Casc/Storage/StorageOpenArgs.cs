// ------------------------------------------------------------------------------
// <copyright file="StorageOpenArgs.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.IO.Casc.Enums;
using War3Net.IO.Casc.Progress;

namespace War3Net.IO.Casc.Storage
{
    /// <summary>
    /// Arguments for opening CASC storage with advanced options.
    /// </summary>
    public class StorageOpenArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageOpenArgs"/> class.
        /// </summary>
        public StorageOpenArgs()
        {
            LocaleFlags = CascLocaleFlags.All;
            Features = CascFeatures.None;
        }

        /// <summary>
        /// Gets or sets the local storage path.
        /// </summary>
        public string? LocalPath { get; set; }

        /// <summary>
        /// Gets or sets the product code name.
        /// </summary>
        public string? CodeName { get; set; }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        public string? Region { get; set; }

        /// <summary>
        /// Gets or sets the progress reporter.
        /// </summary>
        public IProgressReporter? ProgressReporter { get; set; }

        /// <summary>
        /// Gets or sets the locale flags.
        /// </summary>
        public CascLocaleFlags LocaleFlags { get; set; }

        /// <summary>
        /// Gets or sets additional features to enable.
        /// </summary>
        public CascFeatures Features { get; set; }

        /// <summary>
        /// Gets or sets the build key for a specific version.
        /// </summary>
        public string? BuildKey { get; set; }

        /// <summary>
        /// Gets or sets the CDN host URL.
        /// </summary>
        public string? CdnHostUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use online storage.
        /// </summary>
        public bool OnlineStorage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to force download of version files.
        /// </summary>
        public bool ForceDownload { get; set; }

        /// <summary>
        /// Gets or sets the encryption key file path.
        /// </summary>
        public string? KeyFilePath { get; set; }

        /// <summary>
        /// Gets or sets the list file path for name resolution.
        /// </summary>
        public string? ListFilePath { get; set; }
    }
}