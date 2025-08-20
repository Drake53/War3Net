// ------------------------------------------------------------------------------
// <copyright file="CdnPathHelper.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

using War3Net.IO.Casc.Structures;

namespace War3Net.IO.Casc.Helpers
{
    /// <summary>
    /// Helper class for constructing CDN paths.
    /// </summary>
    public static class CdnPathHelper
    {
        /// <summary>
        /// Constructs a CDN URL path segment from a hash and path type.
        /// </summary>
        /// <param name="pathType">The path type (data, config, or patch).</param>
        /// <param name="eKey">The hash string.</param>
        /// <returns>The URL path segment in the format: pathType/XX/YY/hash.</returns>
        public static string GetCdnUrlPath(string pathType, EKey eKey)
        {
            var hash = eKey.ToString().ToLowerInvariant();
            if (string.IsNullOrEmpty(hash))
            {
                throw new ArgumentException("Hash cannot be null or empty", nameof(hash));
            }

            if (hash.Length < 4)
            {
                throw new ArgumentException("Hash must be at least 4 characters long", nameof(hash));
            }

            return $"{pathType}/{hash.Substring(0, 2)}/{hash.Substring(2, 2)}/{hash}";
        }

        /// <summary>
        /// Constructs a CDN data file path from an EKey.
        /// </summary>
        /// <param name="storagePath">The base storage path.</param>
        /// <param name="ekey">The EKey.</param>
        /// <returns>The full path in the format: storagePath/Data/data/XX/YY/hash.</returns>
        public static string GetDataPath(string storagePath, EKey ekey)
        {
            var hash = ekey.ToString().ToLowerInvariant();
            if (string.IsNullOrEmpty(hash))
            {
                throw new ArgumentException("EKey cannot be empty", nameof(ekey));
            }

            if (hash.Length < 4)
            {
                throw new ArgumentException("EKey string must be at least 4 characters long", nameof(ekey));
            }

            return Path.Combine(
                storagePath,
                "Data",
                "data",
                hash.Substring(0, 2),
                hash.Substring(2, 2),
                hash);
        }

        /// <summary>
        /// Constructs a CDN config file path from a hash.
        /// </summary>
        /// <param name="storagePath">The base storage path.</param>
        /// <param name="hash">The hash string.</param>
        /// <returns>The full path in the format: storagePath/Data/config/XX/YY/hash.</returns>
        public static string GetConfigPath(string storagePath, string hash)
        {
            if (string.IsNullOrEmpty(hash))
            {
                throw new ArgumentException("Hash cannot be null or empty", nameof(hash));
            }

            hash = hash.ToLowerInvariant();
            if (hash.Length < 4)
            {
                throw new ArgumentException("Hash must be at least 4 characters long", nameof(hash));
            }

            return Path.Combine(
                storagePath,
                "Data",
                "config",
                hash.Substring(0, 2),
                hash.Substring(2, 2),
                hash);
        }

        /// <summary>
        /// Ensures the directory for a file path exists.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public static void EnsureDirectoryExists(string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}