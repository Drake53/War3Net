// ------------------------------------------------------------------------------
// <copyright file="CdnClient.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using War3Net.IO.Casc.Utilities;

namespace War3Net.IO.Casc.Cdn
{
    /// <summary>
    /// Client for downloading files from Blizzard CDN.
    /// </summary>
    public class CdnClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly List<string> _cdnHosts;
        private readonly string _cdnPath;
        private readonly bool _ownsHttpClient;
        private int _currentHostIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="CdnClient"/> class.
        /// </summary>
        /// <param name="region">The region code (us, eu, kr, cn, etc.).</param>
        public CdnClient(string region = "eu", string cdnPath = "tpr/war3")
            : this(GetDefaultCdnHosts(region), cdnPath)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CdnClient"/> class.
        /// </summary>
        /// <param name="cdnHosts">The list of CDN hosts.</param>
        /// <param name="cdnPath">The CDN path.</param>
        public CdnClient(List<string> cdnHosts, string cdnPath)
        {
            _cdnHosts = cdnHosts ?? throw new ArgumentNullException(nameof(cdnHosts));
            _cdnPath = cdnPath ?? throw new ArgumentNullException(nameof(cdnPath));
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "War3Net.IO.Casc/1.0");
            _ownsHttpClient = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CdnClient"/> class with custom HttpClient.
        /// </summary>
        /// <param name="httpClient">The HTTP client to use.</param>
        /// <param name="cdnHosts">The list of CDN hosts.</param>
        /// <param name="cdnPath">The CDN path.</param>
        public CdnClient(HttpClient httpClient, List<string> cdnHosts, string cdnPath)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _cdnHosts = cdnHosts ?? throw new ArgumentNullException(nameof(cdnHosts));
            _cdnPath = cdnPath ?? throw new ArgumentNullException(nameof(cdnPath));
            _ownsHttpClient = false;
        }

        /// <summary>
        /// Downloads a file from the CDN.
        /// </summary>
        /// <param name="path">The file path on the CDN.</param>
        /// <returns>The file data.</returns>
        public async Task<byte[]> DownloadFileAsync(string path)
        {
            return await DownloadFileAsync(path, CancellationToken.None);
        }

        /// <summary>
        /// Downloads a file from the CDN.
        /// </summary>
        /// <param name="path">The file path on the CDN.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The file data.</returns>
        public async Task<byte[]> DownloadFileAsync(string path, CancellationToken cancellationToken)
        {
            // Use centralized path sanitization to prevent directory traversal attacks
            path = PathSanitizer.SanitizeCdnPath(path);

            Exception? lastException = null;
            var triedHosts = new HashSet<int>();
            var random = new Random();
            const int maxRetries = 3;
            const int baseDelayMs = 500;
            const int maxDelayMs = 30000; // 30 seconds max delay

            while (triedHosts.Count < _cdnHosts.Count)
            {
                var hostIndex = _currentHostIndex % _cdnHosts.Count;
                if (triedHosts.Contains(hostIndex))
                {
                    _currentHostIndex++;
                    continue;
                }

                triedHosts.Add(hostIndex);
                var url = BuildUrl(_cdnHosts[hostIndex], path);
                var isHttps = url.StartsWith("https://", StringComparison.OrdinalIgnoreCase);

                // Try each host up to maxRetries times with exponential backoff and jitter
                for (var retryCount = 0; retryCount < maxRetries; retryCount++)
                {
                    try
                    {
                        // Apply exponential backoff with jitter (except for first attempt)
                        if (retryCount > 0)
                        {
                            // Calculate exponential backoff with jitter to prevent thundering herd
                            var exponentialDelay = baseDelayMs * (int)Math.Pow(2, retryCount - 1);
                            var jitter = random.Next(0, exponentialDelay / 4); // Add up to 25% jitter
                            var delay = Math.Min(exponentialDelay + jitter, maxDelayMs);

                            await Task.Delay(delay, cancellationToken);
                        }

                        // Set appropriate timeout based on retry count
                        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                        var timeout = TimeSpan.FromSeconds(30 + (retryCount * 15)); // Increase timeout with retries
                        cts.CancelAfter(timeout);

                        var response = await _httpClient.GetAsync(url, cts.Token);
                        if (response.IsSuccessStatusCode)
                        {
                            var data = await response.Content.ReadAsByteArrayAsync();

                            // Validate data is not empty
                            if (data == null || data.Length == 0)
                            {
                                lastException = new CascException($"Downloaded file is empty: {path}");
                                continue; // Retry
                            }

                            return data;
                        }

                        // Handle specific status codes
                        switch ((int)response.StatusCode)
                        {
                            case 404: // Not found - no point retrying this host
                                lastException = new CascFileNotFoundException($"File not found on CDN: {path}");
                                retryCount = maxRetries; // Skip remaining retries for this host
                                break;

                            case 403: // Forbidden - likely auth issue, try next host
                                lastException = new HttpRequestException($"Access forbidden: {response.ReasonPhrase}");
                                retryCount = maxRetries; // Skip remaining retries for this host
                                break;

                            case >= 500: // Server error - retry with this host
                                lastException = new HttpRequestException($"Server error {response.StatusCode}: {response.ReasonPhrase}");
                                break;

                            case 429: // Too many requests - back off more aggressively
                                lastException = new HttpRequestException("Rate limited by CDN");
                                await Task.Delay(Math.Min(baseDelayMs * (int)Math.Pow(3, retryCount), maxDelayMs), cancellationToken);
                                break;

                            default: // Other errors - retry
                                lastException = new HttpRequestException($"HTTP {response.StatusCode}: {response.ReasonPhrase}");
                                break;
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        lastException = ex;
                        // Network error - retry with exponential backoff

                        // If HTTPS failed and we have more retries, consider the error type
                        if (isHttps && retryCount == maxRetries - 1)
                        {
                            // Last retry on HTTPS failed, will try HTTP fallback on next host
                            System.Diagnostics.Trace.TraceWarning($"HTTPS request failed for {url}: {ex.Message}");
                        }
                    }
                    catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
                    {
                        // Timeout (not user cancellation) - retry with exponential backoff
                        lastException = new TimeoutException($"Request to {url} timed out", ex);
                    }
                    catch (TaskCanceledException ex)
                    {
                        // User cancellation - propagate immediately
                        throw new OperationCanceledException("Download cancelled by user", ex, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        lastException = ex;
                        break; // Unexpected error - move to next host
                    }
                }

                _currentHostIndex++;
            }

            throw new CascException($"Failed to download '{path}' from all CDN hosts after {maxRetries} retries per host", lastException);
        }

        /// <summary>
        /// Downloads a file from the CDN to a stream.
        /// </summary>
        /// <param name="path">The file path on the CDN.</param>
        /// <param name="outputStream">The output stream.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task DownloadFileAsync(string path, Stream outputStream, CancellationToken cancellationToken = default)
        {
            var data = await DownloadFileAsync(path, cancellationToken);
            await outputStream.WriteAsync(data, 0, data.Length, cancellationToken);
        }

        /// <summary>
        /// Downloads a config file.
        /// </summary>
        /// <param name="hash">The config file hash.</param>
        /// <returns>The config data.</returns>
        public async Task<byte[]> DownloadConfigAsync(string hash)
        {
            if (string.IsNullOrEmpty(hash))
            {
                throw new ArgumentException("Hash cannot be null or empty", nameof(hash));
            }

            // Config files are stored in config/xx/yy/xxyy...
            var prefix = hash.Substring(0, 2);
            var suffix = hash.Substring(2, 2);
            var path = $"config/{prefix}/{suffix}/{hash}";

            return await DownloadFileAsync(path);
        }

        /// <summary>
        /// Downloads a data file.
        /// </summary>
        /// <param name="hash">The data file hash.</param>
        /// <returns>The data.</returns>
        public async Task<byte[]> DownloadDataAsync(string hash)
        {
            if (string.IsNullOrEmpty(hash))
            {
                throw new ArgumentException("Hash cannot be null or empty", nameof(hash));
            }

            // Data files are stored in data/xx/yy/xxyy...
            var prefix = hash.Substring(0, 2);
            var suffix = hash.Substring(2, 2);
            var path = $"data/{prefix}/{suffix}/{hash}";

            return await DownloadFileAsync(path);
        }

        /// <summary>
        /// Downloads a patch file.
        /// </summary>
        /// <param name="hash">The patch file hash.</param>
        /// <returns>The patch data.</returns>
        public async Task<byte[]> DownloadPatchAsync(string hash)
        {
            if (string.IsNullOrEmpty(hash))
            {
                throw new ArgumentException("Hash cannot be null or empty", nameof(hash));
            }

            // Patch files are stored in patch/xx/yy/xxyy...
            var prefix = hash.Substring(0, 2);
            var suffix = hash.Substring(2, 2);
            var path = $"patch/{prefix}/{suffix}/{hash}";

            return await DownloadFileAsync(path);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (_ownsHttpClient)
            {
                _httpClient?.Dispose();
            }
        }

        private string BuildUrl(string host, string path)
        {
            // Ensure host doesn't end with slash and path doesn't start with slash
            host = host.TrimEnd('/');
            path = path.TrimStart('/');

            // Combine with CDN path
            var cdnPath = _cdnPath.Trim('/');
            return $"{host}/{cdnPath}/{path}";
        }

        private static List<string> GetDefaultCdnHosts(string region)
        {
            // Prefer HTTPS for security, with HTTP fallback
            // Updated URLs based on current Blizzard CDN infrastructure
            return region.ToLowerInvariant() switch
            {
                "us" => new List<string>
                {
                    "https://level3.blizzard.com",
                    "https://blzddist1-a.akamaihd.net",
                    "https://cdn.blizzard.com",
                    "http://level3.blizzard.com",  // HTTP fallback
                    "http://blzddist1-a.akamaihd.net",
                },
                "eu" => new List<string>
                {
                    "https://level3.blizzard.com",
                    "https://blzddist1-a.akamaihd.net",
                    "https://cdn.blizzard.com",
                    "http://level3.blizzard.com",  // HTTP fallback
                    "http://blzddist1-a.akamaihd.net",
                },
                "kr" => new List<string>
                {
                    "https://blzddist1-a.akamaihd.net",
                    "https://blzddistkr1-a.akamaihd.net",
                    "http://blzddist1-a.akamaihd.net",  // HTTP fallback
                    "http://blzddistkr1-a.akamaihd.net",
                },
                "cn" => new List<string>
                {
                    "https://client04.pdl.wow.battlenet.com.cn",
                    "https://client02.pdl.wow.battlenet.com.cn",
                    "http://client04.pdl.wow.battlenet.com.cn",  // HTTP fallback
                    "http://client02.pdl.wow.battlenet.com.cn",
                },
                "tw" => new List<string>
                {
                    "https://level3.blizzard.com",
                    "https://blzddist1-a.akamaihd.net",
                    "http://level3.blizzard.com",  // HTTP fallback
                },
                _ => new List<string>
                {
                    "https://level3.blizzard.com",
                    "https://blzddist1-a.akamaihd.net",
                    "https://cdn.blizzard.com",
                    "http://level3.blizzard.com",  // HTTP fallback
                    "http://blzddist1-a.akamaihd.net",
                },
            };
        }
    }
}