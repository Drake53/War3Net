// ------------------------------------------------------------------------------
// <copyright file="CascFileInfoClass.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.IO.Casc.Enums
{
    /// <summary>
    /// Specifies the type of information to retrieve about a CASC file.
    /// </summary>
    public enum CascFileInfoClass
    {
        /// <summary>
        /// Returns the content key of the file.
        /// </summary>
        ContentKey,

        /// <summary>
        /// Returns the encoded key of the file.
        /// </summary>
        EncodedKey,

        /// <summary>
        /// Returns full file information.
        /// </summary>
        FullInfo,

        /// <summary>
        /// Returns span information for multi-part files.
        /// </summary>
        SpanInfo,
    }
}