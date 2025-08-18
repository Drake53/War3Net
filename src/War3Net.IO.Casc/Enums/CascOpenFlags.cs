// ------------------------------------------------------------------------------
// <copyright file="CascOpenFlags.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.IO.Casc.Enums
{
    /// <summary>
    /// Flags for opening CASC files.
    /// </summary>
    [Flags]
    public enum CascOpenFlags : uint
    {
        /// <summary>
        /// Open the file by name. This is the default value.
        /// </summary>
        OpenByName = 0x00000000,

        /// <summary>
        /// The name is just the content key; skip ROOT file processing.
        /// </summary>
        OpenByCKey = 0x00000001,

        /// <summary>
        /// The name is just the encoded key; skip ROOT file processing.
        /// </summary>
        OpenByEKey = 0x00000002,

        /// <summary>
        /// The name is a file data ID.
        /// </summary>
        OpenByFileId = 0x00000003,

        /// <summary>
        /// Mask to get open type from flags.
        /// </summary>
        OpenTypeMask = 0x0000000F,

        /// <summary>
        /// Mask to get flags from open type.
        /// </summary>
        OpenFlagsMask = 0xFFFFFFF0,

        /// <summary>
        /// Verify all data read from a file.
        /// </summary>
        StrictDataCheck = 0x00000010,

        /// <summary>
        /// When encountering an encrypted block with missing key, fill with zeros and return success.
        /// </summary>
        OvercomeEncrypted = 0x00000020,

        /// <summary>
        /// Only open a file with given CKey once, regardless of how many names it has.
        /// </summary>
        OpenCKeyOnce = 0x00000040,
    }
}