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
        /// <returns>The path to the cached encoding file, or null if download failed.</returns>
        public static async Task<string?> DownloadEncodingFileAsync(BuildConfig buildConfig, CdnClient cdnClient, string storagePath)
        {
            var encodingEntry = buildConfig.GetValue("encoding");
            if (string.IsNullOrEmpty(encodingEntry))
            {
                return null;
            }

            var encodingParts = encodingEntry.Split(' ');
            // Use the second hash (ekey) for downloading the encoding file
            var encodingHash = encodingParts.Length > 1 ? encodingParts[1] : encodingParts[0];

            if (string.IsNullOrEmpty(encodingHash))
            {
                return null;
            }

            var encodingPath = CdnPathHelper.GetDataPath(storagePath, encodingHash);

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
        /// <returns>The path to the cached root file, or null if download failed.</returns>
        public static async Task<string?> DownloadRootFileAsync(BuildConfig buildConfig, string encodingPath, CdnClient cdnClient, string storagePath)
        {
            var rootCKeyString = buildConfig.Root;
            if (string.IsNullOrEmpty(rootCKeyString) || !File.Exists(encodingPath))
            {
                return null;
            }

            try
            {
                // Parse the encoding file to get the EKey for the root CKey
                EncodingFile? encodingFile;
                using (var encodingStream = File.OpenRead(encodingPath))
                {
                    encodingFile = EncodingFile.Parse(encodingStream);
                }

                // Look up the root file's EKey
                var rootCKey = CascKey.Parse(rootCKeyString);
                var rootEKey = encodingFile.GetEKey(rootCKey);

                if (!rootEKey.HasValue || rootEKey.Value.IsEmpty)
                {
                    System.Diagnostics.Trace.TraceWarning($"Could not find EKey for root CKey {rootCKeyString} in encoding file");
                    return null;
                }

                var rootPath = CdnPathHelper.GetDataPath(storagePath, rootEKey.Value.ToString());

                if (!File.Exists(rootPath))
                {
                    CdnPathHelper.EnsureDirectoryExists(rootPath);
                    System.Diagnostics.Trace.TraceInformation($"Downloading root file with EKey: {rootEKey.Value}");
                    var rootData = await cdnClient.DownloadDataAsync(rootEKey.Value.ToString());
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