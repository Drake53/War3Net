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
        private readonly byte[]? _key;

        private EKey(byte[]? key)
        {
            _key = key;
        }

        /// <summary>
        /// Gets an empty encoded key representing no encoded content.
        /// </summary>
        public static EKey Empty => default;

        /// <summary>
        /// Gets the key bytes as a read-only span.
        /// </summary>
        /// <value>A read-only span containing the encoded key bytes, or empty if the key is not initialized.</value>
        public ReadOnlySpan<byte> Value => _key ?? ReadOnlySpan<byte>.Empty;

        /// <summary>
        /// Gets the length of the encoded key in bytes.
        /// </summary>
        public int Length => IsEmpty ? 0 : _key.Length;

        /// <summary>
        /// Gets a value indicating whether this key is empty.
        /// </summary>
        /// <value><see langword="true"/> if the key is empty; otherwise, <see langword="false"/>.</value>
        [MemberNotNullWhen(false, nameof(_key))]
        public bool IsEmpty => _key is null;

        /// <summary>
        /// Gets a value indicating whether this key has been truncated to 9 bytes.
        /// </summary>
        public bool IsPartial => Length == CascConstants.PartialEKeySize;

        public static bool operator ==(EKey left, EKey right) => left.Equals(right);

        public static bool operator !=(EKey left, EKey right) => !left.Equals(right);

        public static EKey FromBytes(ReadOnlySpan<byte> bytes)
        {
            if (bytes.Length != CascConstants.PartialEKeySize &&
                bytes.Length != CascConstants.EKeySize)
            {
                throw new ArgumentException($"Invalid byte array length. Must be {CascConstants.PartialEKeySize} or {CascConstants.EKeySize} bytes long.", nameof(bytes));
            }

            return new EKey(bytes.ToArray());
        }

        /// <summary>
        /// Parses an encoded key from a hex string representation.
        /// </summary>
        /// <param name="hex">The hex string representing the encoded key hash.</param>
        /// <returns>The parsed <see cref="EKey"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="hex"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="hex"/> has invalid length.</exception>
        /// <exception cref="FormatException">Thrown when <paramref name="hex"/> contains invalid hexadecimal characters.</exception>
        /// <remarks>
        /// The resulting key can be used for lookups in <see cref="Index.IndexFile"/> or <see cref="Encoding.EncodingFile"/>.
        /// </remarks>
        public static EKey Parse(string hex)
        {
            if (hex is null)
            {
                throw new ArgumentNullException(nameof(hex));
            }

            if (hex.Length != CascConstants.PartialEKeyStringLength &&
                hex.Length != CascConstants.EKeyStringLength)
            {
                throw new ArgumentException($"Invalid hex string length. Must be {CascConstants.PartialEKeyStringLength} or {CascConstants.EKeyStringLength} characters long.", nameof(hex));
            }

            return new EKey(Convert.FromHexString(hex));
        }

        /// <summary>
        /// Attempts to parse an encoded key from a hex string representation.
        /// </summary>
        /// <param name="hex">The hex string representing the encoded key hash.</param>
        /// <param name="key">When this method returns, contains the parsed <see cref="EKey"/> if parsing succeeded, or <see cref="Empty"/> if parsing failed.</param>
        /// <returns><see langword="true"/> if parsing succeeded; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// This method provides a safe way to parse hex strings without throwing exceptions.
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
        /// <remarks>
        /// Only the first 9 bytes are compared, allowing partial and full keys to be mixed for hashset/dictionary lookup and comparison.
        /// </remarks>
        public bool Equals(EKey other)
        {
            if (IsEmpty)
            {
                return other.IsEmpty;
            }

            if (other.IsEmpty)
            {
                return false;
            }

            return _key.AsSpan()[..CascConstants.PartialEKeySize].SequenceEqual(other._key.AsSpan()[..CascConstants.PartialEKeySize]);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is EKey key && Equals(key);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            if (IsEmpty || _key.Length < 4)
            {
                return 0;
            }

            return BinaryPrimitives.ReadInt32LittleEndian(_key.AsSpan(0, 4));
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return IsEmpty ? string.Empty : Convert.ToHexString(_key);
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
            if (IsEmpty)
            {
                return Array.Empty<byte>();
            }

            var result = new byte[_key.Length];
            Array.Copy(_key, result, _key.Length);
            return result;
        }
    }
}