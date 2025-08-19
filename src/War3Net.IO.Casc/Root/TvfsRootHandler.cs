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
    /// <remarks>
    /// <para>
    /// TVFS is Warcraft III's implementation of the root file system in TACT. Unlike World of Warcraft's
    /// MFST (manifest) format, TVFS uses a more structured approach with separate tables for different
    /// data types. This handler is used by <see cref="Storage.OnlineCascStorage"/> to resolve file paths
    /// to <see cref="CascKey"/>s for lookup in the <see cref="Encoding.EncodingFile"/>.
    /// </para>
    /// <para>
    /// TVFS file structure:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Header: Contains signature "TVFS", version info, and offsets to various tables</description></item>
    /// <item><description>Path table: Contains null-terminated strings of file paths</description></item>
    /// <item><description>VFS table: Maps path indices to content and encoding key indices</description></item>
    /// <item><description>CKey table: Contains <see cref="CascKey"/>s (content hashes) for files</description></item>
    /// <item><description>EKey table: Contains <see cref="EKey"/>s (encoding hashes) for files</description></item>
    /// <item><description>Patch EKey table: Contains <see cref="EKey"/>s for patch files</description></item>
    /// <item><description>Directory manifest: Optional directory structure information</description></item>
    /// </list>
    /// <para>
    /// Each VFS entry links a file path to its corresponding <see cref="CascKey"/> and <see cref="EKey"/>,
    /// establishing the complete mapping from filename → <see cref="CascKey"/> → <see cref="EKey"/>
    /// needed to retrieve files from the CASC storage through <see cref="Index.IndexFile"/>s.
    /// </para>
    /// <para>
    /// TVFS is referenced in the <see cref="Cdn.BuildConfig"/> as "vfs-root" and is a key file that's typically
    /// stored as a loose file on the CDN for quick access. It's essential for translating human-readable
    /// file paths to the hashes needed to retrieve actual file data through <see cref="Compression.BlteDecoder"/>.
    /// </para>
    /// </remarks>
    public class TvfsRootHandler : RootHandlerBase
    {
        private const uint TvfsSignature = 0x53465654; // 'TVFS' - bytes: 54 56 46 53 in little-endian

        /// <summary>
        /// Parses the TVFS root file from a stream.
        /// </summary>
        /// <param name="stream">The stream containing TVFS root file data.</param>
        /// <exception cref="InvalidDataException">Thrown when the TVFS signature is invalid.</exception>
        /// <remarks>
        /// <para>
        /// This method reads the entire TVFS structure including all tables and creates <see cref="RootEntry"/>
        /// instances that map file paths to their <see cref="CascKey"/>s and <see cref="EKey"/>s.
        /// </para>
        /// <para>
        /// The parsed entries are used by <see cref="Storage.OnlineCascStorage"/> to resolve file names
        /// to content keys, which are then looked up in the <see cref="Encoding.EncodingFile"/> to find
        /// the corresponding <see cref="EKey"/>s for retrieval from <see cref="Index.IndexFile"/>s.
        /// </para>
        /// </remarks>
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