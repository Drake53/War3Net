// ------------------------------------------------------------------------------
// <copyright file="EKey.cs" company="Drake53">
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
    /// Represents a CASC encoded key (EKey).
    /// </summary>
    /// <remarks>
    /// <para>
    /// In the TACT system, an Encoding Hash/EKey is the MD5 hash of the potentially encoded file.
    /// For unencoded files, the <see cref="EKey"/> equals the <see cref="CascKey"/>. For chunkless
    /// <see cref="Compression.BlteDecoder"/> files without a chunk table, this hash covers the entire
    /// encoded file. For chunked BLTE files, this hash covers only the BLTE headers including the
    /// chunk table, as the chunk table contains hashes of each chunk's content.
    /// </para>
    /// <para>
    /// A single <see cref="CascKey"/> may have multiple <see cref="EKey"/> instances if the file can be
    /// encoded in different ways (e.g., encrypted vs unencrypted versions). The <see cref="EKey"/> is
    /// also referred to as the CDN Key since it's used to lookup files on the CDN through
    /// <see cref="Index.IndexFile"/> structures.
    /// </para>
    /// <para>
    /// EKeys can be variable length: typically 9 bytes (truncated) in some contexts like the Download Size
    /// file, or 16 bytes (full MD5) in most other contexts. The maximum size is 16 bytes.
    /// </para>
    /// <para>
    /// Encoded keys are used by:
    /// </para>
    /// <list type="bullet">
    /// <item><description><see cref="Encoding.EncodingFile"/> for mapping <see cref="CascKey"/> to EKey</description></item>
    /// <item><description><see cref="Index.IndexFile"/> for locating files in local archives</description></item>
    /// <item><description><see cref="Cdn.CdnConfig"/> archives for CDN file retrieval</description></item>
    /// </list>
    /// </remarks>
    public readonly struct EKey : IEquatable<EKey>
    {
        private readonly byte[] _key;
        private readonly int _length;

        /// <summary>
        /// Initializes a new instance of the <see cref="EKey"/> struct.
        /// </summary>
        /// <param name="key">The key bytes representing the MD5 hash of the encoded file content.</param>
        /// <param name="length">The actual length of the key. If not specified, uses the array length.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown when the key length exceeds 16 bytes or is negative, or when the source array is smaller than the requested length.</exception>
        public EKey(byte[] key, int? length = null)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var actualLength = length ?? key.Length;
            // EKeys can be variable length, but typically 9 bytes (truncated) or 16 bytes (full)
            // Maximum size is same as CKey (16 bytes)
            if (actualLength > CascConstants.CKeySize)
            {
                throw new ArgumentException($"Encoded key cannot be larger than {CascConstants.CKeySize} bytes.", nameof(key));
            }

            if (actualLength < 0)
            {
                throw new ArgumentException("Key length cannot be negative.", nameof(length));
            }

            if (key.Length < actualLength)
            {
                throw new ArgumentException($"Source array length {key.Length} is smaller than requested length {actualLength}.", nameof(key));
            }

            _length = actualLength;
            _key = new byte[_length];
            Array.Copy(key, _key, _length);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EKey"/> struct.
        /// </summary>
        /// <param name="key">The key bytes representing the MD5 hash of the encoded file content.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="key"/> length exceeds 16 bytes.</exception>
        public EKey(ReadOnlySpan<byte> key)
        {
            if (key.Length > CascConstants.CKeySize)
            {
                throw new ArgumentException($"Encoded key cannot be larger than {CascConstants.CKeySize} bytes.");
            }

            _length = key.Length;
            _key = new byte[_length];
            key.CopyTo(_key);
        }

        /// <summary>
        /// Gets an empty encoded key representing no encoded content.
        /// </summary>
        /// <value>An <see cref="EKey"/> instance with all bytes set to zero.</value>
        public static EKey Empty { get; } = CreateEmpty();

        private static EKey CreateEmpty()
        {
            return new EKey(new byte[CascConstants.EKeySize]);
        }

        /// <summary>
        /// Gets the key bytes as a read-only span.
        /// </summary>
        /// <value>A read-only span containing the encoded key bytes, or empty if the key is not initialized.</value>
        public ReadOnlySpan<byte> Value => _key ?? ReadOnlySpan<byte>.Empty;

        /// <summary>
        /// Gets the length of the encoded key in bytes.
        /// </summary>
        /// <value>The actual length of the key, which can be between 0 and 16 bytes.</value>
        public int Length => _length;

        /// <summary>
        /// Gets a value indicating whether this key is empty (all bytes are zero).
        /// </summary>
        /// <value><see langword="true"/> if the key is empty or not initialized; otherwise, <see langword="false"/>.</value>
        public bool IsEmpty => _key == null || _key.All(b => b == 0);

        /// <summary>
        /// Creates a truncated encoded key (9 bytes) from a full key.
        /// </summary>
        /// <param name="fullKey">The full key bytes to truncate.</param>
        /// <returns>A new <see cref="EKey"/> containing the first 9 bytes of the input key.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="fullKey"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// Truncated EKeys are commonly used in <see cref="Cdn.CdnConfig"/> files for size optimization.
        /// The truncated key maintains sufficient uniqueness for most lookup operations.
        /// </remarks>
        public static EKey CreateTruncated(byte[] fullKey)
        {
            if (fullKey == null)
            {
                throw new ArgumentNullException(nameof(fullKey));
            }

            var truncated = new byte[CascConstants.EKeySize];
            Array.Copy(fullKey, truncated, Math.Min(fullKey.Length, CascConstants.EKeySize));
            return new EKey(truncated);
        }

        /// <summary>
        /// Parses an encoded key from a hex string representation.
        /// </summary>
        /// <param name="hex">The hex string representing the encoded key hash.</param>
        /// <returns>The parsed <see cref="EKey"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="hex"/> is <see langword="null"/>, empty, has invalid length, or exceeds maximum key size.</exception>
        /// <exception cref="FormatException">Thrown when <paramref name="hex"/> contains invalid hexadecimal characters.</exception>
        /// <remarks>
        /// The method accepts hex strings with or without separators (hyphens or spaces).
        /// The resulting key can be used for lookups in <see cref="Index.IndexFile"/> or <see cref="Encoding.EncodingFile"/>.
        /// </remarks>
        public static EKey Parse(string hex)
        {
            if (string.IsNullOrEmpty(hex))
            {
                throw new ArgumentException("Hex string cannot be null or empty.", nameof(hex));
            }

            // Remove common separators for cleaner parsing
            var cleanHex = hex.Replace("-", string.Empty).Replace(" ", string.Empty);

            if (cleanHex.Length % 2 != 0)
            {
                throw new ArgumentException($"Invalid hex string length. Must be even number of characters, got {cleanHex.Length}.", nameof(hex));
            }

            if (cleanHex.Length > CascConstants.CKeySize * 2)
            {
                throw new ArgumentException($"Encoded key hex string too long. Maximum {CascConstants.CKeySize * 2} characters, got {cleanHex.Length}.", nameof(hex));
            }

            // Use efficient hex conversion (available in .NET 5+)
            var bytes = Convert.FromHexString(cleanHex);
            return new EKey(bytes);
        }

        /// <summary>
        /// Attempts to parse an encoded key from a hex string representation.
        /// </summary>
        /// <param name="hex">The hex string representing the encoded key hash.</param>
        /// <param name="key">When this method returns, contains the parsed <see cref="EKey"/> if parsing succeeded, or <see cref="Empty"/> if parsing failed.</param>
        /// <returns><see langword="true"/> if parsing succeeded; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// This method provides a safe way to parse hex strings without throwing exceptions.
        /// The method accepts hex strings with or without separators (hyphens or spaces).
        /// </remarks>
        public static bool TryParse(string hex, [NotNullWhen(true)] out EKey key)
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
        public bool Equals(EKey other)
        {
            if (_key == null)
            {
                return other._key == null;
            }

            if (other._key == null || _length != other._length)
            {
                return false;
            }

            return _key.SequenceEqual(other._key);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is EKey key && Equals(key);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            if (_key == null || _key.Length < 4)
            {
                return 0;
            }

            return BinaryPrimitives.ReadInt32LittleEndian(_key);
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
        /// Converts the encoded key to a byte array.
        /// </summary>
        /// <returns>A new byte array containing a copy of the encoded key bytes, or an empty array if the key is not initialized.</returns>
        /// <remarks>
        /// This method creates a defensive copy of the internal key data. The returned array
        /// can be safely used for lookups in <see cref="Index.IndexFile"/> or <see cref="Encoding.EncodingFile"/>.
        /// </remarks>
        public byte[] ToArray()
        {
            if (_key == null)
            {
                return Array.Empty<byte>();
            }

            var result = new byte[_length];
            Array.Copy(_key, result, _length);
            return result;
        }

        /// <summary>
        /// Equality operator.
        /// </summary>
        public static bool operator ==(EKey left, EKey right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Inequality operator.
        /// </summary>
        public static bool operator !=(EKey left, EKey right)
        {
            return !left.Equals(right);
        }
    }
}