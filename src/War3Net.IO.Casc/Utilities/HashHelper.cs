// ------------------------------------------------------------------------------
// <copyright file="HashHelper.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace War3Net.IO.Casc.Utilities
{
    /// <summary>
    /// Provides hashing utility methods for CASC.
    /// </summary>
    public static class HashHelper
    {
        /// <summary>
        /// Computes an MD5 hash of the input data.
        /// </summary>
        /// <param name="data">The data to hash.</param>
        /// <returns>The MD5 hash.</returns>
        public static byte[] ComputeMD5(byte[] data)
        {
            using var md5 = MD5.Create();
            return md5.ComputeHash(data);
        }

        /// <summary>
        /// Computes an MD5 hash of the input data.
        /// </summary>
        /// <param name="data">The data to hash.</param>
        /// <returns>The MD5 hash.</returns>
        public static byte[] ComputeMD5(ReadOnlySpan<byte> data)
        {
            using var md5 = MD5.Create();
            return md5.ComputeHash(data.ToArray());
        }

        /// <summary>
        /// Computes an MD5 hash of a string.
        /// </summary>
        /// <param name="text">The text to hash.</param>
        /// <returns>The MD5 hash.</returns>
        public static byte[] ComputeMD5(string text)
        {
            return ComputeMD5(System.Text.Encoding.UTF8.GetBytes(text));
        }

        /// <summary>
        /// Computes a SHA1 hash of the input data.
        /// </summary>
        /// <param name="data">The data to hash.</param>
        /// <returns>The SHA1 hash.</returns>
        public static byte[] ComputeSHA1(byte[] data)
        {
            using var sha1 = SHA1.Create();
            return sha1.ComputeHash(data);
        }

        /// <summary>
        /// Computes a SHA256 hash of the input data.
        /// </summary>
        /// <param name="data">The data to hash.</param>
        /// <returns>The SHA256 hash.</returns>
        public static byte[] ComputeSHA256(byte[] data)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(data);
        }

        /// <summary>
        /// Computes a Jenkins hash (lookup3) of the input string.
        /// </summary>
        /// <param name="text">The text to hash.</param>
        /// <returns>The Jenkins hash.</returns>
        public static uint ComputeJenkinsHash(string text)
        {
            return ComputeJenkinsHash(System.Text.Encoding.UTF8.GetBytes(text));
        }

        /// <summary>
        /// Computes a Jenkins hash (lookup3) of the input data.
        /// </summary>
        /// <param name="data">The data to hash.</param>
        /// <returns>The Jenkins hash.</returns>
        public static uint ComputeJenkinsHash(byte[] data)
        {
            return JenkinsHash.HashLittle(data);
        }

        /// <summary>
        /// Computes a Jenkins hash (lookup3) with initial value.
        /// </summary>
        /// <param name="data">The data to hash.</param>
        /// <param name="initval">The initial value.</param>
        /// <returns>The Jenkins hash.</returns>
        public static uint JenkinsHashLookup3(byte[] data, uint initval)
        {
            uint a, b, c;
            var length = data.Length;
            var offset = 0;

            // Set up the internal state
            a = b = c = 0xdeadbeef + (uint)length + initval;

            // Handle most of the key
            while (length > 12)
            {
                a += BitConverter.ToUInt32(data, offset);
                b += BitConverter.ToUInt32(data, offset + 4);
                c += BitConverter.ToUInt32(data, offset + 8);

                // Mix
                a -= c; a ^= Rot(c, 4); c += b;
                b -= a; b ^= Rot(a, 6); a += c;
                c -= b; c ^= Rot(b, 8); b += a;
                a -= c; a ^= Rot(c, 16); c += b;
                b -= a; b ^= Rot(a, 19); a += c;
                c -= b; c ^= Rot(b, 4); b += a;

                length -= 12;
                offset += 12;
            }

            // Handle the last few bytes
            switch (length)
            {
                case 12: c += BitConverter.ToUInt32(data, offset + 8); goto case 8;
                case 11: c += (uint)data[offset + 10] << 16; goto case 10;
                case 10: c += (uint)data[offset + 9] << 8; goto case 9;
                case 9: c += data[offset + 8]; goto case 8;
                case 8: b += BitConverter.ToUInt32(data, offset + 4); goto case 4;
                case 7: b += (uint)data[offset + 6] << 16; goto case 6;
                case 6: b += (uint)data[offset + 5] << 8; goto case 5;
                case 5: b += data[offset + 4]; goto case 4;
                case 4: a += BitConverter.ToUInt32(data, offset); break;
                case 3: a += (uint)data[offset + 2] << 16; goto case 2;
                case 2: a += (uint)data[offset + 1] << 8; goto case 1;
                case 1: a += data[offset]; break;
                case 0: return c; // Zero length requires no mixing
            }

            // Final mix
            c ^= b; c -= Rot(b, 14);
            a ^= c; a -= Rot(c, 11);
            b ^= a; b -= Rot(a, 25);
            c ^= b; c -= Rot(b, 16);
            a ^= c; a -= Rot(c, 4);
            b ^= a; b -= Rot(a, 14);
            c ^= b; c -= Rot(b, 24);

            return c;
        }

        /// <summary>
        /// Verifies an MD5 hash.
        /// </summary>
        /// <param name="data">The data to verify.</param>
        /// <param name="expectedHash">The expected hash.</param>
        /// <returns>true if the hash matches; otherwise, false.</returns>
        public static bool VerifyMD5(byte[] data, byte[] expectedHash)
        {
            if (expectedHash == null || expectedHash.Length != CascConstants.MD5HashSize)
            {
                return false;
            }

            var actualHash = ComputeMD5(data);
            return actualHash.SequenceEqual(expectedHash);
        }

        /// <summary>
        /// Verifies an MD5 hash from a stream.
        /// </summary>
        /// <param name="stream">The stream to verify.</param>
        /// <param name="expectedHash">The expected hash.</param>
        /// <returns>true if the hash matches; otherwise, false.</returns>
        public static bool VerifyMD5(Stream stream, byte[] expectedHash)
        {
            if (expectedHash == null || expectedHash.Length != CascConstants.MD5HashSize)
            {
                return false;
            }

            using var md5 = MD5.Create();
            var actualHash = md5.ComputeHash(stream);
            return actualHash.SequenceEqual(expectedHash);
        }

        /// <summary>
        /// Verifies a Jenkins hash.
        /// </summary>
        /// <param name="data">The data to verify.</param>
        /// <param name="expectedHash">The expected hash.</param>
        /// <returns>true if the hash matches; otherwise, false.</returns>
        public static bool VerifyJenkinsHash(byte[] data, uint expectedHash)
        {
            var actualHash = ComputeJenkinsHash(data);
            return actualHash == expectedHash;
        }

        private static uint Rot(uint x, int k)
        {
            return (x << k) | (x >> (32 - k));
        }
    }
}