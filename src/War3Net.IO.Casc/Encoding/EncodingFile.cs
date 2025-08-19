// ------------------------------------------------------------------------------
// <copyright file="EncodingFile.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using War3Net.IO.Casc.Structures;
using War3Net.IO.Casc.Utilities;

namespace War3Net.IO.Casc.Encoding
{
    /// <summary>
    /// Represents a CASC ENCODING manifest file.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The encoding file is a critical component of the TACT system that maps <see cref="CascKey"/> (Content Hashes)
    /// to <see cref="EKey"/> (Encoded-file Hashes). It provides information on how files are <see cref="Compression.BlteDecoder"/>-encoded
    /// through ESpecs (Encoding Specifications).
    /// </para>
    /// <para>
    /// Structure of the encoding file:
    /// </para>
    /// <list type="number">
    /// <item><description>Header (0x16 bytes) - Contains size information for other blocks</description></item>
    /// <item><description>ESpec block - Encoding specification strings (zero-terminated) describing how files are encoded</description></item>
    /// <item><description>CEKeyPageTable - Maps <see cref="CascKey"/>s to one or more <see cref="EKey"/>s (supports multiple encoded representations)</description></item>
    /// <item><description>EKeySpecPageTable - Maps <see cref="EKey"/>s to their corresponding ESpec describing the encoding</description></item>
    /// <item><description>Encoding specification for the encoding file itself</description></item>
    /// </list>
    /// <para>
    /// The file uses a paged structure for efficient key lookups. Each page table has an index for fast
    /// key-to-page access followed by the actual pages containing the mapping data. Pages are checksummed
    /// using MD5 for integrity verification.
    /// </para>
    /// <para>
    /// This file is referenced in the <see cref="Cdn.BuildConfig"/> and is essential for translating between content
    /// identifiers and their encoded representations on the CDN. It's used by <see cref="Storage.OnlineCascStorage"/>
    /// to resolve <see cref="CascKey"/> lookups from <see cref="Root.TvfsRootHandler"/> to <see cref="EKey"/>s for
    /// retrieval from <see cref="Index.IndexFile"/>s.
    /// </para>
    /// </remarks>
    public class EncodingFile
    {
        private readonly Dictionary<CascKey, EncodingEntry> _entriesByCKey;
        private readonly Dictionary<EKey, EncodingEntry> _entriesByEKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="EncodingFile"/> class.
        /// </summary>
        public EncodingFile()
        {
            _entriesByCKey = new Dictionary<CascKey, EncodingEntry>();
            _entriesByEKey = new Dictionary<EKey, EncodingEntry>();
            ESpecStrings = new List<string>();
        }

        /// <summary>
        /// Gets the encoding header containing file structure information.
        /// </summary>
        /// <value>The <see cref="EncodingHeader"/> if parsed, otherwise <see langword="null"/>.</value>
        public EncodingHeader? Header { get; private set; }

        /// <summary>
        /// Gets the encoding specification strings describing how files are encoded.
        /// </summary>
        /// <value>A list of ESpec strings used by <see cref="Compression.BlteDecoder"/> to determine encoding methods.</value>
        public List<string> ESpecStrings { get; }

        /// <summary>
        /// Gets the number of <see cref="CascKey"/> to <see cref="EKey"/> mapping entries.
        /// </summary>
        /// <value>The total count of unique <see cref="CascKey"/> entries in the encoding file.</value>
        public int EntryCount => _entriesByCKey.Count;

        /// <summary>
        /// Gets all <see cref="EncodingEntry"/> instances representing <see cref="CascKey"/> to <see cref="EKey"/> mappings.
        /// </summary>
        /// <value>An enumerable collection of all encoding entries loaded from the file.</value>
        public IEnumerable<EncodingEntry> Entries => _entriesByCKey.Values;

        /// <summary>
        /// Parses an ENCODING file from a stream.
        /// </summary>
        /// <param name="stream">The stream containing ENCODING file data.</param>
        /// <returns>A new <see cref="EncodingFile"/> instance with all entries loaded.</returns>
        /// <exception cref="CascParserException">Thrown when the file format is invalid or corrupted.</exception>
        /// <remarks>
        /// <para>
        /// This method reads the entire encoding file structure including the header, ESpec strings,
        /// and all <see cref="CascKey"/> to <see cref="EKey"/> mappings. The parsed file can be used
        /// by <see cref="Storage.OnlineCascStorage"/> for content resolution.
        /// </para>
        /// <para>
        /// The method includes validation to prevent excessive memory allocation from corrupted files.
        /// </para>
        /// </remarks>
        public static EncodingFile Parse(Stream stream)
        {
            var encoding = new EncodingFile();
            using var reader = new BinaryReader(stream, System.Text.Encoding.UTF8, true);

            // Parse header
            encoding.Header = EncodingHeader.Parse(reader);

            // Read ESpec block if present
            if (encoding.Header.ESpecBlockSize > 0)
            {
                var especBytes = reader.ReadBytes((int)encoding.Header.ESpecBlockSize);
                encoding.ParseESpecBlock(especBytes);
            }

            // Read CKey pages
            var ckeyPages = new List<EncodingPage>();
            for (uint i = 0; i < encoding.Header.CKeyPageCount; i++)
            {
                ckeyPages.Add(EncodingPage.Parse(reader));
            }

            // Validate page sizes to prevent excessive memory allocation
            const int MaxPageSize = 10 * 1024 * 1024; // 10 MB max page size - reasonable for CASC files
            if (encoding.Header.CKeyPageSizeBytes > MaxPageSize)
            {
                throw new CascParserException($"CKey page size {encoding.Header.CKeyPageSizeBytes} exceeds maximum allowed size of {MaxPageSize} bytes");
            }

            if (encoding.Header.EKeyPageSizeBytes > MaxPageSize)
            {
                throw new CascParserException($"EKey page size {encoding.Header.EKeyPageSizeBytes} exceeds maximum allowed size of {MaxPageSize} bytes");
            }

            // Additional validation: ensure we don't allocate more memory than available
            var totalPagesSize = (long)encoding.Header.CKeyPageCount * encoding.Header.CKeyPageSizeBytes +
                               (long)encoding.Header.EKeyPageCount * encoding.Header.EKeyPageSizeBytes;
            const long MaxTotalSize = 500L * 1024 * 1024; // 500 MB total max
            if (totalPagesSize > MaxTotalSize)
            {
                throw new CascParserException($"Total page allocation {totalPagesSize} bytes exceeds maximum allowed {MaxTotalSize} bytes");
            }

            // Read CKey entries
            var totalEntriesRead = 0;
            for (uint pageIndex = 0; pageIndex < encoding.Header.CKeyPageCount; pageIndex++)
            {
                // Validate we have enough data remaining
                if (stream.Position + encoding.Header.CKeyPageSizeBytes > stream.Length)
                {
                    throw new CascParserException($"Unexpected end of stream while reading CKey page {pageIndex}");
                }

                var pageData = reader.ReadBytes(encoding.Header.CKeyPageSizeBytes);
                using var pageStream = new MemoryStream(pageData);
                using var pageReader = new BinaryReader(pageStream);

                var pageEntriesRead = 0;
                while (pageStream.Position < pageStream.Length)
                {
                    // Check if we have enough bytes for a minimal entry
                    // Minimal size: 1 byte keyCount + 5 bytes size + CKeyLength + at least 1 EKey
                    var minEntrySize = 1 + 5 + encoding.Header.CKeyLength + encoding.Header.EKeyLength;
                    if (pageStream.Length - pageStream.Position < minEntrySize)
                    {
                        break; // Not enough data for a valid entry
                    }

                    // Improved padding detection:
                    // Save position for potential rollback
                    var entryStartPos = pageStream.Position;

                    try
                    {
                        // Peek at the key count (first byte)
                        var keyCountByte = pageReader.ReadByte();
                        if (keyCountByte == -1)
                        {
                            break; // End of stream
                        }

                        var keyCount = (byte)keyCountByte;

                        // Validate key count - CascLib allows up to 256 keys per entry
                        // Zero key count indicates padding
                        if (keyCount == 0)
                        {
                            // Hit padding - stop processing this page
                            break;
                        }

                        // Sanity check: while technically up to 256 is allowed,
                        // in practice more than 100 keys is extremely rare and likely corrupt data
                        if (keyCount > 256)
                        {
                            // Invalid data - stop processing
                            pageStream.Position = entryStartPos;
                            break;
                        }

                        // Reset to read the full entry
                        pageStream.Position = entryStartPos;

                        var entry = EncodingEntry.Parse(pageReader, encoding.Header);

                        // Additional validation
                        if (entry.CKey.IsEmpty || entry.EKeys.Count == 0 || entry.EKeys.Count != keyCount)
                        {
                            // Invalid entry structure
                            pageStream.Position = entryStartPos;
                            break;
                        }

                        encoding.AddEntry(entry);
                        pageEntriesRead++;
                    }
                    catch (EndOfStreamException)
                    {
                        // Hit end of valid data
                        break;
                    }
                    catch (Exception ex)
                    {
                        // Log the error for debugging but continue trying to parse
                        System.Diagnostics.Trace.TraceWarning($"Failed to parse encoding entry at position {entryStartPos}: {ex.Message}");

                        // Try to recover by seeking to next potential entry
                        // This is a last-resort recovery attempt
                        pageStream.Position = entryStartPos + 1;

                        // If we're too close to the end, just stop
                        if (pageStream.Length - pageStream.Position < minEntrySize)
                        {
                            break;
                        }
                    }
                }

                System.Diagnostics.Trace.TraceInformation($"Page {pageIndex}: Read {pageEntriesRead} entries");
                totalEntriesRead += pageEntriesRead;
            }

            System.Diagnostics.Trace.TraceInformation($"Total entries read from encoding file: {totalEntriesRead}");

            // Skip EKey pages (not typically used for lookups)
            reader.Skip((int)(encoding.Header.EKeyPageCount * EncodingPage.Size));
            reader.Skip((int)(encoding.Header.EKeyPageCount * encoding.Header.EKeyPageSizeBytes));

            return encoding;
        }

        /// <summary>
        /// Parses an ENCODING file from a file path.
        /// </summary>
        /// <param name="filePath">The path to the ENCODING file on disk.</param>
        /// <returns>A new <see cref="EncodingFile"/> instance with all entries loaded.</returns>
        /// <exception cref="FileNotFoundException">Thrown when the file at <paramref name="filePath"/> does not exist.</exception>
        /// <exception cref="CascParserException">Thrown when the file format is invalid or corrupted.</exception>
        public static EncodingFile ParseFile(string filePath)
        {
            using var stream = File.OpenRead(filePath);
            return Parse(stream);
        }

        /// <summary>
        /// Tries to get an encoding entry by content key.
        /// </summary>
        /// <param name="cKey">The <see cref="CascKey"/> to look up.</param>
        /// <param name="entry">When this method returns, contains the <see cref="EncodingEntry"/> if found, otherwise <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the entry was found; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// This method is used by <see cref="Storage.OnlineCascStorage"/> to resolve <see cref="CascKey"/>s
        /// obtained from <see cref="Root.TvfsRootHandler"/> to <see cref="EKey"/>s for file retrieval.
        /// </remarks>
        public bool TryGetEntry(CascKey cKey, out EncodingEntry? entry)
        {
            return _entriesByCKey.TryGetValue(cKey, out entry);
        }

        /// <summary>
        /// Tries to get an encoding entry by encoded key.
        /// </summary>
        /// <param name="eKey">The <see cref="EKey"/> to look up.</param>
        /// <param name="entry">When this method returns, contains the <see cref="EncodingEntry"/> if found, otherwise <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the entry was found; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// This method is typically used for reverse lookups when you have an <see cref="EKey"/> from
        /// an <see cref="Index.IndexFile"/> and need to find the corresponding <see cref="CascKey"/>.
        /// </remarks>
        public bool TryGetEntry(EKey eKey, out EncodingEntry? entry)
        {
            return _entriesByEKey.TryGetValue(eKey, out entry);
        }

        /// <summary>
        /// Gets an encoding entry by content key.
        /// </summary>
        /// <param name="cKey">The <see cref="CascKey"/> to look up.</param>
        /// <returns>The <see cref="EncodingEntry"/> associated with the content key.</returns>
        /// <exception cref="CascFileNotFoundException">Thrown when no entry is found for the specified <paramref name="cKey"/>.</exception>
        public EncodingEntry GetEntry(CascKey cKey)
        {
            if (!_entriesByCKey.TryGetValue(cKey, out var entry))
            {
                throw new CascFileNotFoundException(cKey);
            }

            return entry;
        }

        /// <summary>
        /// Gets an encoding entry by encoded key.
        /// </summary>
        /// <param name="eKey">The <see cref="EKey"/> to look up.</param>
        /// <returns>The <see cref="EncodingEntry"/> associated with the encoded key.</returns>
        /// <exception cref="CascFileNotFoundException">Thrown when no entry is found for the specified <paramref name="eKey"/>.</exception>
        public EncodingEntry GetEntry(EKey eKey)
        {
            if (!_entriesByEKey.TryGetValue(eKey, out var entry))
            {
                throw new CascFileNotFoundException(eKey);
            }

            return entry;
        }

        /// <summary>
        /// Gets the primary encoded key for a content key.
        /// </summary>
        /// <param name="cKey">The <see cref="CascKey"/> to look up.</param>
        /// <returns>The primary <see cref="EKey"/> associated with the content key, or <see langword="null"/> if not found.</returns>
        /// <remarks>
        /// This method returns the first <see cref="EKey"/> from the entry's key list. If multiple
        /// encoded representations exist, use <see cref="GetEntry(CascKey)"/> to access all keys.
        /// </remarks>
        public EKey? GetEKey(CascKey cKey)
        {
            return TryGetEntry(cKey, out var entry) ? entry?.PrimaryEKey : null;
        }

        /// <summary>
        /// Gets the content key for an encoded key.
        /// </summary>
        /// <param name="eKey">The <see cref="EKey"/> to look up.</param>
        /// <returns>The <see cref="CascKey"/> associated with the encoded key, or <see langword="null"/> if not found.</returns>
        /// <remarks>
        /// This method performs a reverse lookup from <see cref="EKey"/> to <see cref="CascKey"/>.
        /// Useful when working with <see cref="Index.IndexFile"/> entries that contain <see cref="EKey"/>s.
        /// </remarks>
        public CascKey? GetCKey(EKey eKey)
        {
            return TryGetEntry(eKey, out var entry) ? entry?.CKey : null;
        }

        /// <summary>
        /// Adds an encoding entry to the file.
        /// </summary>
        /// <param name="entry">The <see cref="EncodingEntry"/> to add containing <see cref="CascKey"/> to <see cref="EKey"/> mappings.</param>
        /// <remarks>
        /// This method updates both the <see cref="CascKey"/> and <see cref="EKey"/> lookup dictionaries.
        /// If an entry with the same <see cref="CascKey"/> already exists, it will be replaced.
        /// </remarks>
        public void AddEntry(EncodingEntry entry)
        {
            _entriesByCKey[entry.CKey] = entry;

            foreach (var eKey in entry.EKeys)
            {
                _entriesByEKey[eKey] = entry;
            }
        }

        /// <summary>
        /// Removes an encoding entry by content key.
        /// </summary>
        /// <param name="cKey">The <see cref="CascKey"/> of the entry to remove.</param>
        /// <returns><see langword="true"/> if the entry was removed; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// This method removes the entry from both <see cref="CascKey"/> and <see cref="EKey"/> lookup dictionaries.
        /// All <see cref="EKey"/>s associated with the <see cref="CascKey"/> will be removed.
        /// </remarks>
        public bool RemoveEntry(CascKey cKey)
        {
            if (_entriesByCKey.TryGetValue(cKey, out var entry))
            {
                _entriesByCKey.Remove(cKey);

                foreach (var eKey in entry.EKeys)
                {
                    _entriesByEKey.Remove(eKey);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Clears all encoding entries and ESpec strings.
        /// </summary>
        /// <remarks>
        /// This method removes all <see cref="CascKey"/> to <see cref="EKey"/> mappings and ESpec strings,
        /// effectively resetting the encoding file to an empty state.
        /// </remarks>
        public void Clear()
        {
            _entriesByCKey.Clear();
            _entriesByEKey.Clear();
            ESpecStrings.Clear();
        }

        private void ParseESpecBlock(byte[] especData)
        {
            try
            {
                var text = System.Text.Encoding.UTF8.GetString(especData);
                var strings = text.Split(new[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
                ESpecStrings.AddRange(strings);
            }
            catch (DecoderFallbackException ex)
            {
                throw new CascParserException($"Invalid UTF-8 encoding in ESpec block: {ex.Message}", ex);
            }
        }
    }
}