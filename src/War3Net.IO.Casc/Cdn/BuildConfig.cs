// ------------------------------------------------------------------------------
// <copyright file="BuildConfig.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;

namespace War3Net.IO.Casc.Cdn
{
    /// <summary>
    /// Represents a build configuration file.
    /// </summary>
    public class BuildConfig
    {
        private readonly Dictionary<string, string> _data;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildConfig"/> class.
        /// </summary>
        public BuildConfig()
        {
            _data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the build key.
        /// </summary>
        public string? BuildKey => GetValue("build-uid");

        /// <summary>
        /// Gets the root content hash.
        /// </summary>
        public string? Root => GetValue("root");

        /// <summary>
        /// Gets the VFS root content hash (used by Warcraft III).
        /// </summary>
        public string? VfsRoot => GetValue("vfs-root");

        /// <summary>
        /// Gets the encoding hash.
        /// </summary>
        public string? Encoding => GetValue("encoding");

        /// <summary>
        /// Gets the install hash.
        /// </summary>
        public string? Install => GetValue("install");

        /// <summary>
        /// Gets the download hash.
        /// </summary>
        public string? Download => GetValue("download");

        /// <summary>
        /// Gets the size hash.
        /// </summary>
        public string? Size => GetValue("size");

        /// <summary>
        /// Gets the patch hash.
        /// </summary>
        public string? Patch => GetValue("patch");

        /// <summary>
        /// Gets the patch config hash.
        /// </summary>
        public string? PatchConfig => GetValue("patch-config");

        /// <summary>
        /// Parses a build config from a stream.
        /// </summary>
        /// <param name="stream">The stream to parse.</param>
        /// <returns>The parsed build config.</returns>
        public static BuildConfig Parse(Stream stream)
        {
            var config = new BuildConfig();
            using var reader = new StreamReader(stream);

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#", StringComparison.Ordinal))
                {
                    continue;
                }

                var equalIndex = line.IndexOf('=', StringComparison.Ordinal);
                if (equalIndex < 0)
                {
                    continue;
                }

                var key = line.Substring(0, equalIndex).Trim();
                var value = line.Substring(equalIndex + 1).Trim();

                config._data[key] = value;
            }

            return config;
        }

        /// <summary>
        /// Gets a value for a key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value, or null if not found.</returns>
        public string? GetValue(string key)
        {
            return _data.TryGetValue(key, out var value) ? value : null;
        }

        /// <summary>
        /// Gets all key-value pairs.
        /// </summary>
        /// <returns>A dictionary of all key-value pairs.</returns>
        public IReadOnlyDictionary<string, string> GetAll()
        {
            return _data;
        }
    }
}