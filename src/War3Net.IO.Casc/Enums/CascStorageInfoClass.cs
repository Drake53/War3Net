// ------------------------------------------------------------------------------
// <copyright file="CascStorageInfoClass.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.IO.Casc.Enums
{
    /// <summary>
    /// Specifies the type of information to retrieve about a CASC storage.
    /// </summary>
    public enum CascStorageInfoClass
    {
        /// <summary>
        /// Returns the number of local files in the storage.
        /// </summary>
        LocalFileCount,

        /// <summary>
        /// Returns the total file count, including offline files.
        /// </summary>
        TotalFileCount,

        /// <summary>
        /// Returns the features flag.
        /// </summary>
        Features,

        /// <summary>
        /// Returns installed locales. Not supported.
        /// </summary>
        InstalledLocales,

        /// <summary>
        /// Returns CASC_STORAGE_PRODUCT structure.
        /// </summary>
        Product,

        /// <summary>
        /// Returns CASC_STORAGE_TAGS structure.
        /// </summary>
        Tags,

        /// <summary>
        /// Returns Path:Product as a string.
        /// </summary>
        PathProduct,
    }
}