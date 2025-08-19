// ------------------------------------------------------------------------------
// <copyright file="TextRootHandler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

using War3Net.IO.Casc.Structures;

namespace War3Net.IO.Casc.Root
{
    /// <summary>
    /// Handles text-based root files (used by some older games).
    /// </summary>
    public class TextRootHandler : RootHandlerBase
    {
        /// <inheritdoc/>
        public override void Parse(Stream stream)
        {
            Clear();

            using var reader = new StreamReader(stream, System.Text.Encoding.UTF8, true, 1024, true);
            string? line;

            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#", StringComparison.Ordinal))
                {
                    continue;
                }

                // Expected format: FileName|CKey|FileSize
                var parts = line.Split('|');
                if (parts.Length >= 2)
                {
                    var entry = new RootEntry
                    {
                        FileName = parts[0].Trim(),
                    };

                    // Parse CKey
                    if (CascKey.TryParse(parts[1].Trim(), out var cKey))
                    {
                        entry.CKey = cKey;
                    }

                    // Parse file size if available
                    if (parts.Length >= 3 && ulong.TryParse(parts[2].Trim(), out var fileSize))
                    {
                        // Store file size in entry if needed
                    }

                    if (entry.HasFileName && entry.HasCKey)
                    {
                        AddEntry(entry);
                    }
                }
            }
        }
    }
}