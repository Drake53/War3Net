// ------------------------------------------------------------------------------
// <copyright file="BasicRootHandler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using War3Net.IO.Casc.Enums;
using War3Net.IO.Casc.Structures;

namespace War3Net.IO.Casc.Root
{
    /// <summary>
    /// Basic root handler implementation for file name to key resolution.
    /// </summary>
    public class BasicRootHandler : RootHandlerBase
    {
        private readonly Dictionary<string, RootEntry> _rootEntries;
        private readonly Dictionary<uint, RootEntry> _fileDataIdEntries;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicRootHandler"/> class.
        /// </summary>
        public BasicRootHandler()
        {
            _rootEntries = new Dictionary<string, RootEntry>(StringComparer.OrdinalIgnoreCase);
            _fileDataIdEntries = new Dictionary<uint, RootEntry>();
        }

        /// <summary>
        /// Gets the root entry count.
        /// </summary>
        public int RootEntryCount => _rootEntries.Count;

        /// <summary>
        /// Tries to get an entry by file name.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="entry">The entry, if found.</param>
        /// <returns>true if the entry was found; otherwise, false.</returns>
        public bool TryGetEntry(string fileName, out RootEntry? entry)
        {
            return _rootEntries.TryGetValue(fileName, out entry);
        }

        /// <summary>
        /// Tries to get an entry by file data ID.
        /// </summary>
        /// <param name="fileDataId">The file data ID.</param>
        /// <param name="entry">The entry, if found.</param>
        /// <returns>true if the entry was found; otherwise, false.</returns>
        public bool TryGetEntry(uint fileDataId, out RootEntry? entry)
        {
            return _fileDataIdEntries.TryGetValue(fileDataId, out entry);
        }

        /// <summary>
        /// Gets entries for a file name.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The entries.</returns>
        public IEnumerable<RootEntry> GetEntries(string fileName)
        {
            if (_rootEntries.TryGetValue(fileName, out var entry))
            {
                yield return entry;
            }
        }

        /// <summary>
        /// Gets all entries.
        /// </summary>
        /// <returns>All root entries.</returns>
        public IEnumerable<RootEntry> GetAllEntries()
        {
            return _rootEntries.Values;
        }

        /// <inheritdoc/>
        public override RootEntry? GetEntry(string fileName)
        {
            return _rootEntries.TryGetValue(fileName, out var entry) ? entry : null;
        }

        /// <inheritdoc/>
        public override RootEntry? GetEntry(uint fileDataId)
        {
            return _fileDataIdEntries.TryGetValue(fileDataId, out var entry) ? entry : null;
        }

        /// <inheritdoc/>
        public override IEnumerable<RootEntry> GetEntries()
        {
            return _rootEntries.Values;
        }

        /// <inheritdoc/>
        public override void Clear()
        {
            _rootEntries.Clear();
            _fileDataIdEntries.Clear();
        }

        /// <inheritdoc/>
        public override void Parse(Stream stream)
        {
            LoadFromStream(stream);
        }

        /// <summary>
        /// Adds a root entry.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="entry">The root entry.</param>
        public void AddEntry(string fileName, RootEntry entry)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
            }

            _rootEntries[fileName] = entry;

            if (entry.FileDataId != CascConstants.InvalidId)
            {
                _fileDataIdEntries[entry.FileDataId] = entry;
            }
        }

        /// <summary>
        /// Removes a root entry.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>true if the entry was removed; otherwise, false.</returns>
        public bool RemoveEntry(string fileName)
        {
            if (_rootEntries.TryGetValue(fileName, out var entry))
            {
                _rootEntries.Remove(fileName);

                if (entry.FileDataId != CascConstants.InvalidId)
                {
                    _fileDataIdEntries.Remove(entry.FileDataId);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Loads root entries from a stream.
        /// </summary>
        /// <param name="stream">The stream to load from.</param>
        /// <returns>A list of errors encountered during loading, or empty if successful.</returns>
        public List<string> LoadFromStream(Stream stream)
        {
            var errors = new List<string>();
            using var reader = new StreamReader(stream);
            string? line;
            int lineNumber = 0;
            int successCount = 0;

            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                {
                    continue;
                }

                var parts = line.Split('|');
                if (parts.Length < 2)
                {
                    errors.Add($"Line {lineNumber}: Invalid format - expected at least 2 pipe-separated values");
                    continue;
                }

                var fileName = parts[0].Trim();
                var keyString = parts[1].Trim();

                if (string.IsNullOrEmpty(fileName))
                {
                    errors.Add($"Line {lineNumber}: Empty file name");
                    continue;
                }

                try
                {
                    var entry = new RootEntry
                    {
                        CKey = CascKey.Parse(keyString),
                        LocaleFlags = CascLocaleFlags.All,
                        ContentFlags = CascContentFlags.None,
                    };

                    // Parse optional fields
                    if (parts.Length > 2 && uint.TryParse(parts[2], out var fileDataId))
                    {
                        entry.FileDataId = fileDataId;
                    }

                    if (parts.Length > 3 && uint.TryParse(parts[3], out var localeFlags))
                    {
                        entry.LocaleFlags = (CascLocaleFlags)localeFlags;
                    }

                    if (parts.Length > 4 && uint.TryParse(parts[4], out var contentFlags))
                    {
                        entry.ContentFlags = (CascContentFlags)contentFlags;
                    }

                    AddEntry(fileName, entry);
                    successCount++;
                }
                catch (FormatException ex)
                {
                    errors.Add($"Line {lineNumber}: Invalid key format for '{fileName}': {ex.Message}");
                }
                catch (ArgumentException ex)
                {
                    errors.Add($"Line {lineNumber}: Invalid entry for '{fileName}': {ex.Message}");
                }
                catch (Exception ex)
                {
                    errors.Add($"Line {lineNumber}: Unexpected error for '{fileName}': {ex.Message}");
                }
            }

            // Log summary
            if (errors.Count > 0)
            {
                System.Diagnostics.Trace.TraceWarning($"Loaded {successCount} root entries with {errors.Count} errors");
                foreach (var error in errors.Take(10)) // Log first 10 errors
                {
                    System.Diagnostics.Trace.TraceWarning($"Root loading error: {error}");
                }
                if (errors.Count > 10)
                {
                    System.Diagnostics.Trace.TraceWarning($"... and {errors.Count - 10} more errors");
                }
            }
            else
            {
                System.Diagnostics.Trace.TraceInformation($"Successfully loaded {successCount} root entries");
            }

            return errors;
        }

        /// <summary>
        /// Saves root entries to a stream.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        public void SaveToStream(Stream stream)
        {
            using var writer = new StreamWriter(stream);
            writer.WriteLine("# CASC Root File");
            writer.WriteLine("# Format: FileName|CKey|FileDataId|LocaleFlags|ContentFlags");

            foreach (var kvp in _rootEntries)
            {
                var entry = kvp.Value;
                writer.WriteLine($"{kvp.Key}|{entry.CKey}|{entry.FileDataId}|{(uint)entry.LocaleFlags}|{(uint)entry.ContentFlags}");
            }
        }
    }
}