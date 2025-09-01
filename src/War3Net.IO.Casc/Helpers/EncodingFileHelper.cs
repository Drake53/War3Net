// ------------------------------------------------------------------------------
// <copyright file="EncodingFileHelper.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using War3Net.IO.Casc.Cdn;
using War3Net.IO.Casc.Encoding;
using War3Net.IO.Casc.Structures;

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
        /// <param name="logger">The logger instance.</param>
        /// <returns>The path to the cached encoding file, or null if download failed.</returns>
        public static async Task<string?> DownloadEncodingFileAsync(BuildConfig buildConfig, CdnClient cdnClient, string storagePath, ILogger? logger = null)
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
                    logger?.LogInformation("Attempting to download encoding file with hash: {EncodingHash}", encodingHash);
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
        /// <param name="logger">The logger instance.</param>
        /// <returns>The path to the cached root file, or null if download failed.</returns>
        public static async Task<string?> DownloadRootFileAsync(BuildConfig buildConfig, string encodingPath, CdnClient cdnClient, string storagePath, ILogger? logger = null)
        {
            var rootEntry = buildConfig.Root;
            var vfsRoot = buildConfig.VfsRoot;

            // For Warcraft III, prioritize vfs-root over root
            // The vfs-root contains the TVFS directory structure that the root handler needs
            CascKey rootCKey;
            if (!vfsRoot.IsEmpty)
            {
                // Warcraft III uses vfs-root for the TVFS root directory
                rootCKey = vfsRoot.CKey;
                logger?.LogInformation("Using vfs-root for Warcraft III TVFS: {VfsRootCKey}", rootCKey);

                // If VfsRoot has an EKey directly, we can use it without encoding file lookup
                if (vfsRoot.HasEKey)
                {
                    var rootPath = CdnPathHelper.GetLooseFilePath(storagePath, vfsRoot.EKey);
                    if (!File.Exists(rootPath))
                    {
                        CdnPathHelper.EnsureDirectoryExists(rootPath);
                        logger?.LogInformation("Downloading vfs-root file directly with EKey: {VfsRootEKey}", vfsRoot.EKey);
                        try
                        {
                            var rootData = await cdnClient.DownloadDataAsync(vfsRoot.EKey);
                            await File.WriteAllBytesAsync(rootPath, rootData);
                            logger?.LogInformation("VFS root file downloaded successfully");
                        }
                        catch (Exception ex)
                        {
                            logger?.LogWarning(ex, "Failed to download vfs-root file with EKey");
                            return null;
                        }
                    }

                    return rootPath;
                }
            }
            else if (!rootEntry.IsEmpty)
            {
                // Fallback to regular root entry for other games
                rootCKey = rootEntry;
                logger?.LogInformation("Using regular root entry: {RootCKey}", rootCKey);
            }
            else
            {
                logger?.LogWarning("Neither root nor vfs-root entries found in build config");
                return null;
            }

            if (!File.Exists(encodingPath))
            {
                logger?.LogWarning("Encoding file not found at path: {EncodingPath}", encodingPath);
                return null;
            }

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
                    logger?.LogWarning("Could not find EKey for root CKey {RootCKey} in encoding file", rootCKey);
                    return null;
                }

                var rootEKey = foundEKey.Value;
                logger?.LogInformation("Found EKey {RootEKey} for root CKey {RootCKey}", rootEKey, rootCKey);

                var rootPath = CdnPathHelper.GetLooseFilePath(storagePath, rootEKey);

                if (!File.Exists(rootPath))
                {
                    CdnPathHelper.EnsureDirectoryExists(rootPath);

                    // Try to download the root file
                    logger?.LogInformation("Downloading root file with EKey: {RootEKey}", rootEKey);
                    try
                    {
                        var rootData = await cdnClient.DownloadDataAsync(rootEKey);
                        await File.WriteAllBytesAsync(rootPath, rootData);
                        logger?.LogInformation("Root file downloaded successfully, size: {Size} bytes", rootData.Length);
                    }
                    catch (CascFileNotFoundException ex)
                    {
                        logger?.LogError(ex, "Root file with EKey {RootEKey} not found on CDN", rootEKey);
                        return null;
                    }
                    catch (Exception ex)
                    {
                        logger?.LogError(ex, "Failed to download root file with EKey {RootEKey}", rootEKey);
                        return null;
                    }
                }
                else
                {
                    logger?.LogInformation("Root file already cached at: {RootPath}", rootPath);
                }

                return rootPath;
            }
            catch (Exception ex)
            {
                logger?.LogWarning(ex, "Failed to download root file");
                return null;
            }
        }

        /// <summary>
        /// Loads and parses an encoding file from disk.
        /// </summary>
        /// <param name="encodingPath">The path to the encoding file.</param>
        /// <returns>The parsed encoding file, or null if parsing failed.</returns>
        public static EncodingFile LoadEncodingFile(string encodingPath)
        {
            if (!File.Exists(encodingPath))
            {
                throw new ArgumentException("File does not exist.", nameof(encodingPath));
            }

            using var stream = File.OpenRead(encodingPath);
            return EncodingFile.Parse(stream);
        }
    }
}