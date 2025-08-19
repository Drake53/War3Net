// ------------------------------------------------------------------------------
// <copyright file="IndexFile.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using War3Net.IO.Casc.Structures;

namespace War3Net.IO.Casc.Index
{
    /// <summary>
    /// Represents a CASC index file (.idx).
    /// </summary>
    /// <remarks>
    /// <para>
    /// Index files (.idx) are storage structures used by the CASC system to map <see cref="EKey"/>s (Encoding Keys)
    /// to their locations in archive files. These can be either local index files for local archives or
    /// CDN index files downloaded from the CDN by <see cref="Storage.OnlineCascStorage"/>.
    /// </para>
    /// <para>
    /// Local index files are organized into buckets (16 files, numbered 00-0F) to distribute entries
    /// and improve lookup performance. Each entry maps an <see cref="EKey"/> to an archive number, offset within
    /// that archive, and the size of the encoded <see cref="Compression.BlteDecoder"/> data.
    /// </para>
    /// <para>
    /// The index file format has evolved through multiple versions:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Version 1: Original format with basic header and entry structure</description></item>
    /// <item><description>Version 2: Enhanced format with additional metadata and padding</description></item>
    /// </list>
    /// <para>
    /// These files are critical for the CASC storage to quickly locate and retrieve game data.
    /// Local installations store them in the Data/indices/ directory, while <see cref="Storage.OnlineCascStorage"/>
    /// downloads them from the CDN based on <see cref="Cdn.CdnConfig"/> archive lists.
    /// </para>
    /// <para>
    /// The index file works in conjunction with the <see cref="Encoding.EncodingFile"/> - the encoding file
    /// maps <see cref="CascKey"/>s to <see cref="EKey"/>s, and the index file maps those <see cref="EKey"/>s
    /// to physical archive locations.
    /// </para>
    /// </remarks>
    public class IndexFile
    {
        private readonly Dictionary<EKey, EKeyEntry> _entries;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexFile"/> class.
        /// </summary>
        public IndexFile()
        {
            _entries = new Dictionary<EKey, EKeyEntry>();
        }

        /// <summary>
        /// Gets the index header containing format and structure information.
        /// </summary>
        /// <value>The parsed <see cref="IndexHeader"/> if available, otherwise <see langword="null"/>.</value>
        public IndexHeader? Header { get; private set; }

        /// <summary>
        /// Gets the bucket index for this file.
        /// </summary>
        /// <value>The bucket index (0-15) used for distributing <see cref="EKey"/> entries across multiple index files.</value>
        /// <remarks>
        /// <para>
        /// The bucket index should match the upper 4 bits of the first byte in the index filename.
        /// This bucketing system distributes <see cref="EKey"/> entries across 16 separate index files
        /// for improved lookup performance.
        /// </para>
        /// </remarks>
        public byte BucketIndex => Header?.BucketIndex ?? 0;

        /// <summary>
        /// Gets the number of <see cref="EKey"/> entries in the index.
        /// </summary>
        /// <value>The total count of <see cref="EKeyEntry"/> instances loaded from the index file.</value>
        public int EntryCount => _entries.Count;

        /// <summary>
        /// Gets all <see cref="EKey"/> entries in the index.
        /// </summary>
        /// <value>An enumerable collection of all <see cref="EKeyEntry"/> instances representing <see cref="EKey"/> to archive location mappings.</value>
        public IEnumerable<EKeyEntry> Entries => _entries.Values;

        /// <summary>
        /// Parses an index file from a stream.
        /// </summary>
        /// <param name="stream">The stream containing index file data.</param>
        /// <returns>A new <see cref="IndexFile"/> instance with all <see cref="EKeyEntry"/> instances loaded.</returns>
        /// <exception cref="CascParserException">Thrown when the index version is unknown or entry size doesn't match expected size.</exception>
        /// <remarks>
        /// <para>
        /// This method automatically detects the index file version and parses the appropriate format.
        /// The parsed entries map <see cref="EKey"/>s to their locations in archive files, which can then
        /// be used by <see cref="Storage.OnlineCascStorage"/> to retrieve <see cref="Compression.BlteDecoder"/>-encoded content.
        /// </para>
        /// </remarks>
        public static IndexFile Parse(Stream stream)
        {
            var index = new IndexFile();

            // Peek at version to determine header type
            var version = (ushort)(stream.ReadByte() | (stream.ReadByte() << 8));
            stream.Position = 0;

            // Parse header based on version
            if (version == IndexHeaderV1.ExpectedVersion)
            {
                var v1Header = IndexHeaderV1.Parse(stream);
                index.Header = v1Header.ToNormalizedHeader();
            }
            else if (version == IndexHeaderV2.ExpectedVersion)
            {
                var v2Header = IndexHeaderV2.Parse(stream);
                index.Header = v2Header.ToNormalizedHeader();

                // Skip padding after v2 header
                stream.Position += index.Header.HeaderPadding;
            }
            else
            {
                throw new CascParserException($"Unknown index version: 0x{version:X4}");
            }

            // Read entries
            using var reader = new BinaryReader(stream, System.Text.Encoding.UTF8, true);

            if (index.Header.IsVersion1)
            {
                // Version 1: Process entries with checksums
                var entrySize = index.Header.EntryLength;
                var pageSize = CascConstants.FileIndexPageSize;
                var entriesPerPage = pageSize / entrySize;

                if (index.Header.EKeyCount > 0)
                {
                    // Read specified number of entries
                    for (var i = 0; i < index.Header.EKeyCount; i++)
                    {
                        // Check if we need to skip page checksum data
                        if ((stream.Position % pageSize) + entrySize > pageSize)
                        {
                            // Skip to next page
                            var nextPageStart = ((stream.Position / pageSize) + 1) * pageSize;
                            stream.Position = nextPageStart;
                        }

                        // Ensure we read exactly the expected number of bytes
                        var entryStartPos = stream.Position;
                        var entry = EKeyEntry.Parse(reader, index.Header);
                        var bytesRead = stream.Position - entryStartPos;

                        if (bytesRead != entrySize)
                        {
                            throw new CascParserException($"Expected to read {entrySize} bytes for entry, but read {bytesRead} bytes");
                        }

                        if (!entry.EKey.IsEmpty)
                        {
                            index.AddEntry(entry);
                        }
                    }
                }
            }
            else
            {
                // Version 2: Read pages of entries
                var entrySize = index.Header.EntryLength;
                var pageSize = CascConstants.FileIndexPageSize;
                var entriesPerPage = pageSize / entrySize;

                while (stream.Position < stream.Length)
                {
                    var pageStart = stream.Position;
                    var pageIndex = pageStart / pageSize;

                    // Read entries for this page
                    for (var i = 0; i < entriesPerPage; i++)
                    {
                        // Check if we have enough data for a full entry
                        if (stream.Position + entrySize > stream.Length)
                        {
                            // End of file reached
                            goto EndOfFile;
                        }

                        // Check if entry position exceeds page boundary (shouldn't happen in v2)
                        if (stream.Position >= pageStart + pageSize)
                        {
                            break;
                        }

                        // Ensure we read exactly the expected number of bytes
                        var entryStartPos = stream.Position;
                        var entry = EKeyEntry.Parse(reader, index.Header);
                        var bytesRead = stream.Position - entryStartPos;

                        if (bytesRead != entrySize)
                        {
                            throw new CascParserException($"Expected to read {entrySize} bytes for entry, but read {bytesRead} bytes");
                        }

                        // Check if entry is valid (not all zeros)
                        if (!entry.EKey.IsEmpty)
                        {
                            index.AddEntry(entry);
                        }
                    }

                    // Move to the next page
                    stream.Position = (pageIndex + 1) * pageSize;
                }

                EndOfFile:;
            }

            return index;
        }

        /// <summary>
        /// Parses an index file from a file path.
        /// </summary>
        /// <param name="filePath">The path to the index file on disk.</param>
        /// <returns>A new <see cref="IndexFile"/> instance with all <see cref="EKeyEntry"/> instances loaded.</returns>
        /// <exception cref="FileNotFoundException">Thrown when the file at <paramref name="filePath"/> does not exist.</exception>
        /// <exception cref="CascParserException">Thrown when the file format is invalid or corrupted.</exception>
        public static IndexFile ParseFile(string filePath)
        {
            using var stream = File.OpenRead(filePath);
            return Parse(stream);
        }

        /// <summary>
        /// Tries to get an entry by encoded key.
        /// </summary>
        /// <param name="eKey">The <see cref="EKey"/> to look up in the index.</param>
        /// <param name="entry">When this method returns, contains the <see cref="EKeyEntry"/> if found, otherwise <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the entry was found; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// This method is used by <see cref="Storage.OnlineCascStorage"/> to locate files in archives
        /// after resolving <see cref="CascKey"/>s to <see cref="EKey"/>s through the <see cref="Encoding.EncodingFile"/>.
        /// </remarks>
        public bool TryGetEntry(EKey eKey, out EKeyEntry? entry)
        {
            return _entries.TryGetValue(eKey, out entry);
        }

        /// <summary>
        /// Gets an entry by encoded key.
        /// </summary>
        /// <param name="eKey">The <see cref="EKey"/> to look up in the index.</param>
        /// <returns>The <see cref="EKeyEntry"/> containing archive location information.</returns>
        /// <exception cref="CascFileNotFoundException">Thrown when no entry is found for the specified <paramref name="eKey"/>.</exception>
        public EKeyEntry GetEntry(EKey eKey)
        {
            if (!_entries.TryGetValue(eKey, out var entry))
            {
                throw new CascFileNotFoundException(eKey);
            }

            return entry;
        }

        /// <summary>
        /// Adds an <see cref="EKey"/> entry to the index.
        /// </summary>
        /// <param name="entry">The <see cref="EKeyEntry"/> to add containing <see cref="EKey"/> to archive location mapping.</param>
        /// <remarks>
        /// If an entry with the same <see cref="EKey"/> already exists, it will be replaced with the new entry.
        /// </remarks>
        public void AddEntry(EKeyEntry entry)
        {
            _entries[entry.EKey] = entry;
        }

        /// <summary>
        /// Removes an entry from the index.
        /// </summary>
        /// <param name="eKey">The <see cref="EKey"/> of the entry to remove.</param>
        /// <returns><see langword="true"/> if the entry was removed; otherwise, <see langword="false"/>.</returns>
        public bool RemoveEntry(EKey eKey)
        {
            return _entries.Remove(eKey);
        }

        /// <summary>
        /// Clears all <see cref="EKey"/> entries from the index.
        /// </summary>
        /// <remarks>
        /// This method removes all <see cref="EKeyEntry"/> instances, effectively resetting the index to an empty state.
        /// </remarks>
        public void Clear()
        {
            _entries.Clear();
        }
    }
}