// ------------------------------------------------------------------------------
// <copyright file="IndexFile.cs" company="Drake53">
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
    /// Represents a CASC index file (.idx).
    /// </summary>
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
        /// Gets the index header.
        /// </summary>
        public IndexHeader? Header { get; private set; }

        /// <summary>
        /// Gets the bucket index for this file.
        /// </summary>
        /// <remarks>
        /// The bucket index should match the upper 4 bits of the first byte in the index filename.
        /// </remarks>
        public byte BucketIndex => Header?.BucketIndex ?? 0;

        /// <summary>
        /// Gets the number of entries in the index.
        /// </summary>
        public int EntryCount => _entries.Count;

        /// <summary>
        /// Gets all entries in the index.
        /// </summary>
        public IEnumerable<EKeyEntry> Entries => _entries.Values;

        /// <summary>
        /// Parses an index file from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The parsed index file.</returns>
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
                    for (int i = 0; i < index.Header.EKeyCount; i++)
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
                    for (int i = 0; i < entriesPerPage; i++)
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
        /// <param name="filePath">The path to the index file.</param>
        /// <returns>The parsed index file.</returns>
        public static IndexFile ParseFile(string filePath)
        {
            using var stream = File.OpenRead(filePath);
            return Parse(stream);
        }

        /// <summary>
        /// Tries to get an entry by encoded key.
        /// </summary>
        /// <param name="eKey">The encoded key.</param>
        /// <param name="entry">The found entry.</param>
        /// <returns>true if the entry was found; otherwise, false.</returns>
        public bool TryGetEntry(EKey eKey, out EKeyEntry? entry)
        {
            return _entries.TryGetValue(eKey, out entry);
        }

        /// <summary>
        /// Gets an entry by encoded key.
        /// </summary>
        /// <param name="eKey">The encoded key.</param>
        /// <returns>The entry.</returns>
        public EKeyEntry GetEntry(EKey eKey)
        {
            if (!_entries.TryGetValue(eKey, out var entry))
            {
                throw new CascFileNotFoundException(eKey);
            }

            return entry;
        }

        /// <summary>
        /// Adds an entry to the index.
        /// </summary>
        /// <param name="entry">The entry to add.</param>
        public void AddEntry(EKeyEntry entry)
        {
            _entries[entry.EKey] = entry;
        }

        /// <summary>
        /// Removes an entry from the index.
        /// </summary>
        /// <param name="eKey">The encoded key of the entry to remove.</param>
        /// <returns>true if the entry was removed; otherwise, false.</returns>
        public bool RemoveEntry(EKey eKey)
        {
            return _entries.Remove(eKey);
        }

        /// <summary>
        /// Clears all entries from the index.
        /// </summary>
        public void Clear()
        {
            _entries.Clear();
        }
    }
}