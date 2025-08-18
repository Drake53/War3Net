// ------------------------------------------------------------------------------
// <copyright file="CascStorageContext.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using War3Net.IO.Casc.Encoding;
using War3Net.IO.Casc.Enums;
using War3Net.IO.Casc.Index;
using War3Net.IO.Casc.Root;
using War3Net.IO.Casc.Structures;

namespace War3Net.IO.Casc.Storage
{
    /// <summary>
    /// Internal context for CASC storage.
    /// </summary>
    internal class CascStorageContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CascStorageContext"/> class.
        /// </summary>
        public CascStorageContext()
        {
            EncryptionKeys = new Dictionary<ulong, byte[]>();
        }

        /// <summary>
        /// Gets or sets the storage path.
        /// </summary>
        public string? StoragePath { get; set; }

        /// <summary>
        /// Gets or sets the data path.
        /// </summary>
        public string? DataPath { get; set; }

        /// <summary>
        /// Gets or sets the config path.
        /// </summary>
        public string? ConfigPath { get; set; }

        /// <summary>
        /// Gets or sets the build info.
        /// </summary>
        public BuildInfo? BuildInfo { get; set; }

        /// <summary>
        /// Gets or sets the active build.
        /// </summary>
        public BuildInfoEntry? ActiveBuild { get; set; }

        /// <summary>
        /// Gets or sets the index manager.
        /// </summary>
        public IndexManager? IndexManager { get; set; }

        /// <summary>
        /// Gets or sets the encoding file.
        /// </summary>
        public EncodingFile? EncodingFile { get; set; }

        /// <summary>
        /// Gets or sets the root handler.
        /// </summary>
        public IRootHandler? RootHandler { get; set; }

        /// <summary>
        /// Gets or sets the storage features.
        /// </summary>
        public CascFeatures Features { get; set; }

        /// <summary>
        /// Gets or sets the storage product.
        /// </summary>
        public CascStorageProduct? Product { get; set; }

        /// <summary>
        /// Gets or sets the locale flags.
        /// </summary>
        public CascLocaleFlags LocaleFlags { get; set; }

        /// <summary>
        /// Gets the encryption keys.
        /// </summary>
        public Dictionary<ulong, byte[]> EncryptionKeys { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the storage is online.
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// Gets or sets the CDN base URL.
        /// </summary>
        public string? CDNUrl { get; set; }
    }
}