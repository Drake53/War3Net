// ------------------------------------------------------------------------------
// <copyright file="CascFindData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.IO.Casc.Enums;

namespace War3Net.IO.Casc.Structures
{
    /// <summary>
    /// Contains information about a file found in CASC storage.
    /// </summary>
    public class CascFindData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CascFindData"/> class.
        /// </summary>
        public CascFindData()
        {
            FileName = string.Empty;
            PlainName = string.Empty;
            CKey = CascKey.Empty;
            EKey = EKey.Empty;
            FileDataId = CascConstants.InvalidId;
            LocaleFlags = CascConstants.InvalidId;
            ContentFlags = CascConstants.InvalidId;
        }

        /// <summary>
        /// Gets or sets the full name of the found file.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the plain name of the found file (without path).
        /// </summary>
        public string PlainName { get; set; }

        /// <summary>
        /// Gets or sets the content key.
        /// </summary>
        public CascKey CKey { get; set; }

        /// <summary>
        /// Gets or sets the encoded key.
        /// </summary>
        public EKey EKey { get; set; }

        /// <summary>
        /// Gets or sets the tag bitmask.
        /// </summary>
        public ulong TagBitMask { get; set; }

        /// <summary>
        /// Gets or sets the file size.
        /// </summary>
        public ulong FileSize { get; set; }

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

        /// <summary>
        /// Gets or sets the span count for multi-part files.
        /// </summary>
        public uint SpanCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the file is available locally.
        /// </summary>
        public bool FileAvailable { get; set; }

        /// <summary>
        /// Gets or sets the name type.
        /// </summary>
        public CascNameType NameType { get; set; }
    }
}