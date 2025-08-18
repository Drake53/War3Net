// ------------------------------------------------------------------------------
// <copyright file="CascFileSpanInfo.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.IO.Casc.Structures
{
    /// <summary>
    /// Contains information about a file span in CASC.
    /// </summary>
    public class CascFileSpanInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CascFileSpanInfo"/> class.
        /// </summary>
        public CascFileSpanInfo()
        {
            CKey = CascKey.Empty;
            EKey = EKey.Empty;
        }

        /// <summary>
        /// Gets or sets the content key of the file span.
        /// </summary>
        public CascKey CKey { get; set; }

        /// <summary>
        /// Gets or sets the encoded key of the file span.
        /// </summary>
        public EKey EKey { get; set; }

        /// <summary>
        /// Gets or sets the starting offset of the file span.
        /// </summary>
        public ulong StartOffset { get; set; }

        /// <summary>
        /// Gets or sets the ending offset of the file span.
        /// </summary>
        public ulong EndOffset { get; set; }

        /// <summary>
        /// Gets or sets the index of the archive.
        /// </summary>
        public uint ArchiveIndex { get; set; }

        /// <summary>
        /// Gets or sets the offset in the archive.
        /// </summary>
        public uint ArchiveOffset { get; set; }

        /// <summary>
        /// Gets or sets the size of encoded frame headers.
        /// </summary>
        public uint HeaderSize { get; set; }

        /// <summary>
        /// Gets or sets the number of frames in this span.
        /// </summary>
        public uint FrameCount { get; set; }

        /// <summary>
        /// Gets the size of the span.
        /// </summary>
        public ulong Size => EndOffset - StartOffset;
    }
}