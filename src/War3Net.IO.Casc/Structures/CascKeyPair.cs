// ------------------------------------------------------------------------------
// <copyright file="CascKeyPair.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.IO.Casc.Structures
{
    /// <summary>
    /// Represents a pair of CASC content key (<see cref="CascKey"/>) and encoding key (<see cref="EKey"/>).
    /// </summary>
    /// <remarks>
    /// <para>
    /// In the TACT system, many configuration fields contain pairs of keys where the first
    /// value is the content hash (<see cref="CascKey"/>) of the decoded file and the second value (if present)
    /// is the encoding hash (<see cref="EKey"/>) of the encoded file. If the <see cref="EKey"/> is not present, it must
    /// be looked up in the <see cref="Encoding.EncodingFile"/> using the <see cref="CascKey"/>.
    /// </para>
    /// <para>
    /// This structure is commonly used in <see cref="Cdn.BuildConfig"/> files for fields like:
    /// <list type="bullet">
    /// <item>root: CKey of root file (EKey must be looked up)</item>
    /// <item>install: CKey and optional EKey of install manifest</item>
    /// <item>download: CKey and optional EKey of download manifest</item>
    /// <item>encoding: CKey and EKey of encoding file (both always present)</item>
    /// <item>size: CKey and EKey of download size file</item>
    /// </list>
    /// </para>
    /// <para>
    /// See also: <see cref="Encoding.EncodingEntry"/> for how these pairs are stored in the encoding file.
    /// </para>
    /// </remarks>
    public readonly struct CascKeyPair : IEquatable<CascKeyPair>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CascKeyPair"/> struct with both keys.
        /// </summary>
        /// <param name="cKey">The content key.</param>
        /// <param name="eKey">The encoding key.</param>
        public CascKeyPair(CascKey cKey, EKey eKey)
        {
            CKey = cKey;
            EKey = eKey;
            HasEKey = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascKeyPair"/> struct with only a content key.
        /// </summary>
        /// <param name="cKey">The content key.</param>
        public CascKeyPair(CascKey cKey)
        {
            CKey = cKey;
            EKey = EKey.Empty;
            HasEKey = false;
        }

        /// <summary>
        /// Gets an empty key pair.
        /// </summary>
        public static CascKeyPair Empty { get; } = new CascKeyPair(CascKey.Empty, EKey.Empty);

        /// <summary>
        /// Gets the content key (<see cref="CascKey"/>).
        /// </summary>
        /// <remarks>
        /// The <see cref="CascKey"/> is the MD5 hash of the uncompressed file content.
        /// </remarks>
        public CascKey CKey { get; }

        /// <summary>
        /// Gets the encoding key (<see cref="EKey"/>).
        /// </summary>
        /// <remarks>
        /// The <see cref="EKey"/> is the MD5 hash of the encoded file. May be empty if not provided
        /// and needs to be looked up in the <see cref="Encoding.EncodingFile"/>.
        /// </remarks>
        public EKey EKey { get; }

        /// <summary>
        /// Gets a value indicating whether this pair has an encoding key.
        /// </summary>
        /// <remarks>
        /// When <see langword="false"/>, the <see cref="EKey"/> must be looked up in the
        /// <see cref="Encoding.EncodingFile"/> using the <see cref="CKey"/>.
        /// </remarks>
        public bool HasEKey { get; }

        /// <summary>
        /// Gets a value indicating whether this key pair is empty.
        /// </summary>
        public bool IsEmpty => CKey.IsEmpty && EKey.IsEmpty;

        /// <summary>
        /// Parses a key pair from a space-separated string.
        /// </summary>
        /// <param name="value">The string containing one or two hex keys.</param>
        /// <returns>The parsed <see cref="CascKeyPair"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when the value format is invalid.</exception>
        public static CascKeyPair Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
            }

            var parts = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return parts.Length switch
            {
                1 => new CascKeyPair(CascKey.Parse(parts[0])),
                2 => new CascKeyPair(CascKey.Parse(parts[0]), EKey.Parse(parts[1])),
                _ => throw new ArgumentException($"Invalid key pair format. Expected 1 or 2 keys, got {parts.Length}.", nameof(value))
            };
        }

        /// <summary>
        /// Attempts to parse a key pair from a space-separated string.
        /// </summary>
        /// <param name="value">The string containing one or two hex keys.</param>
        /// <param name="keyPair">The parsed <see cref="CascKeyPair"/>.</param>
        /// <returns><see langword="true"/> if parsing succeeded; otherwise, <see langword="false"/>.</returns>
        public static bool TryParse(string value, out CascKeyPair keyPair)
        {
            try
            {
                keyPair = Parse(value);
                return true;
            }
            catch
            {
                keyPair = Empty;
                return false;
            }
        }

        /// <summary>
        /// Creates a key pair with an encoding key.
        /// </summary>
        /// <param name="cKey">The content key.</param>
        /// <param name="eKey">The encoding key.</param>
        /// <returns>A new <see cref="CascKeyPair"/> with both keys.</returns>
        public static CascKeyPair WithBothKeys(CascKey cKey, EKey eKey)
        {
            return new CascKeyPair(cKey, eKey);
        }

        /// <summary>
        /// Creates a key pair without an encoding key.
        /// </summary>
        /// <param name="cKey">The content key.</param>
        /// <returns>A new <see cref="CascKeyPair"/> with only a <see cref="CascKey"/>.</returns>
        public static CascKeyPair WithCKeyOnly(CascKey cKey)
        {
            return new CascKeyPair(cKey);
        }

        /// <summary>
        /// Updates this key pair with an encoding key.
        /// </summary>
        /// <param name="eKey">The encoding key to add.</param>
        /// <returns>A new <see cref="CascKeyPair"/> with the <see cref="EKey"/> added.</returns>
        public CascKeyPair WithEKey(EKey eKey)
        {
            return new CascKeyPair(CKey, eKey);
        }

        /// <inheritdoc/>
        public bool Equals(CascKeyPair other)
        {
            return CKey.Equals(other.CKey) &&
                   HasEKey == other.HasEKey &&
                   EKey.Equals(other.EKey);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is CascKeyPair pair && Equals(pair);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(CKey, EKey, HasEKey);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return HasEKey
                ? $"{CKey} {EKey}"
                : CKey.ToString();
        }

        /// <summary>
        /// Deconstructs the key pair into its components.
        /// </summary>
        /// <param name="cKey">The <see cref="CascKey"/> component.</param>
        /// <param name="eKey">The <see cref="EKey"/> component.</param>
        public void Deconstruct(out CascKey cKey, out EKey eKey)
        {
            cKey = CKey;
            eKey = EKey;
        }

        /// <summary>
        /// Deconstructs the key pair into its components with the <see cref="HasEKey"/> flag.
        /// </summary>
        /// <param name="cKey">The <see cref="CascKey"/> component.</param>
        /// <param name="eKey">The <see cref="EKey"/> component.</param>
        /// <param name="hasEKey">Whether the <see cref="EKey"/> is present.</param>
        public void Deconstruct(out CascKey cKey, out EKey eKey, out bool hasEKey)
        {
            cKey = CKey;
            eKey = EKey;
            hasEKey = HasEKey;
        }

        /// <summary>
        /// Determines whether two <see cref="CascKeyPair"/> instances are equal.
        /// </summary>
        /// <param name="left">The first <see cref="CascKeyPair"/> to compare.</param>
        /// <param name="right">The second <see cref="CascKeyPair"/> to compare.</param>
        /// <returns><see langword="true"/> if the instances are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(CascKeyPair left, CascKeyPair right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="CascKeyPair"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="CascKeyPair"/> to compare.</param>
        /// <param name="right">The second <see cref="CascKeyPair"/> to compare.</param>
        /// <returns><see langword="true"/> if the instances are not equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(CascKeyPair left, CascKeyPair right)
        {
            return !left.Equals(right);
        }
    }
}