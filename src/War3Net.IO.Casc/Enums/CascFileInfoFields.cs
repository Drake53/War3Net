// ------------------------------------------------------------------------------
// <copyright file="CascFileInfoFields.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.IO.Casc.Enums
{
    /// <summary>
    /// Specifies the type of information to retrieve about a CASC file.
    /// </summary>
    [Flags]
    public enum CascFileInfoFields
    {
        /// <summary>
        /// Returns the content key of the file.
        /// </summary>
        ContentKey = 1 << 0,

        /// <summary>
        /// Returns the encoded key of the file.
        /// </summary>
        EncodedKey = 1 << 1,

        DataFileName = 1 << 2,

        FileNameHash = 1 << 3,

        StorageOffset = 1 << 4,

        SegmentOffset = 1 << 5,

        SegmentIndex = 1 << 6,

        FileDataId = 1 << 7,

        ContentSize = 1 << 8,

        EncodedSize = 1 << 9,

        NumberOfSpans = 1 << 10,

        LocaleFlags = 1 << 11,

        ContentFlags = 1 << 12,

        AvailableLocally = 1 << 13,

        Tags = 1 << 14,

        All = ContentKey
            | EncodedKey
            | DataFileName
            | FileNameHash
            | StorageOffset
            | SegmentOffset
            | SegmentIndex
            | FileDataId
            | ContentSize
            | EncodedSize
            | NumberOfSpans
            | LocaleFlags
            | ContentFlags
            | AvailableLocally
            | Tags,
    }
}