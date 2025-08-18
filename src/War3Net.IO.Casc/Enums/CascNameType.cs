// ------------------------------------------------------------------------------
// <copyright file="CascNameType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.IO.Casc.Enums
{
    /// <summary>
    /// Specifies the type of name returned for a CASC file.
    /// </summary>
    public enum CascNameType
    {
        /// <summary>
        /// Fully qualified file name.
        /// </summary>
        Full,

        /// <summary>
        /// Name created from file data ID.
        /// </summary>
        DataId,

        /// <summary>
        /// Name created as string representation of content key.
        /// </summary>
        CKey,

        /// <summary>
        /// Name created as string representation of encoded key.
        /// </summary>
        EKey,
    }
}