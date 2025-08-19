// ------------------------------------------------------------------------------
// <copyright file="OnlineCascStorage.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

using War3Net.IO.Casc.Cdn;
using War3Net.IO.Casc.Enums;
using War3Net.IO.Casc.Progress;

namespace War3Net.IO.Casc.Storage
{
    /// <summary>
    /// Online CASC storage implementation.
    /// </summary>
    public class OnlineCascStorage : CascStorage
    {
        private CdnClient? _cdnClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnlineCascStorage"/> class.
        /// </summary>
        /// <param name="product">The product code (e.g., "w3", "wow", "d3").</param>
        /// <param name="region">The region (e.g., "us", "eu", "kr", "cn").</param>
        /// <param name="localCachePath">The local cache path.</param>
        /// <param name="localeFlags">The locale flags.</param>
        private OnlineCascStorage(string product, string region, string localCachePath, CascLocaleFlags localeFlags)
            : base(localCachePath, localeFlags)
        {
            Product = product;
            Region = region;
        }

        /// <summary>
        /// Gets the product code.
        /// </summary>
        public new string Product { get; }

        /// <summary>
        /// Gets the region.
        /// </summary>
        public string Region { get; }

        /// <summary>
        /// Gets the CDN client.
        /// </summary>
        public CdnClient? CdnClient => _cdnClient;

        /// <summary>
        /// Opens an online CASC storage.
        /// </summary>
        /// <param name="product">The product code.</param>
        /// <param name="region">The region.</param>
        /// <param name="localCachePath">The local cache path.</param>
        /// <param name="localeFlags">The locale flags.</param>
        /// <param name="progressReporter">Optional progress reporter.</param>
        /// <returns>The opened storage.</returns>
        public static async Task<OnlineCascStorage> OpenStorageAsync(
            string product,
            string region = "eu",
            string? localCachePath = null,
            CascLocaleFlags localeFlags = CascLocaleFlags.All,
            IProgressReporter? progressReporter = null)
        {
            // Validate and sanitize product and region to prevent path traversal
            ValidateProductAndRegion(product, region);
            
            // Decode any URL-encoded sequences first
            product = HttpUtility.UrlDecode(product);
            region = HttpUtility.UrlDecode(region);
            
            // Re-validate after decoding
            ValidateProductAndRegion(product, region);
            
            // Additional checks for various path traversal patterns
            var pathTraversalPatterns = new[] 
            {
                "..", "../", "..\\", 
                "%2e%2e", "%2e%2e%2f", "%2e%2e%5c",
                "..%2f", "..%5c",
                ".%2e", "%2e.",
                ":", // Alternate data streams on Windows
                "$", "~", // Shell expansion characters
            };
            
            foreach (var pattern in pathTraversalPatterns)
            {
                if (product.Contains(pattern, StringComparison.OrdinalIgnoreCase) ||
                    region.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                {
                    throw new ArgumentException($"Invalid characters or patterns detected in product or region.");
                }
            }
            
            // Validate using regex for extra safety
            var safeNamePattern = @"^[a-zA-Z0-9_-]+$";
            if (!Regex.IsMatch(product, safeNamePattern) || !Regex.IsMatch(region, safeNamePattern))
            {
                throw new ArgumentException($"Product and region must contain only alphanumeric characters, hyphens, and underscores.");
            }

            // Additional validation for known product/region combinations
            var validProducts = new HashSet<string>(StringComparer.OrdinalIgnoreCase) 
            { 
                "w3", "war3", "wow", "d3", "diablo3", "sc2", "hs", "hearthstone", "hots", "heroes"
            };
            
            var validRegions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) 
            { 
                "us", "eu", "kr", "cn", "tw", "sea"
            };

            if (!validProducts.Contains(product.ToLowerInvariant()))
            {
                System.Diagnostics.Trace.TraceWarning($"Unknown product '{product}' - proceeding anyway");
            }

            if (!validRegions.Contains(region.ToLowerInvariant()))
            {
                System.Diagnostics.Trace.TraceWarning($"Unknown region '{region}' - proceeding anyway");
            }

            // If no cache path provided, use default temp path
            if (string.IsNullOrWhiteSpace(localCachePath))
            {
                localCachePath = Path.Combine(Path.GetTempPath(), "CascCache", product, region);
            }
            else
            {
                localCachePath = ValidateAndNormalizePath(localCachePath);
            }

            Directory.CreateDirectory(localCachePath);

            var storage = new OnlineCascStorage(product, region, localCachePath, localeFlags);
            await storage.InitializeOnlineAsync(progressReporter);
            return storage;
        }

        /// <summary>
        /// Opens Warcraft III online storage.
        /// </summary>
        /// <param name="region">The region.</param>
        /// <param name="localCachePath">The local cache path.</param>
        /// <param name="progressReporter">Optional progress reporter.</param>
        /// <returns>The opened storage.</returns>
        public static async Task<OnlineCascStorage> OpenWar3Async(
            string region = "eu",
            string? localCachePath = null,
            IProgressReporter? progressReporter = null)
        {
            return await OpenStorageAsync("w3", region, localCachePath, CascLocaleFlags.All, progressReporter);
        }

        /// <summary>
        /// Disposes the online storage.
        /// </summary>
        public new void Dispose()
        {
            _cdnClient?.Dispose();
            base.Dispose();
        }

        private async Task InitializeOnlineAsync(IProgressReporter? progressReporter)
        {
            progressReporter?.ReportProgress(CascProgressMessage.DownloadingFile, "versions", 0, 4);

            // Download versions file
            var versionsUrl = GetVersionsUrl(Product, Region);
            byte[] versionsData;
            using (var httpClient = new HttpClient())
            {
                versionsData = await httpClient.GetByteArrayAsync(versionsUrl);
            }

            // Parse versions
            VersionConfig versions;
            using (var stream = new MemoryStream(versionsData))
            {
                versions = VersionConfig.Parse(stream);
            }

            var versionEntry = versions.GetEntry(Region) ?? versions.GetFirstEntry();
            if (versionEntry == null)
            {
                throw new CascException($"No version entry found for region {Region}");
            }

            progressReporter?.ReportProgress(CascProgressMessage.DownloadingFile, "cdns", 1, 4);

            // Download CDNs file
            var cdnUrl = GetCdnUrl(Product, Region);
            byte[] cdnsData;
            using (var httpClient = new HttpClient())
            {
                cdnsData = await httpClient.GetByteArrayAsync(cdnUrl);
            }

            // Parse CDNs
            CdnServersConfig cdns;
            using (var stream = new MemoryStream(cdnsData))
            {
                cdns = CdnServersConfig.Parse(stream);
            }

            var cdnEntry = cdns.GetEntry(Region) ?? cdns.GetFirstEntry();
            if (cdnEntry == null)
            {
                throw new CascException($"No CDN entry found for region {Region}");
            }

            // Initialize CDN client
            _cdnClient = new CdnClient(cdnEntry.Hosts, cdnEntry.Path);

            progressReporter?.ReportProgress(CascProgressMessage.DownloadingFile, "build config", 2, 4);

            // Download and cache build config
            var buildConfigPath = Path.Combine(StoragePath, "config", versionEntry.BuildConfig);
            if (!File.Exists(buildConfigPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(buildConfigPath)!);
                var buildConfigData = await _cdnClient.DownloadConfigAsync(versionEntry.BuildConfig);
                await File.WriteAllBytesAsync(buildConfigPath, buildConfigData);
            }

            progressReporter?.ReportProgress(CascProgressMessage.DownloadingFile, "cdn config", 3, 4);

            // Download and cache CDN config
            var cdnConfigPath = Path.Combine(StoragePath, "config", versionEntry.CdnConfig);
            if (!File.Exists(cdnConfigPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(cdnConfigPath)!);
                var cdnConfigData = await _cdnClient.DownloadConfigAsync(versionEntry.CdnConfig);
                await File.WriteAllBytesAsync(cdnConfigPath, cdnConfigData);
            }

            // Parse CDN config to get archive information
            CdnConfig cdnConfig;
            using (var stream = File.OpenRead(cdnConfigPath))
            {
                cdnConfig = CdnConfig.Parse(stream);
            }

            progressReporter?.ReportProgress(CascProgressMessage.LoadingIndexes, null, 4, 4);

            // Download index files if needed
            await DownloadIndexFilesAsync(cdnConfig, progressReporter);

            // Continue with base initialization
            // The base class will handle loading the downloaded files
        }

        private async Task DownloadIndexFilesAsync(CdnConfig cdnConfig, IProgressReporter? progressReporter)
        {
            var dataPath = Path.Combine(StoragePath, "data");
            Directory.CreateDirectory(dataPath);

            // Download archive indexes
            var archives = cdnConfig.Archives;
            for (int i = 0; i < Math.Min(archives.Count, 16); i++) // Limit to first 16 archives
            {
                var archiveKey = archives[i];
                if (string.IsNullOrEmpty(archiveKey))
                {
                    continue;
                }

                var indexFileName = $"{archiveKey.Substring(0, 16)}.idx";
                var indexPath = Path.Combine(dataPath, indexFileName);

                if (!File.Exists(indexPath))
                {
                    progressReporter?.ReportProgress(CascProgressMessage.DownloadingArchiveIndexes, indexFileName, i, archives.Count);

                    try
                    {
                        var indexData = await _cdnClient!.DownloadDataAsync(archiveKey + ".index");
                        await File.WriteAllBytesAsync(indexPath, indexData);
                    }
                    catch (HttpRequestException ex)
                    {
                        // Log HTTP errors but continue - not all indexes may be available
                        System.Diagnostics.Trace.TraceWarning($"Failed to download index {indexFileName}: HTTP error - {ex.Message}");
                    }
                    catch (IOException ex)
                    {
                        // Log IO errors with more detail
                        System.Diagnostics.Trace.TraceError($"Failed to save index {indexFileName} to {indexPath}: {ex.Message}");
                        throw new CascException($"Unable to save index file: {ex.Message}", ex);
                    }
                    catch (Exception ex)
                    {
                        // Log unexpected errors
                        System.Diagnostics.Trace.TraceError($"Unexpected error downloading index {indexFileName}: {ex.GetType().Name} - {ex.Message}");
                    }
                }
            }
        }

        private static string GetVersionsUrl(string product, string region)
        {
            return product.ToLowerInvariant() switch
            {
                "w3" or "war3" => $"http://{region}.patch.battle.net:1119/w3/versions",
                "wow" => $"http://{region}.patch.battle.net:1119/wow/versions",
                "d3" or "diablo3" => $"http://{region}.patch.battle.net:1119/d3/versions",
                "hs" or "hearthstone" => $"http://{region}.patch.battle.net:1119/hs/versions",
                "sc2" => $"http://{region}.patch.battle.net:1119/sc2/versions",
                "hots" or "heroes" => $"http://{region}.patch.battle.net:1119/hero/versions",
                _ => throw new ArgumentException($"Unknown product: {product}"),
            };
        }

        private static string GetCdnUrl(string product, string region)
        {
            return product.ToLowerInvariant() switch
            {
                "w3" or "war3" => $"http://{region}.patch.battle.net:1119/w3/cdns",
                "wow" => $"http://{region}.patch.battle.net:1119/wow/cdns",
                "d3" or "diablo3" => $"http://{region}.patch.battle.net:1119/d3/cdns",
                "hs" or "hearthstone" => $"http://{region}.patch.battle.net:1119/hs/cdns",
                "sc2" => $"http://{region}.patch.battle.net:1119/sc2/cdns",
                "hots" or "heroes" => $"http://{region}.patch.battle.net:1119/hero/cdns",
                _ => throw new ArgumentException($"Unknown product: {product}"),
            };
        }
        
        private static void ValidateProductAndRegion(string product, string region)
        {
            if (string.IsNullOrWhiteSpace(product) || 
                product.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 || 
                product.Contains("..") || 
                product.Contains("/") || 
                product.Contains("\\") ||
                product.Length > 50) // Reasonable length limit
            {
                throw new ArgumentException($"Invalid product name: '{product}'. Must be a valid directory name without path separators.", nameof(product));
            }

            if (string.IsNullOrWhiteSpace(region) || 
                region.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 || 
                region.Contains("..") || 
                region.Contains("/") || 
                region.Contains("\\") ||
                region.Length > 10) // Reasonable length limit for region codes
            {
                throw new ArgumentException($"Invalid region name: '{region}'. Must be a valid region code (e.g., 'us', 'eu', 'kr').", nameof(region));
            }
        }
        
        private static string ValidateAndNormalizePath(string path)
        {
            // Decode any URL-encoded sequences
            var decodedPath = HttpUtility.UrlDecode(path);
            
            // Check for path traversal patterns before normalization
            var pathTraversalPatterns = new[] 
            {
                "..", "../", "..\\",
                "%2e%2e", "%2e%2e%2f", "%2e%2e%5c",
                "..%2f", "..%5c",
                ".%2e", "%2e.",
            };
            
            foreach (var pattern in pathTraversalPatterns)
            {
                if (decodedPath.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                {
                    throw new ArgumentException($"Path contains invalid traversal pattern: {pattern}");
                }
            }
            
            // Normalize the path
            string normalizedPath;
            try
            {
                normalizedPath = Path.GetFullPath(decodedPath);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Invalid cache path: {ex.Message}", nameof(path), ex);
            }
            
            // Check for alternate data streams (Windows)
            if (normalizedPath.Contains(':') && !Path.IsPathRooted(normalizedPath))
            {
                throw new ArgumentException($"Path cannot contain alternate data stream syntax");
            }
            
            // Ensure the resolved path doesn't escape expected boundaries
            var tempPath = Path.GetFullPath(Path.GetTempPath());
            var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var programData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            
            // Allow paths only within safe directories
            bool isInSafeDirectory = 
                normalizedPath.StartsWith(tempPath, StringComparison.OrdinalIgnoreCase) ||
                normalizedPath.StartsWith(userProfile, StringComparison.OrdinalIgnoreCase) ||
                normalizedPath.StartsWith(appData, StringComparison.OrdinalIgnoreCase) ||
                normalizedPath.StartsWith(localAppData, StringComparison.OrdinalIgnoreCase) ||
                normalizedPath.StartsWith(programData, StringComparison.OrdinalIgnoreCase);

            if (!isInSafeDirectory)
            {
                throw new ArgumentException($"Cache path must be within temp directory, user profile, or application data directories");
            }

            // Additional check: ensure the path doesn't contain shell expansion characters after normalization
            var shellChars = new[] { '~', '$', '`', '!', '&', '|', ';' };
            if (normalizedPath.IndexOfAny(shellChars) >= 0)
            {
                throw new ArgumentException($"Cache path cannot contain special shell characters");
            }
            
            return normalizedPath;
        }
    }
}