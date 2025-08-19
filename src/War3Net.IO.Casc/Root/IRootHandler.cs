// ------------------------------------------------------------------------------
// <copyright file="IRootHandler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

namespace War3Net.IO.Casc.Root
{
    /// <summary>
    /// Interface for CASC root file handlers.
    /// </summary>
    public interface IRootHandler
    {
        /// <summary>
        /// Gets the number of files in the root.
        /// </summary>
        int FileCount { get; }

        /// <summary>
        /// Gets the number of locale flags supported.
        /// </summary>
        int LocaleCount { get; }

        /// <summary>
        /// Parses the root file from a stream.
        /// </summary>
        /// <param name="stream">The stream containing the root file data.</param>
        void Parse(Stream stream);

        /// <summary>
        /// Gets a file entry by name.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The root entry, or null if not found.</returns>
        RootEntry? GetEntry(string fileName);

        /// <summary>
        /// Gets a file entry by file data ID.
        /// </summary>
        /// <param name="fileDataId">The file data ID.</param>
        /// <returns>The root entry, or null if not found.</returns>
        RootEntry? GetEntry(uint fileDataId);

        /// <summary>
        /// Gets all file entries.
        /// </summary>
        /// <returns>An enumerable of all root entries.</returns>
        IEnumerable<RootEntry> GetEntries();

        /// <summary>
        /// Gets all file names.
        /// </summary>
        /// <returns>An enumerable of all file names.</returns>
        IEnumerable<string> GetFileNames();

        /// <summary>
        /// Adds a file name mapping.
        /// </summary>
        /// <param name="fileDataId">The file data ID.</param>
        /// <param name="fileName">The file name.</param>
        void AddFileName(uint fileDataId, string fileName);

        /// <summary>
        /// Loads file names from a list file.
        /// </summary>
        /// <param name="listFile">The list file content.</param>
        /// <returns>The number of file names loaded.</returns>
        int LoadListFile(string listFile);

        /// <summary>
        /// Clears all entries.
        /// </summary>
        void Clear();
    }
}