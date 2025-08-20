// ------------------------------------------------------------------------------
// <copyright file="OldVersionPatch.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.IO.Casc.Structures;

namespace War3Net.IO.Casc.Cdn
{
    /// <summary>
    /// Represents a patch from an old version to the current version.
    /// </summary>
    public struct OldVersionPatch
    {
        /// <summary>
        /// Gets or sets the old BLTE-encoding key.
        /// </summary>
        public EKey OldEncodingKey { get; set; }

        /// <summary>
        /// Gets or sets the old content size.
        /// </summary>
        public long OldContentSize { get; set; }

        /// <summary>
        /// Gets or sets the hash of the patch file.
        /// </summary>
        /// <remarks>
        /// These patches are stored externally to the Patch file (which solely handles game asset patching)
        /// and are stored directly on Blizzard's CDN. These are raw ZBSDIFF1 blobs applied to the
        /// BLTE decoded contents of each file.
        /// </remarks>
        public CascKey PatchHash { get; set; }

        /// <summary>
        /// Gets or sets the size of the patch file.
        /// </summary>
        public long PatchSize { get; set; }
    }
}