// ------------------------------------------------------------------------------
// <copyright file="TvfsRootHandler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;

using War3Net.IO.Casc.Structures;

namespace War3Net.IO.Casc.Root
{
    /// <summary>
    /// Handles TVFS (Tree Virtual File System) root files used by Warcraft III.
    /// </summary>
    public class TvfsRootHandler : RootHandlerBase
    {
        private const uint TvfsSignature = 0x53465654; // 'TVFS' - bytes: 54 56 46 53 in little-endian

        /// <inheritdoc/>
        public override void Parse(Stream stream)
        {
            Clear();

            using var reader = new BinaryReader(stream, System.Text.Encoding.UTF8, true);

            // Read TVFS header
            var signature = reader.ReadUInt32();
            if (signature != TvfsSignature)
            {
                throw new InvalidDataException($"Invalid TVFS signature: 0x{signature:X8}");
            }

            var version = reader.ReadByte();
            var headerSize = reader.ReadByte();
            var ekeySize = reader.ReadByte();
            var patchKeySize = reader.ReadByte();
            var flags = reader.ReadUInt32();
            var pathTableOffset = reader.ReadUInt32();
            var pathTableSize = reader.ReadUInt32();
            var vfsTableOffset = reader.ReadUInt32();
            var vfsTableSize = reader.ReadUInt32();
            var ckeyTableOffset = reader.ReadUInt32();
            var ckeyTableSize = reader.ReadUInt32();
            var ekeyTableOffset = reader.ReadUInt32();
            var ekeyTableSize = reader.ReadUInt32();
            var patchEkeyTableOffset = reader.ReadUInt32();
            var patchEkeyTableSize = reader.ReadUInt32();
            var directoryManifestOffset = reader.ReadUInt32();
            var directoryManifestSize = reader.ReadUInt32();

            // Read path table (file names)
            var paths = new List<string>();
            if (pathTableSize > 0)
            {
                stream.Position = pathTableOffset;
                var pathData = reader.ReadBytes((int)pathTableSize);
                var pathString = System.Text.Encoding.UTF8.GetString(pathData);
                paths.AddRange(pathString.Split('\0', StringSplitOptions.RemoveEmptyEntries));
                System.Diagnostics.Trace.TraceInformation($"TVFS: Read {paths.Count} paths from path table");
            }

            // Read VFS table (Virtual File System entries)
            if (vfsTableSize > 0)
            {
                stream.Position = vfsTableOffset;
                var entryCount = vfsTableSize / 24; // Each entry is 24 bytes
                System.Diagnostics.Trace.TraceInformation($"TVFS: Processing {entryCount} VFS entries");

                var addedCount = 0;
                for (uint i = 0; i < entryCount; i++)
                {
                    // Read VFS entry
                    var pathOffset = reader.ReadUInt32();
                    var pathIndex = reader.ReadUInt32();
                    var ckeyIndex = reader.ReadUInt32();
                    var ekeyIndex = reader.ReadUInt32();
                    var fileSize = reader.ReadUInt32();
                    var flags2 = reader.ReadUInt32();

                    // Skip if no valid path
                    if (pathIndex >= paths.Count)
                    {
                        continue;
                    }

                    var entry = new RootEntry
                    {
                        FileName = paths[(int)pathIndex],
                    };

                    // Read CKey if available
                    if (ckeyIndex != 0xFFFFFFFF && ckeyTableSize > 0)
                    {
                        var ckeyOffset = ckeyTableOffset + (ckeyIndex * 16);
                        if (ckeyOffset + 16 <= stream.Length)
                        {
                            var currentPos = stream.Position;
                            stream.Position = ckeyOffset;
                            var ckeyBytes = reader.ReadBytes(16);
                            entry.CKey = new CascKey(ckeyBytes);
                            stream.Position = currentPos;
                        }
                    }

                    // Read EKey if available
                    if (ekeyIndex != 0xFFFFFFFF && ekeyTableSize > 0)
                    {
                        var ekeyOffset = ekeyTableOffset + (ekeyIndex * ekeySize);
                        if (ekeyOffset + ekeySize <= stream.Length)
                        {
                            var currentPos = stream.Position;
                            stream.Position = ekeyOffset;
                            var ekeyBytes = reader.ReadBytes(ekeySize);

                            // Pad or truncate to 16 bytes for EKey
                            var ekeyData = new byte[16];
                            Array.Copy(ekeyBytes, 0, ekeyData, 0, Math.Min(ekeyBytes.Length, 16));
                            entry.EKey = new EKey(ekeyData);
                            stream.Position = currentPos;
                        }
                    }

                    if (entry.HasFileName && (entry.HasCKey || !entry.EKey.IsEmpty))
                    {
                        AddEntry(entry);
                        addedCount++;
                    }
                }

                System.Diagnostics.Trace.TraceInformation($"TVFS: Added {addedCount} entries to root handler");
            }
        }
    }
}