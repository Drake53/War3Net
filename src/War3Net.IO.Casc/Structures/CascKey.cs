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
    public readonly struct CascKey : IEquatable<CascKey>
    {
        private readonly byte[] _key;

        /// <summary>
        /// Initializes a new instance of the <see cref="CascKey"/> struct.
        /// </summary>
        /// <param name="key">The key bytes.</param>
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
        /// <param name="key">The key bytes.</param>
        public CascKey(ReadOnlySpan<byte> key)
        {
            if (key.Length != CascConstants.CKeySize)
            {
                throw new ArgumentException($"Content key must be exactly {CascConstants.CKeySize} bytes.");
            }

            _key = new byte[CascConstants.CKeySize];
            key.CopyTo(_key);
        }

        // Static empty instance to avoid allocations
        private static readonly byte[] EmptyKeyBytes = new byte[CascConstants.CKeySize];
        
        /// <summary>
        /// Gets an empty content key.
        /// </summary>
        public static CascKey Empty { get; } = new CascKey(EmptyKeyBytes);

        /// <summary>
        /// Gets the key bytes.
        /// </summary>
        public ReadOnlySpan<byte> Value => _key ?? ReadOnlySpan<byte>.Empty;

        /// <summary>
        /// Gets a value indicating whether this key is empty.
        /// </summary>
        public bool IsEmpty => _key == null || _key.All(b => b == 0);

        /// <summary>
        /// Parses a content key from a hex string.
        /// </summary>
        /// <param name="hex">The hex string.</param>
        /// <returns>The parsed content key.</returns>
        public static CascKey Parse(string hex)
        {
            if (string.IsNullOrEmpty(hex))
            {
                throw new ArgumentException("Hex string cannot be null or empty.", nameof(hex));
            }

            // More efficient single-pass processing
            var bytes = new byte[CascConstants.CKeySize];
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
                    throw new ArgumentException($"Invalid hex string format.", nameof(hex));
                }
                
                char c2 = hex[charIndex + 1];
                
                // Skip separator in second char position
                if (c2 == '-' || c2 == ' ')
                {
                    throw new ArgumentException($"Invalid hex string format.", nameof(hex));
                }
                
                bytes[byteIndex] = Convert.ToByte(new string(new[] { c, c2 }), 16);
                byteIndex++;
                charIndex += 2;
            }
            
            if (byteIndex != CascConstants.CKeySize)
            {
                throw new ArgumentException($"Invalid hex string length. Expected {CascConstants.MD5StringSize} hex characters.", nameof(hex));
            }

            return new CascKey(bytes);
        }

        /// <summary>
        /// Attempts to parse a content key from a hex string.
        /// </summary>
        /// <param name="hex">The hex string.</param>
        /// <param name="key">The parsed content key.</param>
        /// <returns>true if parsing succeeded; otherwise, false.</returns>
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
        /// Converts the content key to a byte array.
        /// </summary>
        /// <returns>The key bytes.</returns>
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