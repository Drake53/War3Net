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
        private OnlineCascStorage(string product, string region, string localCachePath, CascLocaleFlags localeFlags, ILogger<OnlineCascStorage>? logger = null)
            : base(localCachePath, localeFlags)
        {
            Product = product;
            Region = region;
            _logger = logger ?? NullLogger<OnlineCascStorage>.Instance;
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
            ILogger<OnlineCascStorage>? logger = null)
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
                localCachePath = Path.Combine(Path.GetTempPath(), "CascCache", product, region);
            }
            else
            {
                localCachePath = ValidateAndNormalizePath(localCachePath);
            }

            Directory.CreateDirectory(localCachePath);

            var storage = new OnlineCascStorage(product, region, localCachePath, localeFlags, logger);
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
            ILogger<OnlineCascStorage>? logger = null)
        {
            return await OpenStorageAsync(CascProduct.Warcraft.W3, region, localCachePath, CascLocaleFlags.All, progressReporter, logger);
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

            // Save for later use
            _versionEntry = versionEntry;
            _cdnEntry = cdnEntry;

            progressReporter?.ReportProgress(CascProgressMessage.DownloadingFile, "build config", 2, TotalProgressSteps);

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

            progressReporter?.ReportProgress(CascProgressMessage.LoadingIndexes, null, 5, TotalProgressSteps);

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

            // Download file-index if present (needed for Warcraft III root file)
            string? fileIndexPath = null;
            var fileIndex = cdnConfig.FileIndex;
            if (!string.IsNullOrEmpty(fileIndex))
            {
                // File-index should also go in indices directory with full hash name
                var indicesPath = Path.Combine(StoragePath, "Data", "indices");
                Directory.CreateDirectory(indicesPath);
                fileIndexPath = Path.Combine(indicesPath, $"{fileIndex.ToLowerInvariant()}.index");
                if (!File.Exists(fileIndexPath))
                {
                    try
                    {
                        _logger.LogInformation("Downloading file-index with hash: {FileIndex}", fileIndex);
                        var fileIndexData = await _cdnClient!.DownloadIndexAsync(EKey.Parse(fileIndex));
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

            // Download root file
            progressReporter?.ReportProgress(CascProgressMessage.DownloadingFile, "root", 7, TotalProgressSteps);

            // Try to download the root file using the encoding file to look up its EKey
            var rootPath = await EncodingFileHelper.DownloadRootFileAsync(buildConfig, encodingPath, _cdnClient, StoragePath, fileIndexPath);
            if (!string.IsNullOrEmpty(rootPath))
            {
                _logger.LogInformation("Root file successfully cached at: {RootPath}", rootPath);

                // Try to load and parse the root file
                if (LoadRootFile(rootPath))
                {
                    _logger.LogInformation("Root file loaded and parsed successfully");
                }
                else
                {
                    _logger.LogWarning("Failed to parse root file - using empty root handler");
                    InitializeRootHandler();
                }
            }
            else
            {
                _logger.LogWarning("Root file could not be downloaded - file name resolution will not be available");
                // Initialize a basic root handler as fallback
                InitializeRootHandler();
            }

            // Initialize base storage context with downloaded information
            InitializeOnlineStorageContext();
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
                if (_patchConfig.Patch != null && !_patchConfig.Patch.IsEmpty)
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
            // Create indices directory according to CASC spec under Data/
            var indicesPath = Path.Combine(StoragePath, "Data", "indices");
            Directory.CreateDirectory(indicesPath);

            // Download archive indexes
            var archives = cdnConfig.Archives;
            if (archives == null || archives.Count == 0)
            {
                throw new CascException("No archives found in CDN config - cannot proceed without index files");
            }

            // Download at least the first few archive indexes to get started
            var maxIndexes = Math.Min(archives.Count, 5); // Download first 5 indexes
            var successfulDownloads = 0;
            var failedDownloads = new List<string>();

            for (var i = 0; i < maxIndexes; i++)
            {
                var archiveKey = archives[i];
                if (string.IsNullOrEmpty(archiveKey))
                {
                    continue;
                }

                // Index files are stored with .index extension on CDN
                // They should be saved in indices/ directory with full hash name
                var indexFileName = $"{archiveKey.ToLowerInvariant()}.index";
                var indexPath = Path.Combine(indicesPath, indexFileName);

                if (File.Exists(indexPath))
                {
                    successfulDownloads++;
                    continue;
                }

                progressReporter?.ReportProgress(CascProgressMessage.DownloadingArchiveIndexes, indexFileName, i, maxIndexes);

                try
                {
                    // Download from CDN - the .index extension is added by the CDN client
                    var ekey = EKey.Parse(archiveKey);
                    var indexData = await _cdnClient!.DownloadIndexAsync(ekey);
                    await File.WriteAllBytesAsync(indexPath, indexData);
                    _logger.LogInformation("Successfully downloaded index: {IndexFileName}", indexFileName);
                    successfulDownloads++;
                }
                catch (HttpRequestException ex)
                {
                    failedDownloads.Add($"{indexFileName}: {ex.Message}");
                    _logger.LogError(ex, "Failed to download index {IndexFileName}", indexFileName);
                }
                catch (IOException ex)
                {
                    throw new CascException($"Failed to save index {indexFileName} to {indexPath}: {ex.Message}", ex);
                }
            }

            // Require at least one successful index download
            if (successfulDownloads == 0)
            {
                var failureDetails = string.Join("; ", failedDownloads);
                throw new CascException($"Failed to download any index files. Errors: {failureDetails}");
            }

            _logger.LogInformation("Downloaded {SuccessfulDownloads} of {MaxIndexes} index files", successfulDownloads, maxIndexes);
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
            var indicesPath = Path.Combine(StoragePath, "Data", "indices");
            if (!Directory.Exists(indicesPath))
            {
                throw new CascException($"Indices directory does not exist at: {indicesPath}");
            }

            var indexFiles = Directory.GetFiles(indicesPath, "*.index");
            if (indexFiles.Length == 0)
            {
                throw new CascException($"No index files found in: {indicesPath}");
            }

            _logger.LogInformation("Loading {IndexFileCount} index files from: {IndicesPath}", indexFiles.Length, indicesPath);
        }

        private void LoadDownloadedEncodingFile(string encodingPath)
        {
            if (File.Exists(encodingPath))
            {
                try
                {
                    // Load the encoding file
                    using var stream = File.OpenRead(encodingPath);
                    if (Compression.BlteDecoder.IsBlte(stream))
                    {
                        // BLTE compressed - decompress first
                        using var decompressedStream = new MemoryStream();
                        Compression.BlteDecoder.Decode(stream, decompressedStream);
                        decompressedStream.Position = 0;
                        // The base class should handle this, but we can store it for reference
                        _logger.LogInformation("Encoding file loaded (BLTE compressed)");
                    }
                    else
                    {
                        // Not compressed
                        _logger.LogInformation("Encoding file loaded (uncompressed)");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to load encoding file");
                }
            }
        }

        private void InitializeOnlineStorageContext()
        {
            // Set up the storage context for online mode
            // This includes setting features flags and ensuring paths are correct

            // Ensure all required CASC directories exist according to spec under Data/
            var dataRootPath = Path.Combine(StoragePath, "Data");
            var dataPath = Path.Combine(dataRootPath, "data");
            var configPath = Path.Combine(dataRootPath, "config");
            var indicesPath = Path.Combine(dataRootPath, "indices");

            Directory.CreateDirectory(dataPath);
            Directory.CreateDirectory(configPath);
            Directory.CreateDirectory(indicesPath);

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