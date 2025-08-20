// ------------------------------------------------------------------------------
// <copyright file="OnlineCascStorage.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

using War3Net.IO.Casc.Cdn;
using War3Net.IO.Casc.Enums;
using War3Net.IO.Casc.Helpers;
using War3Net.IO.Casc.Progress;
using War3Net.IO.Casc.Structures;

namespace War3Net.IO.Casc.Storage
{
    /// <summary>
    /// Online CASC storage implementation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="OnlineCascStorage"/> provides access to CASC data directly from Blizzard's CDNs without requiring
    /// a full local installation. This implementation follows the TACT (Trusted Application Content Transfer)
    /// protocol for retrieving game data.
    /// </para>
    /// <para>
    /// The online storage workflow:
    /// </para>
    /// <list type="number">
    /// <item><description>Retrieves version information from patch servers (via HTTP or Ribbit protocol)</description></item>
    /// <item><description>Downloads CDN configuration to get available CDN hosts</description></item>
    /// <item><description>Fetches <see cref="Cdn.BuildConfig"/> and <see cref="Cdn.CdnConfig"/> using hashes from version info</description></item>
    /// <item><description>Downloads the <see cref="Encoding.EncodingFile"/> to establish <see cref="Structures.CascKey"/> → <see cref="Structures.EKey"/> mappings</description></item>
    /// <item><description>Retrieves the root file (<see cref="Root.TvfsRootHandler"/> for Warcraft III, MFST for WoW) for filename → <see cref="Structures.CascKey"/> mappings</description></item>
    /// <item><description>Downloads <see cref="Index.IndexFile"/>s for <see cref="Structures.EKey"/> to archive location mappings</description></item>
    /// <item><description>Downloads install and download manifests for file metadata</description></item>
    /// </list>
    /// <para>
    /// Files are retrieved on-demand from CDN using the URL format:
    /// http://(cdnHost)/(cdnPath)/(pathType)/(FirstTwoHex)/(SecondTwoHex)/(FullHash)
    /// </para>
    /// <para>
    /// Where pathType is:
    /// </para>
    /// <list type="bullet">
    /// <item><description>config: Configuration files (<see cref="Cdn.BuildConfig"/>, <see cref="Cdn.CdnConfig"/>, patch configs)</description></item>
    /// <item><description>data: Archives, <see cref="Index.IndexFile"/>s, and standalone files (<see cref="Compression.BlteDecoder"/>-encoded)</description></item>
    /// <item><description>patch: Patch manifests and patch files</description></item>
    /// </list>
    /// <para>
    /// The implementation handles:
    /// </para>
    /// <list type="bullet">
    /// <item><description>CDN failover (trying different CDNs if one fails)</description></item>
    /// <item><description>Local caching of downloaded files to reduce bandwidth</description></item>
    /// <item><description>Rate limiting and HTTP 429 responses from CDNs</description></item>
    /// <item><description>Armadillo encryption for protected content (using .ak key files)</description></item>
    /// </list>
    /// <para>
    /// Supported products include Warcraft III (w3/w3t), World of Warcraft (wow/wowt/wow_classic),
    /// and other Blizzard games that use the CASC/TACT system.
    /// </para>
    /// </remarks>
    public class OnlineCascStorage : CascStorage
    {
        private const int TotalProgressSteps = 6;

        private CdnClient? _cdnClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnlineCascStorage"/> class.
        /// </summary>
        /// <param name="product">The product code (e.g., "w3", "wow", "d3").</param>
        /// <param name="region">The region code (e.g., "us", "eu", "kr", "cn").</param>
        /// <param name="localCachePath">The local cache path for storing downloaded files.</param>
        /// <param name="localeFlags">The locale flags for filtering content.</param>
        /// <remarks>
        /// This constructor is private and used internally by the static factory methods.
        /// Use <see cref="OpenStorageAsync"/> or <see cref="OpenWar3Async"/> to create instances.
        /// </remarks>
        private OnlineCascStorage(string product, string region, string localCachePath, CascLocaleFlags localeFlags)
            : base(localCachePath, localeFlags)
        {
            Product = product;
            Region = region;
        }

        /// <summary>
        /// Gets the product code for this storage instance.
        /// </summary>
        /// <value>The product identifier (e.g., "w3" for Warcraft III, "wow" for World of Warcraft).</value>
        public new string Product { get; }

        /// <summary>
        /// Gets the region code for this storage instance.
        /// </summary>
        /// <value>The region identifier (e.g., "us", "eu", "kr", "cn").</value>
        public string Region { get; }

        /// <summary>
        /// Gets the CDN client used for downloading content.
        /// </summary>
        /// <value>The <see cref="CdnClient"/> instance, or <see langword="null"/> if not initialized.</value>
        /// <remarks>
        /// The CDN client is configured with CDN hosts from the <see cref="Cdn.CdnConfig"/> and handles
        /// downloading of <see cref="Compression.BlteDecoder"/>-encoded files, <see cref="Index.IndexFile"/>s,
        /// and configuration files.
        /// </remarks>
        public CdnClient? CdnClient => _cdnClient;

        /// <summary>
        /// Opens an online CASC storage for the specified product and region.
        /// </summary>
        /// <param name="product">The product code (e.g., "w3", "wow", "d3").</param>
        /// <param name="region">The region code (e.g., "us", "eu", "kr", "cn").</param>
        /// <param name="localCachePath">The local cache path for storing downloaded files, or <see langword="null"/> to use default temp location.</param>
        /// <param name="localeFlags">The locale flags for content filtering.</param>
        /// <param name="progressReporter">Optional progress reporter for tracking initialization steps.</param>
        /// <returns>A fully initialized <see cref="OnlineCascStorage"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when product or region contains invalid characters or path traversal patterns.</exception>
        /// <exception cref="CascException">Thrown when version or CDN configuration cannot be retrieved, or required files cannot be downloaded.</exception>
        /// <remarks>
        /// <para>
        /// This method performs the complete TACT initialization workflow:
        /// </para>
        /// <list type="number">
        /// <item><description>Downloads version information from patch servers</description></item>
        /// <item><description>Downloads CDN configuration</description></item>
        /// <item><description>Downloads <see cref="Cdn.BuildConfig"/> and <see cref="Cdn.CdnConfig"/></description></item>
        /// <item><description>Downloads <see cref="Index.IndexFile"/>s for archive lookups</description></item>
        /// <item><description>Downloads <see cref="Encoding.EncodingFile"/> for key mappings</description></item>
        /// <item><description>Downloads root file (e.g., <see cref="Root.TvfsRootHandler"/> for Warcraft III)</description></item>
        /// </list>
        /// <para>
        /// All downloaded files are cached locally to improve performance on subsequent access.
        /// </para>
        /// </remarks>
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
                "w3", "war3", "wow", "d3", "diablo3", "sc2", "hs", "hearthstone", "hots", "heroes",
            };

            var validRegions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "us", "eu", "kr", "cn", "tw",
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
        /// Opens Warcraft III online storage with simplified parameters.
        /// </summary>
        /// <param name="region">The region code (default: "eu").</param>
        /// <param name="localCachePath">The local cache path for storing downloaded files, or <see langword="null"/> to use default temp location.</param>
        /// <param name="progressReporter">Optional progress reporter for tracking initialization steps.</param>
        /// <returns>A fully initialized <see cref="OnlineCascStorage"/> instance configured for Warcraft III.</returns>
        /// <exception cref="ArgumentException">Thrown when region contains invalid characters or path traversal patterns.</exception>
        /// <exception cref="CascException">Thrown when Warcraft III configuration cannot be retrieved or required files cannot be downloaded.</exception>
        /// <remarks>
        /// <para>
        /// This is a convenience method that calls <see cref="OpenStorageAsync"/> with product="w3"
        /// and <see cref="CascLocaleFlags.All"/>. It initializes the complete Warcraft III CASC system
        /// including <see cref="Root.TvfsRootHandler"/> for file path resolution.
        /// </para>
        /// </remarks>
        public static async Task<OnlineCascStorage> OpenWar3Async(
            string region = "eu",
            string? localCachePath = null,
            IProgressReporter? progressReporter = null)
        {
            return await OpenStorageAsync("w3", region, localCachePath, CascLocaleFlags.All, progressReporter);
        }

        /// <summary>
        /// Disposes the online storage and releases all resources.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method disposes the <see cref="CdnClient"/> and calls the base class disposal.
        /// It's important to dispose the storage when finished to properly release HTTP client resources.
        /// </para>
        /// </remarks>
        public new void Dispose()
        {
            _cdnClient?.Dispose();
            base.Dispose();
        }

        private async Task InitializeOnlineAsync(IProgressReporter? progressReporter)
        {
            progressReporter?.ReportProgress(CascProgressMessage.DownloadingFile, "versions", 0, TotalProgressSteps);

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

            progressReporter?.ReportProgress(CascProgressMessage.DownloadingFile, "cdns", 1, TotalProgressSteps);

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

            progressReporter?.ReportProgress(CascProgressMessage.DownloadingFile, "build config", 2, TotalProgressSteps);

            // Download and cache build config
            var buildConfigPath = CdnPathHelper.GetConfigPath(StoragePath, versionEntry.BuildConfig);
            if (!File.Exists(buildConfigPath))
            {
                CdnPathHelper.EnsureDirectoryExists(buildConfigPath);
                var buildConfigData = await _cdnClient.DownloadConfigAsync(versionEntry.BuildConfig);
                await File.WriteAllBytesAsync(buildConfigPath, buildConfigData);
            }

            progressReporter?.ReportProgress(CascProgressMessage.DownloadingFile, "cdn config", 3, TotalProgressSteps);

            // Download and cache CDN config
            var cdnConfigPath = CdnPathHelper.GetConfigPath(StoragePath, versionEntry.CdnConfig);
            if (!File.Exists(cdnConfigPath))
            {
                CdnPathHelper.EnsureDirectoryExists(cdnConfigPath);
                var cdnConfigData = await _cdnClient.DownloadConfigAsync(versionEntry.CdnConfig);
                await File.WriteAllBytesAsync(cdnConfigPath, cdnConfigData);
            }

            // Parse CDN config to get archive information
            CdnConfig cdnConfig;
            using (var stream = File.OpenRead(cdnConfigPath))
            {
                cdnConfig = CdnConfig.Parse(stream);
            }

            // Parse build config to get encoding and root hashes
            BuildConfig buildConfig;
            using (var stream = File.OpenRead(buildConfigPath))
            {
                buildConfig = BuildConfig.Parse(stream);
            }

            progressReporter?.ReportProgress(CascProgressMessage.LoadingIndexes, null, 4, TotalProgressSteps);

            // Download index files FIRST - they contain the EKey mappings needed for other files
            await DownloadIndexFilesAsync(cdnConfig, progressReporter);

            progressReporter?.ReportProgress(CascProgressMessage.DownloadingFile, "encoding", 5, TotalProgressSteps);

            // Download encoding file using the helper
            var encodingPath = await EncodingFileHelper.DownloadEncodingFileAsync(buildConfig, _cdnClient, StoragePath);
            if (string.IsNullOrEmpty(encodingPath))
            {
                throw new CascException("Failed to download encoding file - this is required for online storage");
            }

            // Download root file
            progressReporter?.ReportProgress(CascProgressMessage.DownloadingFile, "root", 6, TotalProgressSteps);

            // Try to download the root file using the encoding file to look up its EKey
            var rootPath = await EncodingFileHelper.DownloadRootFileAsync(buildConfig, encodingPath, _cdnClient, StoragePath);
            if (!string.IsNullOrEmpty(rootPath))
            {
                System.Diagnostics.Trace.TraceInformation($"Root file successfully cached at: {rootPath}");

                // Try to load and parse the root file
                if (LoadRootFile(rootPath))
                {
                    System.Diagnostics.Trace.TraceInformation("Root file loaded and parsed successfully");
                }
                else
                {
                    System.Diagnostics.Trace.TraceWarning("Failed to parse root file - using empty root handler");
                    InitializeRootHandler();
                }
            }
            else
            {
                System.Diagnostics.Trace.TraceWarning("Root file could not be downloaded - file name resolution will not be available");
                // Initialize a basic root handler as fallback
                InitializeRootHandler();
            }

            // Continue with base initialization
            // The base class will handle loading the downloaded files
        }

        private async Task DownloadIndexFilesAsync(CdnConfig cdnConfig, IProgressReporter? progressReporter)
        {
            var dataPath = Path.Combine(StoragePath, "data");
            Directory.CreateDirectory(dataPath);

            // Download archive indexes
            var archives = cdnConfig.Archives;
            if (archives == null || archives.Count == 0)
            {
                System.Diagnostics.Trace.TraceWarning("No archives found in CDN config");
                return;
            }

            // Download at least the first few archive indexes to get started
            var maxIndexes = Math.Min(archives.Count, 5); // Download first 5 indexes
            for (var i = 0; i < maxIndexes; i++)
            {
                var archiveKey = archives[i];
                if (string.IsNullOrEmpty(archiveKey))
                {
                    continue;
                }

                // Index files are stored with .index extension on CDN
                // They should be saved with .idx extension locally
                var indexKey = archiveKey + ".index";
                var indexFileName = $"{archiveKey.Substring(0, Math.Min(16, archiveKey.Length))}.idx";
                var indexPath = Path.Combine(dataPath, indexFileName);

                if (!File.Exists(indexPath))
                {
                    progressReporter?.ReportProgress(CascProgressMessage.DownloadingArchiveIndexes, indexFileName, i, maxIndexes);

                    try
                    {
                        // Download from CDN - the path will be data/xx/yy/{hash}.index
                        var indexData = await _cdnClient!.DownloadDataAsync(indexKey);
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

            // Also download file index if present
            var fileIndex = cdnConfig.FileIndex;
            if (!string.IsNullOrEmpty(fileIndex))
            {
                var fileIndexPath = Path.Combine(dataPath, $"{fileIndex.Substring(0, Math.Min(16, fileIndex.Length))}.idx");
                if (!File.Exists(fileIndexPath))
                {
                    try
                    {
                        var fileIndexData = await _cdnClient!.DownloadDataAsync(fileIndex + ".index");
                        await File.WriteAllBytesAsync(fileIndexPath, fileIndexData);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceWarning($"Failed to download file index: {ex.Message}");
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
                product.Contains("..", StringComparison.Ordinal) ||
                product.Contains("/", StringComparison.Ordinal) ||
                product.Contains("\\", StringComparison.Ordinal) ||
                product.Length > 50) // Reasonable length limit
            {
                throw new ArgumentException($"Invalid product name: '{product}'. Must be a valid directory name without path separators.", nameof(product));
            }

            if (string.IsNullOrWhiteSpace(region) ||
                region.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 ||
                region.Contains("..", StringComparison.Ordinal) ||
                region.Contains("/", StringComparison.Ordinal) ||
                region.Contains("\\", StringComparison.Ordinal) ||
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
            if (normalizedPath.Contains(':', StringComparison.Ordinal) && !Path.IsPathRooted(normalizedPath))
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
            var isInSafeDirectory =
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