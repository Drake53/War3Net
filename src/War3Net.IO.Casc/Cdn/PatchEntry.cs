// ------------------------------------------------------------------------------
// <copyright file="PatchEntry.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using War3Net.IO.Casc.Enums;

using War3Net.IO.Casc.Structures;

namespace War3Net.IO.Casc.Cdn
{
    /// <summary>
    /// Represents a patch entry for a specific file type.
    /// </summary>
    public struct PatchEntry
    {
        /// <summary>
        /// Gets or sets the type of file this patch entry is for.
        /// </summary>
        public PatchEntryType Type { get; set; }

        /// <summary>
        /// Gets or sets the content hash of the target file.
        /// </summary>
        public CascKey ContentHash { get; set; }

        /// <summary>
        /// Gets or sets the size of the uncompressed content.
        /// </summary>
        public long ContentSize { get; set; }

        /// <summary>
        /// Gets or sets the BLTE-encoding key of the target file.
        /// </summary>
        public EKey EncodingKey { get; set; }

        /// <summary>
        /// Gets or sets the size of the BLTE-encoded file.
        /// </summary>
        public long EncodedSize { get; set; }

        /// <summary>
        /// Gets or sets the encoding specification string.
        /// </summary>
        public string EncodingString { get; set; }

        /// <summary>
        /// Gets or sets the list of patches from old versions.
        /// </summary>
        /// <remarks>
        /// Sorted by oldest build to newest. This serves to pair old files to the patch
        /// needed to bring them up to date, as well as providing the information required
        /// to detect if the file is already up to date.
        /// </remarks>
        public List<OldVersionPatch> OldVersionPatches { get; set; }
    }
}