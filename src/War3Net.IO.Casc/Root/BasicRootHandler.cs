// ------------------------------------------------------------------------------
// <copyright file="BasicRootHandler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;

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

        /// <inheritdoc/>
        public override int RootEntryCount => _rootEntries.Count;

        /// <inheritdoc/>
        public override bool TryGetEntry(string fileName, out RootEntry? entry)
        {
            return _rootEntries.TryGetValue(fileName, out entry);
        }

        /// <inheritdoc/>
        public override bool TryGetEntry(uint fileDataId, out RootEntry? entry)
        {
            return _fileDataIdEntries.TryGetValue(fileDataId, out entry);
        }

        /// <inheritdoc/>
        public override IEnumerable<RootEntry> GetEntries(string fileName)
        {
            if (_rootEntries.TryGetValue(fileName, out var entry))
            {
                yield return entry;
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<RootEntry> GetAllEntries()
        {
            return _rootEntries.Values;
        }

        /// <inheritdoc/>
        public override void Clear()
        {
            _rootEntries.Clear();
            _fileDataIdEntries.Clear();
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
        public void LoadFromStream(Stream stream)
        {
            using var reader = new StreamReader(stream);
            string? line;

            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                {
                    continue;
                }

                var parts = line.Split('|');
                if (parts.Length < 2)
                {
                    continue;
                }

                var fileName = parts[0].Trim();
                var keyString = parts[1].Trim();

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
                }
                catch
                {
                    // Skip invalid entries
                }
            }
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