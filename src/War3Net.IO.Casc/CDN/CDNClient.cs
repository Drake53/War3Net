// ------------------------------------------------------------------------------
// <copyright file="CDNClient.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace War3Net.IO.Casc.CDN
{
    /// <summary>
    /// Client for downloading files from Blizzard CDN.
    /// </summary>
    public class CDNClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly List<string> _cdnHosts;
        private readonly string _cdnPath;
        private readonly bool _ownsHttpClient;
        private int _currentHostIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="CDNClient"/> class.
        /// </summary>
        /// <param name="region">The region code (us, eu, kr, cn, etc.).</param>
        public CDNClient(string region = "us")
            : this(GetDefaultCDNHosts(region), GetDefaultCDNPath())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CDNClient"/> class.
        /// </summary>
        /// <param name="cdnHosts">The list of CDN hosts.</param>
        /// <param name="cdnPath">The CDN path.</param>
        public CDNClient(List<string> cdnHosts, string cdnPath)
        {
            _cdnHosts = cdnHosts ?? throw new ArgumentNullException(nameof(cdnHosts));
            _cdnPath = cdnPath ?? throw new ArgumentNullException(nameof(cdnPath));
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "War3Net.IO.Casc/1.0");
            _ownsHttpClient = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CDNClient"/> class with custom HttpClient.
        /// </summary>
        /// <param name="httpClient">The HTTP client to use.</param>
        /// <param name="cdnHosts">The list of CDN hosts.</param>
        /// <param name="cdnPath">The CDN path.</param>
        public CDNClient(HttpClient httpClient, List<string> cdnHosts, string cdnPath)
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
            Exception? lastException = null;
            var triedHosts = new HashSet<int>();

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

                try
                {
                    var response = await _httpClient.GetAsync(url, cancellationToken);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsByteArrayAsync();
                    }

                    // Try next host on non-success
                    lastException = new HttpRequestException($"HTTP {response.StatusCode}: {response.ReasonPhrase}");
                }
                catch (Exception ex)
                {
                    lastException = ex;
                }

                _currentHostIndex++;
            }

            throw new CascException($"Failed to download {path} from all CDN hosts", lastException);
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

        private static List<string> GetDefaultCDNHosts(string region)
        {
            return region.ToLowerInvariant() switch
            {
                "us" => new List<string>
                {
                    "http://level3.blizzard.com",
                    "http://blzddist1-a.akamaihd.net",
                    "http://cdn.blizzard.com",
                },
                "eu" => new List<string>
                {
                    "http://level3.blizzard.com",
                    "http://blzddist1-a.akamaihd.net",
                    "http://cdn.blizzard.com",
                },
                "kr" => new List<string>
                {
                    "http://blzddist1-a.akamaihd.net",
                    "http://blzddistkr1-a.akamaihd.net",
                },
                "cn" => new List<string>
                {
                    "http://client04.pdl.wow.battlenet.com.cn",
                    "http://client02.pdl.wow.battlenet.com.cn",
                },
                _ => new List<string>
                {
                    "http://level3.blizzard.com",
                    "http://blzddist1-a.akamaihd.net",
                },
            };
        }

        private static string GetDefaultCDNPath()
        {
            return "tpr/war3";
        }
    }
}