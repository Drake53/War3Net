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
        /// Gets the encoding header.
        /// </summary>
        public EncodingHeader? Header { get; private set; }

        /// <summary>
        /// Gets the ESpec strings.
        /// </summary>
        public List<string> ESpecStrings { get; }

        /// <summary>
        /// Gets the number of entries.
        /// </summary>
        public int EntryCount => _entriesByCKey.Count;

        /// <summary>
        /// Gets all entries.
        /// </summary>
        public IEnumerable<EncodingEntry> Entries => _entriesByCKey.Values;

        /// <summary>
        /// Parses an ENCODING file from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The parsed ENCODING file.</returns>
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

                while (pageStream.Position < pageStream.Length)
                {
                    // Check if we have enough bytes for a minimal entry
                    // Minimal size: 2 bytes keyCount + 1 byte size + CKeyLength + at least 1 EKey
                    var minEntrySize = 2 + 1 + encoding.Header.CKeyLength + encoding.Header.EKeyLength;
                    if (pageStream.Length - pageStream.Position < minEntrySize)
                    {
                        break; // Not enough data for a valid entry
                    }

                    // Improved padding detection:
                    // Save position for potential rollback
                    var entryStartPos = pageStream.Position;
                    
                    try
                    {
                        // Peek at the key count (first 2 bytes)
                        var keyCountBytes = new byte[2];
                        if (pageReader.Read(keyCountBytes, 0, 2) != 2)
                        {
                            break; // Can't read key count
                        }
                        
                        var keyCount = BitConverter.ToUInt16(keyCountBytes, 0);
                        
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
            }

            // Skip EKey pages (not typically used for lookups)
            reader.Skip((int)(encoding.Header.EKeyPageCount * EncodingPage.Size));
            reader.Skip((int)(encoding.Header.EKeyPageCount * encoding.Header.EKeyPageSizeBytes));

            return encoding;
        }

        /// <summary>
        /// Parses an ENCODING file from a file path.
        /// </summary>
        /// <param name="filePath">The path to the ENCODING file.</param>
        /// <returns>The parsed ENCODING file.</returns>
        public static EncodingFile ParseFile(string filePath)
        {
            using var stream = File.OpenRead(filePath);
            return Parse(stream);
        }

        /// <summary>
        /// Tries to get an entry by content key.
        /// </summary>
        /// <param name="cKey">The content key.</param>
        /// <param name="entry">The found entry.</param>
        /// <returns>true if the entry was found; otherwise, false.</returns>
        public bool TryGetEntry(CascKey cKey, out EncodingEntry? entry)
        {
            return _entriesByCKey.TryGetValue(cKey, out entry);
        }

        /// <summary>
        /// Tries to get an entry by encoded key.
        /// </summary>
        /// <param name="eKey">The encoded key.</param>
        /// <param name="entry">The found entry.</param>
        /// <returns>true if the entry was found; otherwise, false.</returns>
        public bool TryGetEntry(EKey eKey, out EncodingEntry? entry)
        {
            return _entriesByEKey.TryGetValue(eKey, out entry);
        }

        /// <summary>
        /// Gets an entry by content key.
        /// </summary>
        /// <param name="cKey">The content key.</param>
        /// <returns>The entry.</returns>
        public EncodingEntry GetEntry(CascKey cKey)
        {
            if (!_entriesByCKey.TryGetValue(cKey, out var entry))
            {
                throw new CascFileNotFoundException(cKey);
            }

            return entry;
        }

        /// <summary>
        /// Gets an entry by encoded key.
        /// </summary>
        /// <param name="eKey">The encoded key.</param>
        /// <returns>The entry.</returns>
        public EncodingEntry GetEntry(EKey eKey)
        {
            if (!_entriesByEKey.TryGetValue(eKey, out var entry))
            {
                throw new CascFileNotFoundException(eKey);
            }

            return entry;
        }

        /// <summary>
        /// Gets the encoded key for a content key.
        /// </summary>
        /// <param name="cKey">The content key.</param>
        /// <returns>The encoded key, or null if not found.</returns>
        public EKey? GetEKey(CascKey cKey)
        {
            return TryGetEntry(cKey, out var entry) ? entry?.PrimaryEKey : null;
        }

        /// <summary>
        /// Gets the content key for an encoded key.
        /// </summary>
        /// <param name="eKey">The encoded key.</param>
        /// <returns>The content key, or null if not found.</returns>
        public CascKey? GetCKey(EKey eKey)
        {
            return TryGetEntry(eKey, out var entry) ? entry?.CKey : null;
        }

        /// <summary>
        /// Adds an entry to the encoding file.
        /// </summary>
        /// <param name="entry">The entry to add.</param>
        public void AddEntry(EncodingEntry entry)
        {
            _entriesByCKey[entry.CKey] = entry;

            foreach (var eKey in entry.EKeys)
            {
                _entriesByEKey[eKey] = entry;
            }
        }

        /// <summary>
        /// Removes an entry by content key.
        /// </summary>
        /// <param name="cKey">The content key.</param>
        /// <returns>true if the entry was removed; otherwise, false.</returns>
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
        /// Clears all entries.
        /// </summary>
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