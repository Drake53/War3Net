// ------------------------------------------------------------------------------
// <copyright file="EncodingFileHelper.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;

using War3Net.IO.Casc.Cdn;
using War3Net.IO.Casc.Encoding;
using War3Net.IO.Casc.Index;

namespace War3Net.IO.Casc.Helpers
{
    /// <summary>
    /// Helper class for encoding file operations.
    /// </summary>
    public static class EncodingFileHelper
    {
        /// <summary>
        /// Downloads and caches the encoding file.
        /// </summary>
        /// <param name="buildConfig">The build configuration.</param>
        /// <param name="cdnClient">The CDN client.</param>
        /// <param name="storagePath">The storage path.</param>
        /// <returns>The path to the cached encoding file, or null if download failed.</returns>
        public static async Task<string?> DownloadEncodingFileAsync(BuildConfig buildConfig, CdnClient cdnClient, string storagePath)
        {
            var encodingEntry = buildConfig.Encoding;

            if (!encodingEntry.HasEKey)
            {
                return null;
            }

            var encodingHash = encodingEntry.EKey;
            var encodingPath = CdnPathHelper.GetLooseFilePath(storagePath, encodingHash);

            if (!File.Exists(encodingPath))
            {
                CdnPathHelper.EnsureDirectoryExists(encodingPath);

                try
                {
                    System.Diagnostics.Trace.TraceInformation($"Attempting to download encoding file with hash: {encodingHash}");
                    var encodingData = await cdnClient.DownloadDataAsync(encodingHash);
                    await File.WriteAllBytesAsync(encodingPath, encodingData);
                }
                catch (CascFileNotFoundException ex)
                {
                    throw new CascException($"Encoding file {encodingHash} not found on CDN: {ex.Message}", ex);
                }
                catch (Exception ex)
                {
                    throw new CascException($"Failed to download encoding file {encodingHash}: {ex.Message}", ex);
                }
            }

            return encodingPath;
        }

        /// <summary>
        /// Downloads the root file using the encoding file to look up its EKey.
        /// </summary>
        /// <param name="buildConfig">The build configuration.</param>
        /// <param name="encodingPath">The path to the encoding file.</param>
        /// <param name="cdnClient">The CDN client.</param>
        /// <param name="storagePath">The storage path.</param>
        /// <param name="fileIndexPath">The path to the file-index file (optional, for Warcraft III).</param>
        /// <returns>The path to the cached root file, or null if download failed.</returns>
        public static async Task<string?> DownloadRootFileAsync(BuildConfig buildConfig, string encodingPath, CdnClient cdnClient, string storagePath, string? fileIndexPath = null)
        {
            var rootEntry = buildConfig.Root;
            var vfsRoot = buildConfig.VfsRoot;

            if (rootEntry.IsEmpty || !File.Exists(encodingPath))
            {
                return null;
            }

            // Root entry is a CascKey that we need to look up in the encoding file
            var rootCKey = rootEntry;

            try
            {
                // Parse the encoding file to get the EKey for the root CKey
                // The encoding file may be BLTE compressed
                EncodingFile? encodingFile;
                using (var encodingStream = File.OpenRead(encodingPath))
                {
                    if (Compression.BlteDecoder.IsBlte(encodingStream))
                    {
                        // BLTE compressed - decompress first
                        using var decompressedStream = new MemoryStream();
                        Compression.BlteDecoder.Decode(encodingStream, decompressedStream);
                        decompressedStream.Position = 0;
                        encodingFile = EncodingFile.Parse(decompressedStream);
                    }
                    else
                    {
                        // Not compressed, parse directly (IsBlte already reset position)
                        encodingFile = EncodingFile.Parse(encodingStream);
                    }
                }

                // Look up the root file's EKey in encoding file
                var foundEKey = encodingFile.GetEKey(rootCKey);

                if (!foundEKey.HasValue || foundEKey.Value.IsEmpty)
                {
                    System.Diagnostics.Trace.TraceWarning($"Could not find EKey for root CKey {rootCKey} in encoding file");
                    return null;
                }

                var rootEKey = foundEKey.Value;

                var rootPath = CdnPathHelper.GetLooseFilePath(storagePath, rootEKey);

                if (!File.Exists(rootPath))
                {
                    CdnPathHelper.EnsureDirectoryExists(rootPath);

                    // For Warcraft III, the root file is in the file-index, not available as a loose file
                    // We need to check if the file is in the file-index first
                    var foundInFileIndex = false;
                    if (!string.IsNullOrEmpty(fileIndexPath) && File.Exists(fileIndexPath))
                    {
                        try
                        {
                            // Parse the file-index to find the root file
                            using var indexStream = File.OpenRead(fileIndexPath);
                            var indexFile = IndexFile.Parse(indexStream);

                            // Try to find the root file's EKey in the index
                            if (indexFile.TryGetEntry(rootEKey, out var entry))
                            {
                                System.Diagnostics.Trace.TraceInformation($"Found root file in file-index: DataFileIndex={entry.DataFileIndex}, Offset={entry.DataFileOffset}, Size={entry.EncodedSize}");

                                // For loose files in file-index, offset should be 0 and DataFileIndex indicates it's not in an archive
                                // The file should be downloadable directly using the EKey
                                if (entry.DataFileOffset == 0)
                                {
                                    // This is a loose file, but it's indexed
                                    // The file-index just confirms it exists as a loose file
                                    foundInFileIndex = true;
                                }
                            }
                            else
                            {
                                System.Diagnostics.Trace.TraceWarning($"Root file EKey {rootEKey} not found in file-index");
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Trace.TraceWarning($"Failed to check file-index for root file: {ex.Message}");
                        }
                    }

                    // Try to download the root file
                    // If it was found in file-index with offset 0, it should be available as a loose file
                    // If not found in file-index, still try as it might be a regular loose file
                    System.Diagnostics.Trace.TraceInformation($"Downloading root file with EKey: {rootEKey}");
                    var rootData = await cdnClient.DownloadDataAsync(rootEKey);
                    await File.WriteAllBytesAsync(rootPath, rootData);
                    System.Diagnostics.Trace.TraceInformation("Root file downloaded successfully");
                }

                return rootPath;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceWarning($"Failed to download root file: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Loads and parses an encoding file from disk.
        /// </summary>
        /// <param name="encodingPath">The path to the encoding file.</param>
        /// <returns>The parsed encoding file, or null if parsing failed.</returns>
        public static EncodingFile? LoadEncodingFile(string encodingPath)
        {
            if (!File.Exists(encodingPath))
            {
                return null;
            }

            try
            {
                using var stream = File.OpenRead(encodingPath);
                return EncodingFile.Parse(stream);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"Failed to parse encoding file: {ex.Message}");
                return null;
            }
        }
    }
}