// ------------------------------------------------------------------------------
// <copyright file="CompressionType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.IO.Casc.Enums
{
    /// <summary>
    /// Specifies the compression type for CASC data.
    /// </summary>
    public enum CompressionType : byte
    {
        /// <summary>
        /// No compression.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// ZLIB compression.
        /// </summary>
        ZLib = 0x5A, // 'Z'

        /// <summary>
        /// Encrypted data.
        /// </summary>
        Encrypted = 0x45, // 'E'

        /// <summary>
        /// Frame data (recursive).
        /// </summary>
        Frame = 0x46, // 'F'
    }
}