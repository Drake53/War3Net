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
            using var reader = new BinaryReader(stream, Encoding.UTF8, true);

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

                    // More robust padding detection
                    // Check if the next entry would be all zeros (padding)
                    var peekPos = pageStream.Position;
                    var peekBytes = new byte[Math.Min(16, (int)(pageStream.Length - pageStream.Position))];
                    var bytesRead = pageReader.Read(peekBytes, 0, peekBytes.Length);
                    pageStream.Position = peekPos;

                    // If first 16 bytes (or remaining bytes) are all zeros, it's padding
                    var isAllZeros = true;
                    for (int i = 0; i < bytesRead; i++)
                    {
                        if (peekBytes[i] != 0)
                        {
                            isAllZeros = false;
                            break;
                        }
                    }

                    if (isAllZeros)
                    {
                        break; // Hit padding
                    }

                    try
                    {
                        var entry = EncodingEntry.Parse(pageReader, encoding.Header);
                        
                        // Validate the entry before adding
                        if (entry.CKey.IsEmpty && entry.EKeys.Count == 0)
                        {
                            break; // Invalid entry, likely padding
                        }
                        
                        encoding.AddEntry(entry);
                    }
                    catch
                    {
                        // If parsing fails, we've likely hit padding or corrupted data
                        break;
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
        /// <param name="ckey">The content key.</param>
        /// <param name="entry">The found entry.</param>
        /// <returns>true if the entry was found; otherwise, false.</returns>
        public bool TryGetEntry(CascKey ckey, out EncodingEntry? entry)
        {
            return _entriesByCKey.TryGetValue(ckey, out entry);
        }

        /// <summary>
        /// Tries to get an entry by encoded key.
        /// </summary>
        /// <param name="ekey">The encoded key.</param>
        /// <param name="entry">The found entry.</param>
        /// <returns>true if the entry was found; otherwise, false.</returns>
        public bool TryGetEntry(EKey ekey, out EncodingEntry? entry)
        {
            return _entriesByEKey.TryGetValue(ekey, out entry);
        }

        /// <summary>
        /// Gets an entry by content key.
        /// </summary>
        /// <param name="ckey">The content key.</param>
        /// <returns>The entry.</returns>
        public EncodingEntry GetEntry(CascKey ckey)
        {
            if (!_entriesByCKey.TryGetValue(ckey, out var entry))
            {
                throw new CascFileNotFoundException(ckey);
            }

            return entry;
        }

        /// <summary>
        /// Gets an entry by encoded key.
        /// </summary>
        /// <param name="ekey">The encoded key.</param>
        /// <returns>The entry.</returns>
        public EncodingEntry GetEntry(EKey ekey)
        {
            if (!_entriesByEKey.TryGetValue(ekey, out var entry))
            {
                throw new CascFileNotFoundException(ekey);
            }

            return entry;
        }

        /// <summary>
        /// Gets the encoded key for a content key.
        /// </summary>
        /// <param name="ckey">The content key.</param>
        /// <returns>The encoded key, or null if not found.</returns>
        public EKey? GetEKey(CascKey ckey)
        {
            return TryGetEntry(ckey, out var entry) ? entry?.PrimaryEKey : null;
        }

        /// <summary>
        /// Gets the content key for an encoded key.
        /// </summary>
        /// <param name="ekey">The encoded key.</param>
        /// <returns>The content key, or null if not found.</returns>
        public CascKey? GetCKey(EKey ekey)
        {
            return TryGetEntry(ekey, out var entry) ? entry?.CKey : null;
        }

        /// <summary>
        /// Adds an entry to the encoding file.
        /// </summary>
        /// <param name="entry">The entry to add.</param>
        public void AddEntry(EncodingEntry entry)
        {
            _entriesByCKey[entry.CKey] = entry;

            foreach (var ekey in entry.EKeys)
            {
                _entriesByEKey[ekey] = entry;
            }
        }

        /// <summary>
        /// Removes an entry by content key.
        /// </summary>
        /// <param name="ckey">The content key.</param>
        /// <returns>true if the entry was removed; otherwise, false.</returns>
        public bool RemoveEntry(CascKey ckey)
        {
            if (_entriesByCKey.TryGetValue(ckey, out var entry))
            {
                _entriesByCKey.Remove(ckey);

                foreach (var ekey in entry.EKeys)
                {
                    _entriesByEKey.Remove(ekey);
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
                var text = Encoding.UTF8.GetString(especData);
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