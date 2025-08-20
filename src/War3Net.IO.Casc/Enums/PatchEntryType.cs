// ------------------------------------------------------------------------------
// <copyright file="PatchEntryType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.IO.Casc.Enums
{
    /// <summary>
    /// Specifies the type of file a patch entry is for.
    /// </summary>
    public enum PatchEntryType
    {
        /// <summary>
        /// Patch entry for the install manifest file.
        /// </summary>
        Install,

        /// <summary>
        /// Patch entry for the download manifest file.
        /// </summary>
        Download,

        /// <summary>
        /// Patch entry for the encoding file.
        /// </summary>
        Encoding,

        /// <summary>
        /// Patch entry for the size file.
        /// </summary>
        Size,
    }
}