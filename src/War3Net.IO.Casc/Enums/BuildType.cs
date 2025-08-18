// ------------------------------------------------------------------------------
// <copyright file="BuildType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.IO.Casc.Enums
{
    /// <summary>
    /// Specifies the type of build file.
    /// </summary>
    public enum BuildType
    {
        /// <summary>
        /// No build type found.
        /// </summary>
        None = 0,

        /// <summary>
        /// .build.db file (older storages).
        /// </summary>
        BuildDb,

        /// <summary>
        /// .build.info file.
        /// </summary>
        BuildInfo,

        /// <summary>
        /// versions file (cached or online).
        /// </summary>
        Versions,
    }
}