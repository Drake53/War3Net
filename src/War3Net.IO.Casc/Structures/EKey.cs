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
    public readonly struct EKey : IEquatable<EKey>
    {
        private readonly byte[] _key;
        private readonly int _length;

        /// <summary>
        /// Initializes a new instance of the <see cref="EKey"/> struct.
        /// </summary>
        /// <param name="key">The key bytes.</param>
        /// <param name="length">The actual length of the key. If not specified, uses the array length.</param>
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
        /// <param name="key">The key bytes.</param>
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
        /// Gets an empty encoded key.
        /// </summary>
        public static EKey Empty { get; } = CreateEmpty();
        
        private static EKey CreateEmpty()
        {
            return default(EKey);
        }

        /// <summary>
        /// Gets the key bytes.
        /// </summary>
        public ReadOnlySpan<byte> Value => _key ?? ReadOnlySpan<byte>.Empty;

        /// <summary>
        /// Gets the length of the encoded key.
        /// </summary>
        public int Length => _length;

        /// <summary>
        /// Gets a value indicating whether this key is empty.
        /// </summary>
        public bool IsEmpty => _key == null || _key.All(b => b == 0);

        /// <summary>
        /// Creates a truncated encoded key (9 bytes) from a full key.
        /// </summary>
        /// <param name="fullKey">The full key bytes.</param>
        /// <returns>The truncated encoded key.</returns>
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
        /// Parses an encoded key from a hex string.
        /// </summary>
        /// <param name="hex">The hex string.</param>
        /// <returns>The parsed encoded key.</returns>
        public static EKey Parse(string hex)
        {
            if (string.IsNullOrEmpty(hex))
            {
                throw new ArgumentException("Hex string cannot be null or empty.", nameof(hex));
            }

            // Count actual hex chars in single pass
            int hexCharCount = 0;
            for (int i = 0; i < hex.Length; i++)
            {
                char c = hex[i];
                if (c != '-' && c != ' ')
                {
                    hexCharCount++;
                }
            }
            
            if (hexCharCount % 2 != 0)
            {
                throw new ArgumentException("Invalid hex string length.", nameof(hex));
            }

            var bytes = new byte[hexCharCount / 2];
            int byteIndex = 0;
            int charIndex = 0;
            
            while (byteIndex < bytes.Length && charIndex < hex.Length)
            {
                char c = hex[charIndex];
                
                // Skip separators
                if (c == '-' || c == ' ')
                {
                    charIndex++;
                    continue;
                }
                
                // Need at least 2 chars for a byte
                if (charIndex + 1 >= hex.Length)
                {
                    throw new ArgumentException("Invalid hex string format.", nameof(hex));
                }
                
                char c2 = hex[charIndex + 1];
                
                // Skip separator in second char position if needed
                while (c2 == '-' || c2 == ' ')
                {
                    charIndex++;
                    if (charIndex + 1 >= hex.Length)
                    {
                        throw new ArgumentException("Invalid hex string format.", nameof(hex));
                    }
                    c2 = hex[charIndex + 1];
                }
                
                bytes[byteIndex] = Convert.ToByte(new string(new[] { c, c2 }), 16);
                byteIndex++;
                charIndex += 2;
            }

            return new EKey(bytes);
        }

        /// <summary>
        /// Attempts to parse an encoded key from a hex string.
        /// </summary>
        /// <param name="hex">The hex string.</param>
        /// <param name="key">The parsed encoded key.</param>
        /// <returns>true if parsing succeeded; otherwise, false.</returns>
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

            return BitConverter.ToString(_key).Replace("-", string.Empty);
        }

        /// <summary>
        /// Converts the encoded key to a byte array.
        /// </summary>
        /// <returns>The key bytes.</returns>
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