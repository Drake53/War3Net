// ------------------------------------------------------------------------------
// <copyright file="ChecksumValidator.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Security.Cryptography;

namespace War3Net.IO.Casc.Utilities
{
    /// <summary>
    /// Provides checksum validation functionality for CASC files.
    /// </summary>
    public static class ChecksumValidator
    {
        /// <summary>
        /// Validates MD5 checksum of data.
        /// </summary>
        /// <param name="data">The data to validate.</param>
        /// <param name="expectedHash">The expected MD5 hash.</param>
        /// <returns>true if the checksum matches; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
        /// <exception cref="ArgumentException">Thrown when expectedHash is null or not the correct size.</exception>
        public static bool ValidateMD5(byte[] data, byte[] expectedHash)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (expectedHash == null || expectedHash.Length != CascConstants.MD5HashSize)
            {
                throw new ArgumentException($"Expected hash must be {CascConstants.MD5HashSize} bytes.", nameof(expectedHash));
            }

            using var md5 = MD5.Create();
            var actualHash = md5.ComputeHash(data);
            return CompareHashes(actualHash, expectedHash);
        }

        /// <summary>
        /// Validates MD5 checksum of a stream.
        /// </summary>
        /// <param name="stream">The stream to validate.</param>
        /// <param name="expectedHash">The expected MD5 hash.</param>
        /// <returns>true if the checksum matches; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when stream is null.</exception>
        /// <exception cref="ArgumentException">Thrown when expectedHash is null or not the correct size.</exception>
        public static bool ValidateMD5(Stream stream, byte[] expectedHash)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (expectedHash == null || expectedHash.Length != CascConstants.MD5HashSize)
            {
                throw new ArgumentException($"Expected hash must be {CascConstants.MD5HashSize} bytes.", nameof(expectedHash));
            }

            using var md5 = MD5.Create();
            var actualHash = md5.ComputeHash(stream);
            return CompareHashes(actualHash, expectedHash);
        }

        /// <summary>
        /// Computes MD5 hash of data.
        /// </summary>
        /// <param name="data">The data to hash.</param>
        /// <returns>The MD5 hash.</returns>
        /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
        public static byte[] ComputeMD5(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            using var md5 = MD5.Create();
            return md5.ComputeHash(data);
        }

        /// <summary>
        /// Validates SHA1 checksum of data.
        /// </summary>
        /// <param name="data">The data to validate.</param>
        /// <param name="expectedHash">The expected SHA1 hash.</param>
        /// <returns>true if the checksum matches; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
        /// <exception cref="ArgumentException">Thrown when expectedHash is null or not the correct size.</exception>
        public static bool ValidateSHA1(byte[] data, byte[] expectedHash)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (expectedHash == null || expectedHash.Length != CascConstants.SHA1HashSize)
            {
                throw new ArgumentException($"Expected hash must be {CascConstants.SHA1HashSize} bytes.", nameof(expectedHash));
            }

            using var sha1 = SHA1.Create();
            var actualHash = sha1.ComputeHash(data);
            return CompareHashes(actualHash, expectedHash);
        }

        /// <summary>
        /// Validates SHA256 checksum of data.
        /// </summary>
        /// <param name="data">The data to validate.</param>
        /// <param name="expectedHash">The expected SHA256 hash.</param>
        /// <returns>true if the checksum matches; otherwise, false.</returns>
        public static bool ValidateSHA256(byte[] data, byte[] expectedHash)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (expectedHash == null || expectedHash.Length != 32)
            {
                throw new ArgumentException("Expected hash must be 32 bytes.", nameof(expectedHash));
            }

            using var sha256 = SHA256.Create();
            var actualHash = sha256.ComputeHash(data);
            return CompareHashes(actualHash, expectedHash);
        }

        /// <summary>
        /// Compares two hash arrays for equality.
        /// </summary>
        /// <param name="hash1">The first hash.</param>
        /// <param name="hash2">The second hash.</param>
        /// <returns>true if the hashes are equal; otherwise, false.</returns>
        public static bool CompareHashes(byte[] hash1, byte[] hash2)
        {
            if (hash1.Length != hash2.Length)
            {
                return false;
            }

            // Use constant-time comparison to prevent timing attacks
            var result = 0;
            for (var i = 0; i < hash1.Length; i++)
            {
                result |= hash1[i] ^ hash2[i];
            }

            return result == 0;
        }
    }
}