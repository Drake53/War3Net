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

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using War3Net.IO.Casc.Cdn;
using War3Net.IO.Casc.Encoding;
using War3Net.IO.Casc.Enums;
using War3Net.IO.Casc.Helpers;
using War3Net.IO.Casc.Index;
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
        private const int TotalProgressSteps = 8;

        private readonly ILogger<OnlineCascStorage> _logger;
        private CdnClient? _cdnClient;
        private VersionEntry? _versionEntry;
        private CdnServersEntry? _cdnEntry;
        private BuildConfig? _buildConfig;
        private CdnConfig? _cdnConfig;
        private PatchConfig? _patchConfig;

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
        private OnlineCascStorage(string product, string region, string localCachePath, CascLocaleFlags localeFlags, ILoggerFactory? loggerFactory = null)
            : base(localCachePath, localeFlags, loggerFactory)
        {
            Product = product;
            Region = region;
            _logger = loggerFactory?.CreateLogger<OnlineCascStorage>() ?? NullLogger<OnlineCascStorage>.Instance;

            // Mark this as online storage
            Context.IsOnline = true;
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
        /// Gets the patch configuration.
        /// </summary>
        /// <value>The <see cref="PatchConfig"/> instance, or <see langword="null"/> if not available.</value>
        /// <remarks>
        /// The patch config contains information about patches available to update files from older versions,
        /// reducing redundant downloads. This is optional and may not be present for all products.
        /// </remarks>
        public PatchConfig? PatchConfig => _patchConfig;

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
            string region = CascRegion.EU,
            string? localCachePath = null,
            CascLocaleFlags localeFlags = CascLocaleFlags.All,
            IProgressReporter? progressReporter = null,
            ILoggerFactory? loggerFactory = null)
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

            // Validate known product/region combinations
            if (!CascValidation.IsValidProduct(product))
            {
                throw new ArgumentException($"Invalid product code: '{product}'. Must be a valid product identifier (see CascProduct).", nameof(product));
            }

            if (!CascValidation.IsValidRegion(region))
            {
                throw new ArgumentException($"Invalid region code: '{region}'. Must be a valid region (see CascRegion).", nameof(region));
            }

            // If no cache path provided, use default temp path
            if (string.IsNullOrWhiteSpace(localCachePath))
            {
                // Follow CascLib convention: just product folder in temp
                localCachePath = Path.Combine(Path.GetTempPath(), "CascCache", product);
            }
            else
            {
                // Add product subfolder to the provided path
                localCachePath = Path.Combine(ValidateAndNormalizePath(localCachePath), product);
            }

            Directory.CreateDirectory(localCachePath);

            var storage = new OnlineCascStorage(product, region, localCachePath, localeFlags, loggerFactory);
            await storage.InitializeOnlineAsync(progressReporter);
            return storage;
        }

        /// <summary>
        /// Opens Warcraft III online storage with simplified parameters.
        /// </summary>
        /// <param name="region">The region code (default: <see cref="CascRegion.EU"/>).</param>
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
            string region = CascRegion.EU,
            string? localCachePath = null,
            IProgressReporter? progressReporter = null,
            ILoggerFactory? loggerFactory = null)
        {
            return await OpenStorageAsync(CascProduct.Warcraft.W3, region, localCachePath, CascLocaleFlags.All, progressReporter, loggerFactory);
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
            if (versionEntry is null)
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
            if (cdnEntry is null)
            {
                throw new CascException($"No CDN entry found for region {Region}");
            }

            // Initialize CDN client
            _cdnClient = new CdnClient(cdnEntry.Hosts, cdnEntry.Path);

            // Save for later use
            _versionEntry = versionEntry;
            _cdnEntry = cdnEntry;

            progressReporter?.ReportProgress(CascProgressMessage.DownloadingFile, "build config", 2, TotalProgressSteps);

            // Save versions and cdns files for reference (CascLib pattern)
            var versionsPath = Path.Combine(StoragePath, "versions");
            if (!File.Exists(versionsPath))
            {
                await File.WriteAllBytesAsync(versionsPath, versionsData);
            }

            var cdnsPath = Path.Combine(StoragePath, "cdns");
            if (!File.Exists(cdnsPath))
            {
                await File.WriteAllBytesAsync(cdnsPath, cdnsData);
            }

            // Download and cache build config
            var buildConfigPath = CdnPathHelper.GetConfigPath(StoragePath, versionEntry.BuildConfig);
            if (!File.Exists(buildConfigPath))
            {
                CdnPathHelper.EnsureDirectoryExists(buildConfigPath);
                var buildConfigData = await _cdnClient.DownloadConfigAsync(EKey.Parse(versionEntry.BuildConfig));
                await File.WriteAllBytesAsync(buildConfigPath, buildConfigData);
            }

            progressReporter?.ReportProgress(CascProgressMessage.DownloadingFile, "cdn config", 3, TotalProgressSteps);

            // Download and cache CDN config
            var cdnConfigPath = CdnPathHelper.GetConfigPath(StoragePath, versionEntry.CdnConfig);
            if (!File.Exists(cdnConfigPath))
            {
                CdnPathHelper.EnsureDirectoryExists(cdnConfigPath);
                var cdnConfigData = await _cdnClient.DownloadConfigAsync(EKey.Parse(versionEntry.CdnConfig));
                await File.WriteAllBytesAsync(cdnConfigPath, cdnConfigData);
            }

            // Parse CDN config to get archive information
            CdnConfig cdnConfig;
            using (var stream = File.OpenRead(cdnConfigPath))
            {
                cdnConfig = CdnConfig.Parse(stream);
            }

            _cdnConfig = cdnConfig;

            // Parse build config to get encoding and root hashes
            BuildConfig buildConfig;
            using (var stream = File.OpenRead(buildConfigPath))
            {
                buildConfig = BuildConfig.Parse(stream);
            }

            _buildConfig = buildConfig;

            // Download patch config if present
            if (!buildConfig.PatchConfig.IsEmpty)
            {
                progressReporter?.ReportProgress(CascProgressMessage.DownloadingFile, "patch config", 4, TotalProgressSteps);
                await DownloadPatchConfigAsync(buildConfig.PatchConfig);
            }

            progressReporter?.ReportProgress(CascProgressMessage.LoadingIndices, null, 5, TotalProgressSteps);

            // Download index files FIRST - they contain the EKey mappings needed for other files
            await DownloadIndexFilesAsync(cdnConfig, progressReporter);

            // Load downloaded index files into the index manager
            LoadDownloadedIndexFiles();

            progressReporter?.ReportProgress(CascProgressMessage.DownloadingFile, "encoding", 6, TotalProgressSteps);

            // Download encoding file using the helper
            var encodingPath = await EncodingFileHelper.DownloadEncodingFileAsync(buildConfig, _cdnClient, StoragePath);
            if (string.IsNullOrEmpty(encodingPath))
            {
                throw new CascException("Failed to download encoding file - this is required for online storage");
            }

            // Load the encoding file into the storage context
            LoadDownloadedEncodingFile(encodingPath);

            // Initialize storage context paths early so they're available for TVFS parsing
            InitializeOnlineStorageContext();

            // Download root file
            progressReporter?.ReportProgress(CascProgressMessage.DownloadingFile, "root", 7, TotalProgressSteps);

            // Try to download the root file using the encoding file to look up its EKey
            var rootPath = await EncodingFileHelper.DownloadRootFileAsync(buildConfig, encodingPath, _cdnClient, StoragePath);
            if (!string.IsNullOrEmpty(rootPath))
            {
                _logger.LogInformation("Root file successfully cached at: {RootPath}", rootPath);

                // Try to load and parse the root file
                if (await LoadRootFileAsync(rootPath, buildConfig))
                {
                    _logger.LogInformation("Root file loaded and parsed successfully");
                }
                else
                {
                    _logger.LogWarning("Failed to parse root file - using empty root handler");

                    // Initialize a basic root handler as fallback
                    InitializeRootHandler();
                }
            }
            else
            {
                _logger.LogWarning("Root file could not be downloaded - file name resolution will not be available");

                // Initialize a basic root handler as fallback
                InitializeRootHandler();
            }
        }

        private async Task DownloadPatchConfigAsync(CascKey patchConfigKey)
        {
            try
            {
                var patchConfigPath = CdnPathHelper.GetConfigPath(StoragePath, patchConfigKey.ToString());
                if (!File.Exists(patchConfigPath))
                {
                    CdnPathHelper.EnsureDirectoryExists(patchConfigPath);
                    var patchConfigData = await _cdnClient!.DownloadConfigAsync(EKey.Parse(patchConfigKey.ToString()));
                    await File.WriteAllBytesAsync(patchConfigPath, patchConfigData);
                    _logger.LogInformation("Patch config downloaded successfully: {PatchConfigKey}", patchConfigKey);
                }

                // Parse the patch config
                using (var stream = File.OpenRead(patchConfigPath))
                {
                    _patchConfig = PatchConfig.Parse(stream);
                }

                // Patch manifests are not stored locally - they're only used from CDN
                // The patch system applies patches directly without local caching
                if (!_patchConfig.Patch.IsEmpty)
                {
                    _logger.LogInformation("Patch manifest available: {PatchKey}", _patchConfig.Patch);
                    // Patches will be applied on-demand from CDN when needed
                }
            }
            catch (CascException)
            {
                throw; // Re-throw CascExceptions
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to download or parse patch config");
                // Patch config is optional, so we can continue without it
            }
        }

        private async Task DownloadIndexFilesAsync(CdnConfig cdnConfig, IProgressReporter? progressReporter)
        {
            // Download archive indices
            var archives = cdnConfig.Archives;
            if (archives is null || archives.Count == 0)
            {
                throw new CascException("No archives found in CDN config - cannot proceed without index files");
            }

            // Download all archive indices
            var successfulDownloads = 0;
            var failedDownloads = new List<string>();

            for (var i = 0; i < archives.Count; i++)
            {
                var archiveKey = archives[i];
                if (string.IsNullOrEmpty(archiveKey))
                {
                    continue;
                }

                // For online storage, index files go in data/XX/YY/[hash].index
                var hashLower = archiveKey.ToLowerInvariant();
                var indexPath = Path.Combine(
                    StoragePath,
                    "data",
                    hashLower.Substring(0, 2),
                    hashLower.Substring(2, 2),
                    $"{hashLower}.index");

                if (File.Exists(indexPath))
                {
                    successfulDownloads++;
                    continue;
                }

                progressReporter?.ReportProgress(CascProgressMessage.DownloadingArchiveIndices, $"{hashLower}.index", i, archives.Count);

                try
                {
                    // Ensure directory exists
                    var indexDir = Path.GetDirectoryName(indexPath);
                    if (!string.IsNullOrEmpty(indexDir))
                    {
                        Directory.CreateDirectory(indexDir);
                    }

                    // Download from CDN - the .index extension is added by the CDN client
                    var eKey = EKey.Parse(archiveKey);
                    var indexData = await _cdnClient!.DownloadIndexAsync(eKey);
                    await File.WriteAllBytesAsync(indexPath, indexData);
                    _logger.LogInformation("Successfully downloaded index: {IndexPath}", indexPath);
                    successfulDownloads++;
                }
                catch (HttpRequestException ex)
                {
                    failedDownloads.Add($"{hashLower}.index: {ex.Message}");
                    _logger.LogError(ex, "Failed to download index {IndexFileName}", $"{hashLower}.index");
                }
                catch (IOException ex)
                {
                    throw new CascException($"Failed to save index {hashLower}.index to {indexPath}: {ex.Message}", ex);
                }
            }

            // Download file-index if present BEFORE loading indices
            // The file-index contains full 16-byte EKeys that are needed when parsing TVFS
            var fileIndex = cdnConfig.FileIndex;
            if (!string.IsNullOrEmpty(fileIndex))
            {
                // For online storage, file-index goes in data/XX/YY/[hash].index
                var hashLower = fileIndex.ToLowerInvariant();
                var fileIndexPath = Path.Combine(
                    StoragePath,
                    "data",
                    hashLower.Substring(0, 2),
                    hashLower.Substring(2, 2),
                    $"{hashLower}.index");

                if (!File.Exists(fileIndexPath))
                {
                    try
                    {
                        // Ensure directory exists
                        var indexDir = Path.GetDirectoryName(fileIndexPath);
                        if (!string.IsNullOrEmpty(indexDir))
                        {
                            Directory.CreateDirectory(indexDir);
                        }

                        _logger.LogInformation("Downloading file-index with hash: {FileIndex}", fileIndex);
                        if (_cdnClient is null)
                        {
                            throw new CascException("CDN client not initialized");
                        }

                        var fileIndexData = await _cdnClient.DownloadIndexAsync(EKey.Parse(fileIndex));
                        await File.WriteAllBytesAsync(fileIndexPath, fileIndexData);
                        _logger.LogInformation("File-index downloaded successfully");
                    }
                    catch (HttpRequestException ex)
                    {
                        throw new CascException($"Failed to download file-index {fileIndex}: {ex.Message}", ex);
                    }
                    catch (IOException ex)
                    {
                        throw new CascException($"Failed to save file-index to {fileIndexPath}: {ex.Message}", ex);
                    }
                }
            }

            // Require at least one successful index download
            if (successfulDownloads == 0)
            {
                var failureDetails = string.Join("; ", failedDownloads);
                throw new CascException($"Failed to download any index files. Errors: {failureDetails}");
            }

            _logger.LogInformation("Downloaded {SuccessfulDownloads} of {TotalDownloads} index files", successfulDownloads, archives.Count);
        }

        private static string GetVersionsUrl(string product, string region)
        {
            return $"http://{region}.patch.battle.net:1119/{product}/versions";
        }

        private static string GetCdnUrl(string product, string region)
        {
            return $"http://{region}.patch.battle.net:1119/{product}/cdns";
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

        private void LoadDownloadedIndexFiles()
        {
            // For online storage, we need to load archive index files with their proper archive numbers
            // The archives were downloaded in order from the CDN config
            if (_cdnConfig is null)
            {
                throw new CascException("CDN config not loaded");
            }

            var archives = _cdnConfig.Archives;
            if (archives is null || archives.Count == 0)
            {
                throw new CascException("No archives in CDN config");
            }

            var successfulLoads = 0;

            // Load each archive index with its proper archive number
            for (var i = 0; i < archives.Count; i++)
            {
                var archiveKey = archives[i];
                if (string.IsNullOrEmpty(archiveKey))
                {
                    continue;
                }

                // Construct the path where we downloaded this index
                var hashLower = archiveKey.ToLowerInvariant();
                var indexPath = Path.Combine(
                    StoragePath,
                    "data",
                    hashLower.Substring(0, 2),
                    hashLower.Substring(2, 2),
                    $"{hashLower}.index");

                if (!File.Exists(indexPath))
                {
                    continue; // This index wasn't downloaded
                }

                try
                {
                    // Load with the proper archive number (i)
                    Context.IndexManager?.LoadArchiveIndexFile(indexPath, i);
                    _logger.LogInformation("Loaded archive index {ArchiveNumber}: {IndexFile}", i, Path.GetFileName(indexPath));
                    successfulLoads++;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to load archive index {ArchiveNumber}: {IndexFile}", i, indexPath);
                }
            }

            // Also load the file-index if it exists (for loose files)
            if (!string.IsNullOrEmpty(_cdnConfig.FileIndex))
            {
                var fileIndexHash = _cdnConfig.FileIndex.ToLowerInvariant();
                var fileIndexPath = Path.Combine(
                    StoragePath,
                    "data",
                    fileIndexHash.Substring(0, 2),
                    fileIndexHash.Substring(2, 2),
                    $"{fileIndexHash}.index");

                if (File.Exists(fileIndexPath))
                {
                    try
                    {
                        // File-index doesn't have an archive number, use -1 or special value
                        Context.IndexManager?.LoadArchiveIndexFile(fileIndexPath, -1);
                        _logger.LogInformation("Loaded file-index: {IndexFile}", Path.GetFileName(fileIndexPath));
                        successfulLoads++;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to load file-index: {IndexFile}", fileIndexPath);
                    }
                }
            }

            if (successfulLoads == 0)
            {
                throw new CascException("Failed to load any index files");
            }

            _logger.LogInformation("Loaded {SuccessfulLoads} index files, total entries: {EntryCount}",
                successfulLoads, Context.IndexManager?.TotalEntryCount ?? 0);
        }

        private void LoadDownloadedEncodingFile(string encodingPath)
        {
            if (File.Exists(encodingPath))
            {
                try
                {
                    // Load and parse the encoding file
                    using var stream = File.OpenRead(encodingPath);
                    if (Compression.BlteDecoder.IsBlte(stream))
                    {
                        // BLTE compressed - decompress first
                        using var decompressedStream = new MemoryStream();
                        Compression.BlteDecoder.Decode(stream, decompressedStream);
                        decompressedStream.Position = 0;
                        Context.EncodingFile = EncodingFile.Parse(decompressedStream, _logger);
                        _logger.LogInformation("Encoding file loaded and parsed (BLTE compressed), {EntryCount} entries", Context.EncodingFile.EntryCount);
                    }
                    else
                    {
                        // Not compressed
                        Context.EncodingFile = EncodingFile.Parse(stream, _logger);
                        _logger.LogInformation("Encoding file loaded and parsed (uncompressed), {EntryCount} entries", Context.EncodingFile.EntryCount);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to load encoding file");
                    throw new CascException($"Failed to load encoding file from {encodingPath}: {ex.Message}", ex);
                }
            }
            else
            {
                throw new CascException($"Encoding file does not exist at {encodingPath}");
            }
        }

        private void InitializeOnlineStorageContext()
        {
            // Set up the storage context for online mode
            // For cached online storage, we use a simpler structure:
            // - config/XX/YY/ for config files
            // - data/XX/YY/ for data files and index files

            // Ensure base directories exist
            var configPath = Path.Combine(StoragePath, "config");
            var dataPath = Path.Combine(StoragePath, "data");

            Directory.CreateDirectory(configPath);
            Directory.CreateDirectory(dataPath);

            // Set the paths in the context
            Context.ConfigPath = configPath;
            Context.DataPath = dataPath;

            // Create a minimal .build.info file for the base class
            if (_versionEntry is not null && _buildConfig is not null)
            {
                var buildInfoPath = Path.Combine(StoragePath, ".build.info");
                if (!File.Exists(buildInfoPath))
                {
                    try
                    {
                        CreateMinimalBuildInfo(buildInfoPath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to create .build.info");
                    }
                }
            }

            // The base class Initialize method should be called to complete setup
            // but we're in a derived class context where it's already initialized
            _logger.LogInformation("Online storage context initialized");
        }

        private void CreateMinimalBuildInfo(string buildInfoPath)
        {
            // Create a minimal .build.info file with information from the online storage
            if (_versionEntry is null || _buildConfig is null)
            {
                return;
            }

            var lines = new List<string>
            {
                "Branch!STRING:0|Active!DEC:1|Build Key!HEX:16|CDN Key!HEX:16|Install Key!HEX:16|Product!STRING:0",
                $"eu|1|{_versionEntry.BuildConfig}|{_versionEntry.CdnConfig}||{Product}",
            };

            File.WriteAllLines(buildInfoPath, lines);
        }

        /// <summary>
        /// Reads data from a data file, downloading from CDN if necessary.
        /// </summary>
        /// <param name="indexEntry">The index entry containing file location information.</param>
        /// <returns>The raw file data.</returns>
        protected override async Task<byte[]> ReadDataFileAsync(EKeyEntry indexEntry)
        {
            // Check if this is a file that needs to be downloaded from CDN
            // Following CascLib's pattern: InvalidIndex means the file is not present locally
            if (indexEntry.DataFileIndex == CascConstants.InvalidIndex)
            {
                // This file was registered but not found in indices - try to download it
                _logger.LogInformation("File with EKey {EKey} not in index, attempting CDN download", indexEntry.EKey);

                var eKey = indexEntry.EKey;

                // The EKey should be the full 16-byte version from file-index
                // If we only have a truncated key, we can't download from CDN
                if (eKey.Length != 16)
                {
                    throw new CascException($"Cannot download file from CDN without full 16-byte EKey (have {eKey.Length} bytes). This usually means the file-index was not loaded properly.");
                }

                // First try as a loose file
                var looseFileData = await TryDownloadLooseFileAsync(eKey).ConfigureAwait(false);
                if (looseFileData is not null)
                {
                    return looseFileData;
                }

                throw new CascFileNotFoundException($"File with EKey {eKey} not found on CDN (tried loose and archives)");
            }

            // Check if it's explicitly marked as a loose file (0xFF is our convention)
            if (indexEntry.IsLooseFile)
            {
                try
                {
                    // Loose files are stored as data/XX/YY/hash where hash is the EKey
                    var eKeyString = indexEntry.EKey.ToString().ToLowerInvariant();

                    // todo: code deduplication
                    var loosePath = Path.Combine(
                        Context.DataPath,
                        eKeyString.Substring(0, 2),
                        eKeyString.Substring(2, 2),
                        eKeyString);

                    byte[] encodedData;
                    if (File.Exists(loosePath))
                    {
                        encodedData = File.ReadAllBytes(loosePath);
                    }
                    else
                    {
                        // This is a loose file, download it directly from CDN using its EKey
                        _logger.LogInformation("Downloading loose file from CDN");

                        if (_cdnClient is null)
                        {
                            throw new CascException("CDN client not initialized");
                        }

                        // Download the loose file directly using its EKey
                        encodedData = await _cdnClient.DownloadDataAsync(indexEntry.EKey).ConfigureAwait(false);

                        // Ensure directory exists
                        var directory = Path.GetDirectoryName(loosePath);
                        Directory.CreateDirectory(directory);

                        // Cache the loose file locally for future use
                        await File.WriteAllBytesAsync(loosePath, encodedData).ConfigureAwait(false);
                        _logger.LogInformation("Downloaded and cached loose file {EKey} ({Size} bytes)", indexEntry.EKey, encodedData.Length);
                    }

                    // Check if data is BLTE-encoded and decode if necessary
                    if (Compression.BlteDecoder.IsBlte(encodedData))
                    {
                        using var inputStream = new MemoryStream(encodedData);
                        using var outputStream = new MemoryStream();
                        Compression.BlteDecoder.Decode(inputStream, outputStream);
                        return outputStream.ToArray();
                    }
                    else
                    {
                        return encodedData;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to download loose file {EKey} from CDN", indexEntry.EKey);
                    throw new CascFileNotFoundException($"Loose file not found on CDN: {indexEntry.EKey}");
                }
            }

            try
            {
                return await DownloadFromArchiveAsync(indexEntry).ConfigureAwait(false);
            }
            catch (CascFileNotFoundException)
            {
                var archiveIndex = indexEntry.DataFileIndex;

                // This is an archived file
                var dataFilePath = IndexManager.GetDataFilePath(indexEntry, Context.DataPath);
                _logger.LogInformation("Archive file not found locally at {Path}, attempting CDN download", dataFilePath);

                var archiveKey = _cdnConfig.Archives[(int)archiveIndex];
                _logger.LogInformation("File is in archive {ArchiveIndex} with key {ArchiveKey}", archiveIndex, archiveKey);

                // Download the archive from CDN
                if (_cdnClient is null)
                {
                    throw new CascException("CDN client not initialized - cannot download missing archives");
                }

                try
                {
                    // The data file path for archives is: data/XX/YY/hash
                    // where hash is the archive key
                    var archiveKeyLower = archiveKey.ToLowerInvariant();
                    var archivePath = Path.Combine(
                        Context.DataPath ?? Path.Combine(Context.StoragePath ?? ".", "data"),
                        archiveKeyLower.Substring(0, 2),
                        archiveKeyLower.Substring(2, 2),
                        archiveKeyLower);

                    // Download the archive if it doesn't exist
                    if (!File.Exists(archivePath))
                    {
                        _logger.LogInformation("Downloading archive {ArchiveKey} to {Path}", archiveKey, archivePath);
                        var archiveData = await _cdnClient.DownloadDataAsync(EKey.Parse(archiveKey)).ConfigureAwait(false);

                        // Ensure directory exists
                        var directory = Path.GetDirectoryName(archivePath);
                        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        await File.WriteAllBytesAsync(archivePath, archiveData).ConfigureAwait(false);
                        _logger.LogInformation("Downloaded and cached archive {ArchiveKey} ({Size} bytes)", archiveKey, archiveData.Length);
                    }

                    // Now try reading again
                    return await DownloadFromArchiveAsync(indexEntry).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to download archive {ArchiveKey} from CDN", archiveKey);
                    throw new CascFileNotFoundException($"Archive not found locally and CDN download failed: {archiveKey}");
                }
            }
        }

        /// <summary>
        /// Tries to download a file as a loose file from CDN.
        /// </summary>
        /// <param name="eKey">The encoded key of the file.</param>
        /// <returns>The decoded file data, or null if not found.</returns>
        private async Task<byte[]?> TryDownloadLooseFileAsync(EKey eKey)
        {
            // Check local cache first
            var eKeyString = eKey.ToString().ToLowerInvariant();
            var loosePath = Path.Combine(
                Context.DataPath,
                eKeyString.Substring(0, 2),
                eKeyString.Substring(2, 2),
                eKeyString);

            if (File.Exists(loosePath))
            {
                return await File.ReadAllBytesAsync(loosePath).ConfigureAwait(false);
            }

            if (_cdnClient is null)
            {
                return null;
            }

            try
            {
                _logger.LogInformation("Attempting to download file as loose file from CDN: {EKey}", eKey);

                // Try to download as a loose file
                var encodedData = await _cdnClient.DownloadDataAsync(eKey).ConfigureAwait(false);

                // Cache the file locally
                var directory = Path.GetDirectoryName(loosePath);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                await File.WriteAllBytesAsync(loosePath, encodedData).ConfigureAwait(false);
                _logger.LogInformation("Downloaded and cached loose file {EKey} ({Size} bytes)", eKey, encodedData.Length);

                // Decode if BLTE-encoded
                if (Compression.BlteDecoder.IsBlte(encodedData))
                {
                    using var inputStream = new MemoryStream(encodedData);
                    using var outputStream = new MemoryStream();
                    Compression.BlteDecoder.Decode(inputStream, outputStream);
                    return outputStream.ToArray();
                }
                else
                {
                    return encodedData;
                }
            }
            catch (HttpRequestException ex)
            {
                // File not found as loose file - this is expected for archived files
                _logger.LogDebug("File {EKey} not found as loose file: {Message}", eKey, ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error downloading loose file {EKey}", eKey);
                return null;
            }
        }

        /// <summary>
        /// Downloads a file from an archive.
        /// </summary>
        /// <param name="eKey">The encoded key of the file to download.</param>
        /// <returns>The decoded file data, or null if not found.</returns>
        private async Task<byte[]?> DownloadFromArchiveAsync(EKey eKey)
        {
            if (_cdnClient is null || _cdnConfig is null)
            {
                return null;
            }

            // Go through all archives to find which one contains this file
            var archives = _cdnConfig.Archives;
            for (var archiveIndex = 0; archiveIndex < archives.Count; archiveIndex++)
            {
                var archiveKey = archives[archiveIndex];

                // Check if we have the archive index loaded
                var archiveIndexPath = Path.Combine(
                    StoragePath,
                    "data",
                    archiveKey.Substring(0, 2).ToLowerInvariant(),
                    archiveKey.Substring(2, 2).ToLowerInvariant(),
                    $"{archiveKey.ToLowerInvariant()}.index");

                if (!File.Exists(archiveIndexPath))
                {
                    // Download the archive index if we don't have it
                    try
                    {
                        _logger.LogDebug("Downloading archive index {ArchiveKey} to check for file", archiveKey);
                        var indexData = await _cdnClient.DownloadIndexAsync(EKey.Parse(archiveKey)).ConfigureAwait(false);

                        var dir = Path.GetDirectoryName(archiveIndexPath);
                        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        await File.WriteAllBytesAsync(archiveIndexPath, indexData).ConfigureAwait(false);

                        // Load the newly downloaded index into the IndexManager
                        Context.IndexManager?.LoadArchiveIndexFile(archiveIndexPath, archiveIndex);
                        _logger.LogInformation("Loaded newly downloaded archive index {ArchiveNumber}: {IndexFile}", archiveIndex, Path.GetFileName(archiveIndexPath));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to download archive index {ArchiveKey}", archiveKey);
                        //continue;
                    }
                }

                // Check if this archive contains our file (either already loaded or just downloaded)
                // Now check the IndexManager to see if this file is in this archive
                if (Context.IndexManager?.TryFindEntry(eKey, out var entry) == true && entry.DataFileIndex == archiveIndex)
                {
                    return await DownloadFromArchiveAsync(entry);
                }
            }

            _logger.LogWarning("File with EKey {EKey} not found in any archive", eKey);
            return null;
        }

        private async Task<byte[]> DownloadFromArchiveAsync(EKeyEntry entry)
        {
            var archiveKey = _cdnConfig.Archives[(int)entry.DataFileIndex];

            _logger.LogInformation("Found file with EKey {EKey} in archive {ArchiveKey} at offset {Offset}",
                entry.EKey, archiveKey, entry.DataFileOffset);

            // Download the archive file if needed
            var archivePath = Path.Combine(
                StoragePath,
                "data",
                archiveKey.Substring(0, 2).ToLowerInvariant(),
                archiveKey.Substring(2, 2).ToLowerInvariant(),
                archiveKey.ToLowerInvariant());

            if (!File.Exists(archivePath))
            {
                _logger.LogInformation("Downloading archive {ArchiveKey} to extract file", archiveKey);
                var archiveData = await _cdnClient.DownloadDataAsync(EKey.Parse(archiveKey)).ConfigureAwait(false);

                var archiveDir = Path.GetDirectoryName(archivePath);
                if (!string.IsNullOrEmpty(archiveDir) && !Directory.Exists(archiveDir))
                {
                    Directory.CreateDirectory(archiveDir);
                }

                await File.WriteAllBytesAsync(archivePath, archiveData).ConfigureAwait(false);
            }

            // Extract the file from the archive
            using var archiveStream = File.OpenRead(archivePath);

            var archiveOffset = entry.DataFileOffset;

            // Seek to the file position in the archive
            archiveStream.Seek(archiveOffset, SeekOrigin.Begin);

            // Read the encoded data
            var encodedData = new byte[entry.EncodedSize];
            await archiveStream.ReadAsync(encodedData, 0, (int)entry.EncodedSize).ConfigureAwait(false);

            // Decode if BLTE-encoded
            if (Compression.BlteDecoder.IsBlte(encodedData))
            {
                using var inputStream = new MemoryStream(encodedData);
                using var outputStream = new MemoryStream();
                Compression.BlteDecoder.Decode(inputStream, outputStream);

                _logger.LogInformation("Extracted and decoded file from archive {ArchiveKey} ({EncodedSize} -> {DecodedSize} bytes)",
                    archiveKey, encodedData.Length, outputStream.Length);

                return outputStream.ToArray();
            }
            else
            {
                _logger.LogInformation("Extracted unencoded file from archive {ArchiveKey} ({Size} bytes)",
                    archiveKey, encodedData.Length);

                return encodedData;
            }
        }

        /// <summary>
        /// Caches a downloaded file locally for future use.
        /// </summary>
        /// <param name="eKey">The encoded key of the file.</param>
        /// <param name="data">The encoded file data.</param>
        private async Task CacheDownloadedFileAsync(EKey eKey, byte[] data)
        {
            try
            {
                // Store as a loose file in the cache
                // Format: data/XX/YY/[hash] where XX and YY are first two bytes of hash
                var hashString = eKey.ToString().ToLowerInvariant();
                if (hashString.Length >= 4)
                {
                    var dataDir = Path.Combine(StoragePath, "data", hashString.Substring(0, 2), hashString.Substring(2, 2));
                    if (!Directory.Exists(dataDir))
                    {
                        Directory.CreateDirectory(dataDir);
                    }

                    var filePath = Path.Combine(dataDir, hashString);
                    await File.WriteAllBytesAsync(filePath, data).ConfigureAwait(false);
                    _logger.LogDebug("Cached file with EKey {EKey} to {Path}", eKey, filePath);
                }
            }
            catch (Exception ex)
            {
                // Caching is optional, don't fail if it doesn't work
                _logger.LogWarning(ex, "Failed to cache file with EKey {EKey}", eKey);
            }
        }

        private async Task DownloadVfsManifestFilesAsync(BuildConfig buildConfig)
        {
            // Download VFS manifest files to ensure they are available when parsing subdirectories
            var vfsManifests = buildConfig.GetAllVfsManifests();
            if (vfsManifests.Count == 0)
            {
                _logger.LogDebug("No VFS manifest files found in build config");
                return;
            }

            _logger.LogInformation("Downloading {Count} VFS manifest files", vfsManifests.Count);

            foreach (var manifest in vfsManifests)
            {
                try
                {
                    // Check if the manifest has an EKey
                    if (!manifest.Value.HasEKey)
                    {
                        _logger.LogWarning("VFS manifest {Index} has no EKey, skipping", manifest.Key);
                        continue;
                    }

                    var eKey = manifest.Value.EKey;
                    var eKeyString = eKey.ToString().ToLowerInvariant();

                    // Check if already downloaded
                    var vfsPath = Path.Combine(
                        Context.DataPath ?? Path.Combine(StoragePath, "data"),
                        eKeyString.Substring(0, 2),
                        eKeyString.Substring(2, 2),
                        eKeyString);

                    if (File.Exists(vfsPath))
                    {
                        _logger.LogDebug("VFS manifest {Index} already cached at {Path}", manifest.Key, vfsPath);
                        continue;
                    }

                    // Download from CDN
                    if (_cdnClient is null)
                    {
                        _logger.LogWarning("CDN client not initialized, cannot download VFS manifest {Index}", manifest.Key);
                        continue;
                    }

                    _logger.LogInformation("Downloading VFS manifest {Index} with EKey {EKey}", manifest.Key, eKey);
                    var vfsData = await _cdnClient.DownloadDataAsync(eKey).ConfigureAwait(false);

                    // Ensure directory exists
                    var directory = Path.GetDirectoryName(vfsPath);
                    if (!string.IsNullOrEmpty(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // Cache the file
                    await File.WriteAllBytesAsync(vfsPath, vfsData).ConfigureAwait(false);
                    _logger.LogInformation("Downloaded and cached VFS manifest {Index} ({Size} bytes)", manifest.Key, vfsData.Length);

                    // Register in the index manager for lookup
                    if (Context.IndexManager is not null)
                    {
                        // Register as a loose file so it can be found
                        var entry = Context.IndexManager.RegisterUnknownEKey(eKey);
                        entry.DataFileIndex = 0xFF; // Mark as loose file
                        _logger.LogDebug("Registered VFS manifest {Index} in index manager", manifest.Key);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to download VFS manifest {Index}", manifest.Key);
                    // Continue with other manifests even if one fails
                }
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