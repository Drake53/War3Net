// ------------------------------------------------------------------------------
// <copyright file="ConfigBase.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace War3Net.IO.Casc.Cdn
{
    /// <summary>
    /// Abstract base class for TACT configuration files.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This base class provides common functionality for parsing and accessing configuration data
    /// from TACT system configuration files such as <see cref="BuildConfig"/> and <see cref="CdnConfig"/>.
    /// </para>
    /// <para>
    /// Configuration files use a simple key=value text format with comments starting with #.
    /// Values can be single or space-separated for multi-value fields.
    /// </para>
    /// </remarks>
    public abstract class ConfigBase
    {
        /// <summary>
        /// The underlying data storage for configuration key-value pairs.
        /// </summary>
        protected readonly Dictionary<string, string> _data;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigBase"/> class.
        /// </summary>
        protected ConfigBase()
        {
            _data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets a required configuration value.
        /// </summary>
        /// <param name="key">The configuration key to look up.</param>
        /// <returns>The configuration value.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the key is not found in the configuration.</exception>
        public string GetRequiredValue(string key)
        {
            if (!_data.TryGetValue(key, out var value))
            {
                throw new KeyNotFoundException($"Required configuration key '{key}' was not found in the {GetType().Name}.");
            }

            return value;
        }

        /// <summary>
        /// Gets an optional configuration value.
        /// </summary>
        /// <param name="key">The configuration key to look up.</param>
        /// <returns>The configuration value, or <see langword="null"/> if not found.</returns>
        public string? GetOptionalValue(string key)
        {
            return _data.TryGetValue(key, out var value) ? value : null;
        }

        /// <summary>
        /// Tries to get the value corresponding to the given configuration key.
        /// </summary>
        /// <param name="key">The configuration key to look up.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the key was found; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// This method provides a safe way to retrieve configuration values without throwing exceptions.
        /// Key lookups are case-insensitive.
        /// </remarks>
        public bool TryGetValue(string key, [NotNullWhen(true)] out string? value)
        {
            return _data.TryGetValue(key, out value);
        }

        /// <summary>
        /// Checks if a configuration key exists.
        /// </summary>
        /// <param name="key">The configuration key to check.</param>
        /// <returns><see langword="true"/> if the key exists; otherwise, <see langword="false"/>.</returns>
        public bool ContainsKey(string key)
        {
            return _data.ContainsKey(key);
        }

        /// <summary>
        /// Gets all key-value pairs.
        /// </summary>
        /// <returns>A read-only dictionary of all key-value pairs.</returns>
        public IReadOnlyDictionary<string, string> GetAll()
        {
            return _data;
        }

        /// <summary>
        /// Parses a configuration from a stream.
        /// </summary>
        /// <param name="stream">The stream containing configuration data.</param>
        /// <param name="config">The configuration instance to populate.</param>
        /// <typeparam name="T">The type of configuration being parsed.</typeparam>
        /// <returns>The populated configuration instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stream"/> is <see langword="null"/>.</exception>
        protected static T ParseConfig<T>(Stream stream, T config)
            where T : ConfigBase
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

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

                var key = line[..equalIndex].Trim();
                var value = line[(equalIndex + 1)..].Trim();

                config._data[key] = value;
            }

            return config;
        }

        /// <summary>
        /// Parses a space-separated multi-value field.
        /// </summary>
        /// <param name="value">The value string to parse.</param>
        /// <returns>A list of parsed values.</returns>
        protected static List<string> ParseMultiValue(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new List<string>();
            }

            return value.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        /// <summary>
        /// Parses a size value from a string.
        /// </summary>
        /// <param name="value">The value string to parse.</param>
        /// <returns>The parsed size, or 0 if invalid or not found.</returns>
        protected static long ParseSize(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }

            // Take the first value if there are multiple
            var parts = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length > 0 && long.TryParse(parts[0], out var size) ? size : 0;
        }

        /// <summary>
        /// Parses multiple size values from a string.
        /// </summary>
        /// <param name="value">The value string to parse.</param>
        /// <returns>An array of parsed sizes.</returns>
        protected static long[] ParseSizes(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Array.Empty<long>();
            }

            var parts = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var sizes = new List<long>();

            foreach (var part in parts)
            {
                if (long.TryParse(part, out var size))
                {
                    sizes.Add(size);
                }
            }

            return sizes.ToArray();
        }
    }
}