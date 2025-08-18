// ------------------------------------------------------------------------------
// <copyright file="CascFileFullInfo.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.IO.Casc.Structures
{
    /// <summary>
    /// Contains complete information about a CASC file.
    /// </summary>
    public class CascFileFullInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CascFileFullInfo"/> class.
        /// </summary>
        public CascFileFullInfo()
        {
            CKey = CascKey.Empty;
            EKey = EKey.Empty;
            DataFileName = string.Empty;
            FileDataId = CascConstants.InvalidId;
            LocaleFlags = CascConstants.InvalidId;
            ContentFlags = CascConstants.InvalidId;
        }

        /// <summary>
        /// Gets or sets the content key.
        /// </summary>
        public CascKey CKey { get; set; }

        /// <summary>
        /// Gets or sets the encoded key.
        /// </summary>
        public EKey EKey { get; set; }

        /// <summary>
        /// Gets or sets the plain name of the data file where the file is stored.
        /// </summary>
        public string DataFileName { get; set; }

        /// <summary>
        /// Gets or sets the offset of the file over the entire storage.
        /// </summary>
        public ulong StorageOffset { get; set; }

        /// <summary>
        /// Gets or sets the offset of the file in the segment file.
        /// </summary>
        public ulong SegmentOffset { get; set; }

        /// <summary>
        /// Gets or sets the bitmask of tags.
        /// </summary>
        public ulong TagBitMask { get; set; }

        /// <summary>
        /// Gets or sets the hash of the file name.
        /// </summary>
        public ulong FileNameHash { get; set; }

        /// <summary>
        /// Gets or sets the content size of all spans.
        /// </summary>
        public ulong ContentSize { get; set; }

        /// <summary>
        /// Gets or sets the encoded size of all spans.
        /// </summary>
        public ulong EncodedSize { get; set; }

        /// <summary>
        /// Gets or sets the index of the segment file.
        /// </summary>
        public uint SegmentIndex { get; set; }

        /// <summary>
        /// Gets or sets the number of spans forming the file.
        /// </summary>
        public uint SpanCount { get; set; }

        /// <summary>
        /// Gets or sets the file data ID.
        /// </summary>
        public uint FileDataId { get; set; }

        /// <summary>
        /// Gets or sets the locale flags.
        /// </summary>
        public uint LocaleFlags { get; set; }

        /// <summary>
        /// Gets or sets the content flags.
        /// </summary>
        public uint ContentFlags { get; set; }
    }
}