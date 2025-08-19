// ------------------------------------------------------------------------------
// <copyright file="CdnConfig.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

namespace War3Net.IO.Casc.Cdn
{
    /// <summary>
    /// Represents a CDN configuration file.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The CDN config file contains information about how data is organized on the CDN for a specific build.
    /// It lists archives, indexes, and other data structures needed to retrieve game files from the CDN.
    /// This configuration is used by <see cref="Storage.OnlineCascStorage"/> to locate and download game content.
    /// </para>
    /// <para>
    /// Key fields from the TACT specification:
    /// </para>
    /// <list type="bullet">
    /// <item><description>archives: <see cref="Structures.EKey"/>s of all archives (append .index to get their <see cref="Index.IndexFile"/>s)</description></item>
    /// <item><description>archive-group: <see cref="Structures.EKey"/> of the combined index file (assembled client-side from all archives)</description></item>
    /// <item><description>file-index: <see cref="Structures.EKey"/> of .index file for unarchived/loose files (0-byte archive offset fields)</description></item>
    /// <item><description>file-index-size: Size of the unarchived file index</description></item>
    /// <item><description>patch-archives: <see cref="Structures.EKey"/>s of patch archives containing ZBSDIFF1 patch blobs</description></item>
    /// <item><description>patch-archive-group: <see cref="Structures.EKey"/> of the combined patch index file</description></item>
    /// <item><description>patch-file-index: <see cref="Structures.EKey"/> of .index file for unarchived patches</description></item>
    /// <item><description>patch-file-index-size: Size of unarchived patch index</description></item>
    /// <item><description>builds: List of <see cref="BuildConfig"/>s this CDN config supports (optional)</description></item>
    /// </list>
    /// <para>
    /// Archives are 256 MB binary files containing <see cref="Compression.BlteDecoder"/>-encoded file fragments.
    /// The archive-group index is special - it doesn't exist on the CDN but is assembled client-side by combining
    /// all individual archive <see cref="Index.IndexFile"/>s for efficient lookup across all archives.
    /// </para>
    /// <para>
    /// This file is referenced via its hash in the version info and is essential for <see cref="Storage.OnlineCascStorage"/>
    /// to understand how to retrieve files from the CDN.
    /// </para>
    /// </remarks>
    public class CdnConfig : ConfigBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CdnConfig"/> class.
        /// </summary>
        public CdnConfig()
            : base()
        {
        }

        /// <summary>
        /// Gets the list of archive hashes.
        /// </summary>
        /// <value>A list of <see cref="Structures.EKey"/> strings for all archive files available on the CDN.</value>
        /// <remarks>
        /// Each archive hash can be used to download both the archive file (.data) and its corresponding
        /// <see cref="Index.IndexFile"/> (.index) from the CDN. Archives contain <see cref="Compression.BlteDecoder"/>-encoded game files.
        /// </remarks>
        /// <exception cref="KeyNotFoundException">Thrown when the 'archives' key is not found in the configuration.</exception>
        public List<string> Archives => ParseMultiValue(GetRequiredValue("archives"));

        /// <summary>
        /// Gets the archive index sizes.
        /// </summary>
        /// <value>An array of sizes corresponding to each archive's index file.</value>
        /// <remarks>
        /// These sizes correspond to the .index files for each archive listed in <see cref="Archives"/>.
        /// </remarks>
        public long[] ArchiveIndexSizes => ParseSizes(GetOptionalValue("archives-index-size"));

        /// <summary>
        /// Gets the archive group index hash.
        /// </summary>
        /// <value>The <see cref="Structures.EKey"/> for the combined archive index.</value>
        /// <remarks>
        /// The archive group is a virtual index created by combining all individual archive <see cref="Index.IndexFile"/>s.
        /// This allows <see cref="Storage.OnlineCascStorage"/> to efficiently search across all archives.
        /// </remarks>
        /// <exception cref="KeyNotFoundException">Thrown when the 'archive-group' key is not found in the configuration.</exception>
        public string ArchiveGroup => GetRequiredValue("archive-group");

        /// <summary>
        /// Gets the list of patch archive hashes.
        /// </summary>
        /// <value>A list of <see cref="Structures.EKey"/> strings for patch archive files, or an empty list if none are specified.</value>
        /// <remarks>
        /// Patch archives contain differential updates (ZBSDIFF1 format) that can be applied to base files
        /// to create updated versions. Used for incremental game updates.
        /// </remarks>
        public List<string> PatchArchives => ParseMultiValue(GetOptionalValue("patch-archives"));

        /// <summary>
        /// Gets the patch archive index sizes.
        /// </summary>
        /// <value>An array of sizes corresponding to each patch archive's index file.</value>
        /// <remarks>
        /// These sizes correspond to the .index files for each patch archive listed in <see cref="PatchArchives"/>.
        /// </remarks>
        public long[] PatchArchiveIndexSizes => ParseSizes(GetOptionalValue("patch-archives-index-size"));

        /// <summary>
        /// Gets the patch archive group hash.
        /// </summary>
        /// <value>The <see cref="Structures.EKey"/> for the combined patch archive index, or <see langword="null"/> if not specified.</value>
        /// <remarks>
        /// Similar to <see cref="ArchiveGroup"/>, this is a virtual index combining all patch archive indices.
        /// </remarks>
        public string? PatchArchiveGroup => GetOptionalValue("patch-archive-group");

        /// <summary>
        /// Gets the file index hash for unarchived files.
        /// </summary>
        /// <value>The <see cref="Structures.EKey"/> for the loose file index.</value>
        /// <remarks>
        /// The file index contains <see cref="Structures.EKey"/> to location mappings for files that are stored
        /// as individual files on the CDN rather than within archives. This is commonly used for frequently
        /// accessed files like configuration files and root handlers such as <see cref="Root.TvfsRootHandler"/>.
        /// </remarks>
        /// <exception cref="KeyNotFoundException">Thrown when the 'file-index' key is not found in the configuration.</exception>
        public string FileIndex => GetRequiredValue("file-index");

        /// <summary>
        /// Gets the file index size.
        /// </summary>
        /// <exception cref="KeyNotFoundException">Thrown when the 'file-index-size' key is not found in the configuration.</exception>
        public long FileIndexSize => ParseSize(GetRequiredValue("file-index-size"));

        /// <summary>
        /// Gets the patch file index.
        /// </summary>
        /// <value>The <see cref="Structures.EKey"/> for the patch file index, or <see langword="null"/> if not specified.</value>
        /// <remarks>
        /// Contains index information for unarchived patch files.
        /// </remarks>
        public string? PatchFileIndex => GetOptionalValue("patch-file-index");

        /// <summary>
        /// Gets the patch file index size.
        /// </summary>
        public long PatchFileIndexSize => ParseSize(GetOptionalValue("patch-file-index-size"));

        /// <summary>
        /// Parses a CDN configuration from a stream.
        /// </summary>
        /// <param name="stream">The stream containing CDN configuration data.</param>
        /// <returns>A new <see cref="CdnConfig"/> instance with all configuration values loaded.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stream"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// The CDN configuration format is a simple key=value text format with comments starting with #.
        /// Multi-value fields like archives are space-separated. This file is typically downloaded by
        /// <see cref="Storage.OnlineCascStorage"/> from version configuration servers.
        /// </remarks>
        public static CdnConfig Parse(Stream stream)
        {
            return ParseConfig(stream, new CdnConfig());
        }
    }
}