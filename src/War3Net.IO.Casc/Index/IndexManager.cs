// ------------------------------------------------------------------------------
// <copyright file="IndexManager.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using War3Net.IO.Casc.Structures;

namespace War3Net.IO.Casc.Index
{
    /// <summary>
    /// Manages multiple index files for CASC storage.
    /// </summary>
    public class IndexManager
    {
        private readonly Dictionary<byte, IndexFile> _indexFiles;
        private readonly Dictionary<EKey, EKeyEntry> _globalIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexManager"/> class.
        /// </summary>
        public IndexManager()
        {
            _indexFiles = new Dictionary<byte, IndexFile>();
            _globalIndex = new Dictionary<EKey, EKeyEntry>();
        }

        /// <summary>
        /// Gets the number of loaded index files.
        /// </summary>
        public int IndexFileCount => _indexFiles.Count;

        /// <summary>
        /// Gets the total number of entries across all index files.
        /// </summary>
        public int TotalEntryCount => _globalIndex.Count;

        /// <summary>
        /// Loads index files from a directory.
        /// </summary>
        /// <param name="dataPath">The path to the data directory.</param>
        /// <returns>The number of index files loaded.</returns>
        public int LoadIndexFiles(string dataPath)
        {
            if (!Directory.Exists(dataPath))
            {
                throw new DirectoryNotFoundException($"Data directory not found: {dataPath}");
            }

            var indexFiles = Directory.GetFiles(dataPath, "*.idx", SearchOption.TopDirectoryOnly)
                .Where(f => IsValidIndexFileName(Path.GetFileName(f)))
                .OrderBy(f => f)
                .ToList();

            foreach (var indexFile in indexFiles)
            {
                try
                {
                    LoadIndexFile(indexFile);
                }
                catch (Exception ex)
                {
                    // Log error but continue loading other files
                    System.Diagnostics.Debug.WriteLine($"Failed to load index file {indexFile}: {ex.Message}");
                }
            }

            return _indexFiles.Count;
        }

        /// <summary>
        /// Loads a single index file.
        /// </summary>
        /// <param name="filePath">The path to the index file.</param>
        public void LoadIndexFile(string filePath)
        {
            var index = IndexFile.ParseFile(filePath);
            var bucketIndex = index.BucketIndex;

            // Add to bucket dictionary
            _indexFiles[bucketIndex] = index;

            // Add all entries to global index
            foreach (var entry in index.Entries)
            {
                _globalIndex[entry.EKey] = entry;
            }
        }

        /// <summary>
        /// Gets an index file by bucket index.
        /// </summary>
        /// <param name="bucketIndex">The bucket index.</param>
        /// <returns>The index file, or null if not found.</returns>
        public IndexFile? GetIndexFile(byte bucketIndex)
        {
            return _indexFiles.TryGetValue(bucketIndex, out var index) ? index : null;
        }

        /// <summary>
        /// Tries to find an entry by encoded key.
        /// </summary>
        /// <param name="ekey">The encoded key.</param>
        /// <param name="entry">The found entry.</param>
        /// <returns>true if the entry was found; otherwise, false.</returns>
        public bool TryFindEntry(EKey ekey, out EKeyEntry? entry)
        {
            // First check global index
            if (_globalIndex.TryGetValue(ekey, out entry))
            {
                return true;
            }

            // Calculate bucket index from first byte of EKey
            // According to CascLib reference, the bucket index is the first byte of the EKey
            var bucketIndex = ekey.Value.Length > 0 ? ekey.Value.Span[0] : (byte)0;

            // Check if we have that bucket's index file
            if (_indexFiles.TryGetValue(bucketIndex, out var indexFile))
            {
                if (indexFile.TryGetEntry(ekey, out entry))
                {
                    // Add to global index for faster future lookups
                    _globalIndex[ekey] = entry;
                    return true;
                }
            }

            entry = null;
            return false;
        }

        /// <summary>
        /// Finds an entry by encoded key.
        /// </summary>
        /// <param name="ekey">The encoded key.</param>
        /// <returns>The entry.</returns>
        public EKeyEntry FindEntry(EKey ekey)
        {
            if (!TryFindEntry(ekey, out var entry))
            {
                throw new CascFileNotFoundException(ekey);
            }

            return entry!;
        }

        /// <summary>
        /// Gets the data file path for an entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="dataPath">The base data path.</param>
        /// <returns>The full path to the data file.</returns>
        public static string GetDataFilePath(EKeyEntry entry, string dataPath)
        {
            return Path.Combine(dataPath, $"data.{entry.DataFileIndex:D3}");
        }

        /// <summary>
        /// Clears all loaded index files.
        /// </summary>
        public void Clear()
        {
            _indexFiles.Clear();
            _globalIndex.Clear();
        }

        private static bool IsValidIndexFileName(string fileName)
        {
            // Index files are named like: 00a1b2c3d4e5f6789.idx
            // 16 hex characters followed by .idx
            if (!fileName.EndsWith(".idx", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            if (nameWithoutExt.Length != 16)
            {
                return false;
            }

            return nameWithoutExt.All(c => char.IsDigit(c) || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F'));
        }
    }
}