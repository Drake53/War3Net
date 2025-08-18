// ------------------------------------------------------------------------------
// <copyright file="CascProgressMessage.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.IO.Casc.Enums
{
    /// <summary>
    /// Specifies the type of progress message during CASC operations.
    /// </summary>
    public enum CascProgressMessage
    {
        /// <summary>
        /// Loading a file.
        /// </summary>
        LoadingFile,

        /// <summary>
        /// Loading a manifest.
        /// </summary>
        LoadingManifest,

        /// <summary>
        /// Downloading a file.
        /// </summary>
        DownloadingFile,

        /// <summary>
        /// Loading index files.
        /// </summary>
        LoadingIndexes,

        /// <summary>
        /// Downloading archive indexes.
        /// </summary>
        DownloadingArchiveIndexes,
    }
}