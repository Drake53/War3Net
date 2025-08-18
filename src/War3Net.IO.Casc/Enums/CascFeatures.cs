// ------------------------------------------------------------------------------
// <copyright file="CascFeatures.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.IO.Casc.Enums
{
    /// <summary>
    /// Feature flags for CASC storage.
    /// </summary>
    [Flags]
    public enum CascFeatures : uint
    {
        /// <summary>
        /// No features.
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// File names are supported by the storage.
        /// </summary>
        FileNames = 0x00000001,

        /// <summary>
        /// Storage's ROOT returns content key.
        /// </summary>
        RootCKey = 0x00000002,

        /// <summary>
        /// Tags are supported by the storage.
        /// </summary>
        Tags = 0x00000004,

        /// <summary>
        /// The storage contains file name hashes on ALL files.
        /// </summary>
        FileNameHashes = 0x00000008,

        /// <summary>
        /// The storage contains file name hashes for SOME files.
        /// </summary>
        FileNameHashesOptional = 0x00000010,

        /// <summary>
        /// The storage indexes files by FileDataId.
        /// </summary>
        FileDataIds = 0x00000020,

        /// <summary>
        /// Locale flags are supported.
        /// </summary>
        LocaleFlags = 0x00000040,

        /// <summary>
        /// Content flags are supported.
        /// </summary>
        ContentFlags = 0x00000080,

        /// <summary>
        /// The storage supports files stored in data.### archives.
        /// </summary>
        DataArchives = 0x00000100,

        /// <summary>
        /// The storage supports raw files stored in CascRoot\xx\yy\xxyy## (CKey-based).
        /// </summary>
        DataFiles = 0x00000200,

        /// <summary>
        /// Load missing files from online CDNs.
        /// </summary>
        Online = 0x00000400,

        /// <summary>
        /// Always download "versions" and "cdns" even if they exist locally.
        /// </summary>
        ForceDownload = 0x00001000,
    }
}