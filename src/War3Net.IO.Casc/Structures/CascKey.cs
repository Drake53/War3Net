// ------------------------------------------------------------------------------
// <copyright file="CascKey.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace War3Net.IO.Casc.Structures
{
    /// <summary>
    /// Represents a CASC content key (CKey).
    /// </summary>
    /// <remarks>
    /// <para>
    /// In the TACT system, a Content Hash/CKey is the MD5 hash of the entire file in its uncompressed state,
    /// representing the purest form of the data. This is distinct from the <see cref="EKey"/> which represents
    /// the potentially encoded file. A single <see cref="CascKey"/> can map to multiple <see cref="EKey"/> instances
    /// if the file has multiple encoded representations (e.g., encrypted and unencrypted versions).
    /// </para>
    /// <para>
    /// CKeys are used throughout the CASC system to uniquely identify file content and are typically 16 bytes
    /// (MD5 hash size). They serve as the primary identifier for file data before any encoding or compression
    /// is applied. The <see cref="Encoding.EncodingFile"/> uses CKeys to look up corresponding <see cref="EKey"/>s
    /// for content retrieval.
    /// </para>
    /// <para>
    /// Content keys are referenced in:
    /// </para>
    /// <list type="bullet">
    /// <item><description><see cref="Cdn.BuildConfig"/> for root file and encoding file identification</description></item>
    /// <item><description><see cref="Encoding.EncodingFile"/> for CKey to <see cref="EKey"/> mappings</description></item>
    /// <item><description><see cref="Root.TvfsRootHandler"/> for mapping file paths to content hashes</description></item>
    /// </list>
    /// </remarks>
    public readonly struct CascKey : IEquatable<CascKey>
    {
        private readonly byte[] _key;

        /// <summary>
        /// Initializes a new instance of the <see cref="CascKey"/> struct.
        /// </summary>
        /// <param name="key">The key bytes representing the MD5 hash of the uncompressed file content.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="key"/> is not exactly 16 bytes in length.</exception>
        public CascKey(byte[] key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (key.Length != CascConstants.CKeySize)
            {
                throw new ArgumentException($"Content key must be exactly {CascConstants.CKeySize} bytes.", nameof(key));
            }

            _key = new byte[CascConstants.CKeySize];
            Array.Copy(key, _key, CascConstants.CKeySize);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascKey"/> struct.
        /// </summary>
        /// <param name="key">The key bytes representing the MD5 hash of the uncompressed file content.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="key"/> is not exactly 16 bytes in length.</exception>
        public CascKey(ReadOnlySpan<byte> key)
        {
            if (key.Length != CascConstants.CKeySize)
            {
                throw new ArgumentException($"Content key must be exactly {CascConstants.CKeySize} bytes.");
            }

            _key = new byte[CascConstants.CKeySize];
            key.CopyTo(_key);
        }

        /// <summary>
        /// Gets an empty content key representing no content.
        /// </summary>
        /// <value>A <see cref="CascKey"/> instance with all bytes set to zero.</value>
        public static CascKey Empty { get; } = new CascKey(new byte[CascConstants.CKeySize]);

        /// <summary>
        /// Gets the key bytes as a read-only span.
        /// </summary>
        /// <value>A read-only span containing the 16-byte MD5 hash, or empty if the key is not initialized.</value>
        public ReadOnlySpan<byte> Value => _key ?? ReadOnlySpan<byte>.Empty;

        /// <summary>
        /// Gets a value indicating whether this key is empty (all bytes are zero).
        /// </summary>
        /// <value><see langword="true"/> if the key is empty or not initialized; otherwise, <see langword="false"/>.</value>
        public bool IsEmpty => _key == null || _key.All(b => b == 0);

        /// <summary>
        /// Parses a content key from a hex string representation.
        /// </summary>
        /// <param name="hex">The hex string representing a 32-character (16-byte) MD5 hash.</param>
        /// <returns>The parsed <see cref="CascKey"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="hex"/> is <see langword="null"/>, empty, or has invalid length.</exception>
        /// <exception cref="FormatException">Thrown when <paramref name="hex"/> contains invalid hexadecimal characters.</exception>
        /// <remarks>
        /// The method accepts hex strings with or without separators (hyphens or spaces).
        /// The resulting key can be used to look up files in the <see cref="Encoding.EncodingFile"/>.
        /// </remarks>
        public static CascKey Parse(string hex)
        {
            if (string.IsNullOrEmpty(hex))
            {
                throw new ArgumentException("Hex string cannot be null or empty.", nameof(hex));
            }

            // Remove common separators for cleaner parsing
            var cleanHex = hex.Replace("-", string.Empty).Replace(" ", string.Empty);

            if (cleanHex.Length != CascConstants.CKeySize * 2)
            {
                throw new ArgumentException($"Invalid hex string length. Expected {CascConstants.CKeySize * 2} hex characters, got {cleanHex.Length}.", nameof(hex));
            }

            // Use efficient hex conversion (available in .NET 5+)
            var bytes = Convert.FromHexString(cleanHex);
            return new CascKey(bytes);
        }

        /// <summary>
        /// Attempts to parse a content key from a hex string representation.
        /// </summary>
        /// <param name="hex">The hex string representing a 32-character (16-byte) MD5 hash.</param>
        /// <param name="key">When this method returns, contains the parsed <see cref="CascKey"/> if parsing succeeded, or <see cref="Empty"/> if parsing failed.</param>
        /// <returns><see langword="true"/> if parsing succeeded; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// This method provides a safe way to parse hex strings without throwing exceptions.
        /// The method accepts hex strings with or without separators (hyphens or spaces).
        /// </remarks>
        public static bool TryParse(string hex, [NotNullWhen(true)] out CascKey key)
        {
            try
            {
                key = Parse(hex);
                return true;
            }
            catch
            {
                key = Empty;
                return false;
            }
        }

        /// <inheritdoc/>
        public bool Equals(CascKey other)
        {
            if (_key == null)
            {
                return other._key == null;
            }

            if (other._key == null)
            {
                return false;
            }

            return _key.SequenceEqual(other._key);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is CascKey key && Equals(key);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            if (_key == null || _key.Length < 4)
            {
                return 0;
            }

            // Use first 4 bytes for hash code
            return BinaryPrimitives.ReadInt32LittleEndian(_key.AsSpan(0, 4));
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (_key == null)
            {
                return string.Empty;
            }

            return Convert.ToHexString(_key);
        }

        /// <summary>
        /// Converts the content key to a byte array.
        /// </summary>
        /// <returns>A new byte array containing a copy of the 16-byte MD5 hash, or an empty array if the key is not initialized.</returns>
        /// <remarks>
        /// This method creates a defensive copy of the internal key data. The returned array
        /// can be safely used for lookups in <see cref="Encoding.EncodingFile"/> or other CASC operations.
        /// </remarks>
        public byte[] ToArray()
        {
            if (_key == null)
            {
                return Array.Empty<byte>();
            }

            var result = new byte[CascConstants.CKeySize];
            Array.Copy(_key, result, CascConstants.CKeySize);
            return result;
        }

        /// <summary>
        /// Equality operator.
        /// </summary>
        public static bool operator ==(CascKey left, CascKey right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Inequality operator.
        /// </summary>
        public static bool operator !=(CascKey left, CascKey right)
        {
            return !left.Equals(right);
        }
    }
}