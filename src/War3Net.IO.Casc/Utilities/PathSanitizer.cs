// ------------------------------------------------------------------------------
// <copyright file="PathSanitizer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace War3Net.IO.Casc.Utilities
{
    /// <summary>
    /// Provides centralized path sanitization functionality to prevent security vulnerabilities.
    /// </summary>
    public static class PathSanitizer
    {
        // Combine platform-specific invalid chars with additional problematic characters
        private static readonly char[] InvalidFileNameChars = GetInvalidFileNameChars();
        private static readonly Regex EncodedTraversalPattern = new Regex(@"%2e%2e|%252e|%c0%ae|%c1%9c", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly string[] DangerousPatterns = new[]
        {
            "..", "..\\", "../", "~", "$", "%",
            "\0", // Null byte
            ":", // Drive separator on Windows
        };

        private static char[] GetInvalidFileNameChars()
        {
            // Start with platform-specific invalid characters
            var platformChars = Path.GetInvalidFileNameChars();

            // Add additional characters that should be blocked on all platforms
            // These are characters that can cause issues with shell interpretation, HTML/XML injection, etc.
            var additionalChars = new char[] { '<', '>', '"', '|', '*', '?' };

            // Combine and remove duplicates
            return platformChars.Union(additionalChars).ToArray();
        }

        /// <summary>
        /// Sanitizes a file path for safe use within CASC storage.
        /// </summary>
        /// <param name="path">The path to sanitize.</param>
        /// <returns>The sanitized path.</returns>
        /// <exception cref="ArgumentException">Thrown when the path contains dangerous patterns.</exception>
        public static string SanitizeFilePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Path cannot be null or empty.", nameof(path));
            }

            // Normalize Unicode to prevent homograph attacks
            // Use NFC (Canonical Decomposition followed by Canonical Composition)
            path = path.Normalize(System.Text.NormalizationForm.FormC);

            // First, decode any URL-encoded sequences (safely)
            try
            {
                path = Uri.UnescapeDataString(path);
            }
            catch (UriFormatException)
            {
                // If the path contains malformed URL encoding, reject it
                throw new ArgumentException("Path contains malformed URL-encoded sequences.", nameof(path));
            }

            // Normalize again after decoding
            path = path.Normalize(System.Text.NormalizationForm.FormC);

            // Normalize path separators to forward slash
            path = path.Replace('\\', '/');

            // Check for absolute paths early (before we process segments)
            if (path.StartsWith('/') || Path.IsPathRooted(path))
            {
                throw new ArgumentException("Absolute paths are not allowed.", nameof(path));
            }

            // Remove any null bytes that could terminate the string early
            if (path.Contains('\0'))
            {
                throw new ArgumentException("Path contains null bytes.", nameof(path));
            }

            // Check for Unicode control characters and zero-width characters
            foreach (var c in path)
            {
                // Block control characters (except tab, newline, carriage return which are handled elsewhere)
                if (char.IsControl(c) && c != '\t' && c != '\n' && c != '\r')
                {
                    throw new ArgumentException($"Path contains control character U+{(int)c:X4}.", nameof(path));
                }

                // Block zero-width and non-printing Unicode characters
                var category = char.GetUnicodeCategory(c);
                if (category == System.Globalization.UnicodeCategory.Format ||
                    category == System.Globalization.UnicodeCategory.OtherNotAssigned ||
                    category == System.Globalization.UnicodeCategory.PrivateUse)
                {
                    throw new ArgumentException($"Path contains invalid Unicode character U+{(int)c:X4}.", nameof(path));
                }

                // Block specific problematic Unicode characters
                // Zero-width joiner/non-joiner, soft hyphen, etc.
                if (c == '\u200B' || c == '\u200C' || c == '\u200D' || c == '\u00AD' ||
                    c == '\uFEFF' || c == '\u202E' || c == '\u202D')
                {
                    throw new ArgumentException($"Path contains zero-width or directional Unicode character U+{(int)c:X4}.", nameof(path));
                }
            }

            // Check for encoded directory traversal attempts
            if (EncodedTraversalPattern.IsMatch(path))
            {
                throw new ArgumentException("Path contains encoded directory traversal sequences.", nameof(path));
            }

            // Split into segments and validate each
            var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            var sanitizedSegments = new string[segments.Length];

            for (var i = 0; i < segments.Length; i++)
            {
                var segment = segments[i];

                // Validate segment doesn't contain dangerous patterns
                foreach (var pattern in DangerousPatterns)
                {
                    if (segment.Contains(pattern, StringComparison.Ordinal))
                    {
                        throw new ArgumentException($"Path segment '{segment}' contains dangerous pattern '{pattern}'.", nameof(path));
                    }
                }

                // Check for invalid characters
                if (segment.IndexOfAny(InvalidFileNameChars) >= 0)
                {
                    throw new ArgumentException($"Path segment '{segment}' contains invalid characters.", nameof(path));
                }

                // Additional validation for special names
                if (IsReservedName(segment))
                {
                    throw new ArgumentException($"Path segment '{segment}' is a reserved name.", nameof(path));
                }

                sanitizedSegments[i] = segment;
            }

            // Rebuild the path
            var sanitizedPath = string.Join('/', sanitizedSegments);

            // Final validation
            if (Path.IsPathRooted(sanitizedPath))
            {
                throw new ArgumentException("Absolute paths are not allowed.", nameof(path));
            }

            return sanitizedPath;
        }

        /// <summary>
        /// Sanitizes a CDN path for safe URL construction.
        /// </summary>
        /// <param name="path">The path to sanitize.</param>
        /// <returns>The sanitized path.</returns>
        public static string SanitizeCdnPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Path cannot be null or empty.", nameof(path));
            }

            // Remove any leading/trailing whitespace
            path = path.Trim();

            // Check for null bytes
            if (path.Contains('\0'))
            {
                throw new ArgumentException("Path contains null bytes.", nameof(path));
            }

            // Normalize path separators
            path = path.Replace('\\', '/');

            // Remove double slashes efficiently using regex
            path = Regex.Replace(path, "/+", "/");

            // Validate no directory traversal
            if (path.Contains("../") || path.Contains("..\\") || path == ".." || path.StartsWith(".."))
            {
                throw new ArgumentException($"Path contains directory traversal sequences: {path}", nameof(path));
            }

            // Remove leading slashes (paths should be relative)
            path = path.TrimStart('/');

            // Check for absolute paths
            if (Path.IsPathRooted(path) || path.Contains(':'))
            {
                throw new ArgumentException($"Absolute paths are not allowed: {path}", nameof(path));
            }

            // Additional check for encoded traversal sequences
            var decodedPath = Uri.UnescapeDataString(path);
            if (decodedPath.Contains("../") || decodedPath.Contains("..\\"))
            {
                throw new ArgumentException($"Path contains encoded directory traversal sequences: {path}", nameof(path));
            }

            return path;
        }

        /// <summary>
        /// Validates that a path is safe for use.
        /// </summary>
        /// <param name="path">The path to validate.</param>
        /// <returns>true if the path is safe; otherwise, false.</returns>
        public static bool IsPathSafe(string path)
        {
            try
            {
                SanitizeFilePath(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if a name is reserved on Windows.
        /// </summary>
        /// <param name="name">The name to check.</param>
        /// <returns>true if the name is reserved; otherwise, false.</returns>
        private static bool IsReservedName(string name)
        {
            // Windows reserved names (case-insensitive)
            string[] reservedNames =
            {
                "CON", "PRN", "AUX", "NUL",
                "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
                "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9",
            };

            // Remove extension for checking
            var nameWithoutExt = Path.GetFileNameWithoutExtension(name);

            return reservedNames.Any(reserved =>
                string.Equals(nameWithoutExt, reserved, StringComparison.OrdinalIgnoreCase));
        }
    }
}