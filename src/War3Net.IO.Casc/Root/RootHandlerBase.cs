// ------------------------------------------------------------------------------
// <copyright file="RootHandlerBase.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;

namespace War3Net.IO.Casc.Root
{
    /// <summary>
    /// Base implementation of a CASC root handler.
    /// </summary>
    public abstract class RootHandlerBase : IRootHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RootHandlerBase"/> class.
        /// </summary>
        protected RootHandlerBase()
        {
            EntriesByName = new Dictionary<string, RootEntry>(StringComparer.OrdinalIgnoreCase);
            EntriesByFileDataId = new Dictionary<uint, RootEntry>();
            FileDataIdToName = new Dictionary<uint, string>();
        }

        /// <summary>
        /// Gets the entries indexed by name.
        /// </summary>
        protected Dictionary<string, RootEntry> EntriesByName { get; }

        /// <summary>
        /// Gets the entries indexed by file data ID.
        /// </summary>
        protected Dictionary<uint, RootEntry> EntriesByFileDataId { get; }

        /// <summary>
        /// Gets the file data ID to name mapping.
        /// </summary>
        protected Dictionary<uint, string> FileDataIdToName { get; }

        /// <inheritdoc/>
        public virtual int FileCount => EntriesByName.Count + EntriesByFileDataId.Count;

        /// <inheritdoc/>
        public virtual int LocaleCount { get; protected set; }

        /// <inheritdoc/>
        public abstract void Parse(Stream stream);

        /// <inheritdoc/>
        public virtual RootEntry? GetEntry(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            // Normalize path separators
            fileName = fileName.Replace('/', '\\');

            return EntriesByName.TryGetValue(fileName, out var entry) ? entry : null;
        }

        /// <inheritdoc/>
        public virtual RootEntry? GetEntry(uint fileDataId)
        {
            return EntriesByFileDataId.TryGetValue(fileDataId, out var entry) ? entry : null;
        }

        /// <inheritdoc/>
        public virtual IEnumerable<RootEntry> GetEntries()
        {
            var entries = new HashSet<RootEntry>();

            foreach (var entry in EntriesByName.Values)
            {
                entries.Add(entry);
            }

            foreach (var entry in EntriesByFileDataId.Values)
            {
                entries.Add(entry);
            }

            return entries;
        }

        /// <inheritdoc/>
        public virtual IEnumerable<string> GetFileNames()
        {
            return EntriesByName.Keys;
        }

        /// <inheritdoc/>
        public virtual void AddFileName(uint fileDataId, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            FileDataIdToName[fileDataId] = fileName;

            // Update existing entry if found
            if (EntriesByFileDataId.TryGetValue(fileDataId, out var entry))
            {
                entry.FileName = fileName;

                // Also add to name index
                EntriesByName[fileName] = entry;
            }
        }

        /// <inheritdoc/>
        public virtual int LoadListFile(string listFile)
        {
            if (string.IsNullOrEmpty(listFile))
            {
                return 0;
            }

            var lines = listFile.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var count = 0;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#", StringComparison.Ordinal) || line.StartsWith(";", StringComparison.Ordinal))
                {
                    continue;
                }

                // Some list files have format: FileDataId;FileName
                var parts = line.Split(';');
                if (parts.Length == 2 && uint.TryParse(parts[0], out var fileDataId))
                {
                    AddFileName(fileDataId, parts[1].Trim());
                    count++;
                }
                else
                {
                    // Try to match with existing entries by file name
                    var fileName = line.Trim();
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        // This would need the file name hash to work properly
                        // For now, just store it for future reference
                        count++;
                    }
                }
            }

            return count;
        }

        /// <inheritdoc/>
        public virtual void Clear()
        {
            EntriesByName.Clear();
            EntriesByFileDataId.Clear();
            FileDataIdToName.Clear();
        }

        /// <summary>
        /// Adds an entry to the root handler.
        /// </summary>
        /// <param name="entry">The entry to add.</param>
        protected void AddEntry(RootEntry entry)
        {
            if (entry.HasFileName)
            {
                EntriesByName[entry.FileName] = entry;
            }

            if (entry.HasFileDataId)
            {
                EntriesByFileDataId[entry.FileDataId] = entry;

                // Check if we have a name mapping
                if (FileDataIdToName.TryGetValue(entry.FileDataId, out var fileName))
                {
                    entry.FileName = fileName;
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        EntriesByName[fileName] = entry;
                    }
                }
            }
        }
    }
}